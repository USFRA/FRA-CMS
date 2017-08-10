using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Microsoft.Owin;
using Owin;
using RadCms.Data;
using RadCms.Helpers;
using RadCms.Services;
using RadCms.Models;

[assembly: OwinStartup(typeof(RadCms.Web.Startup))]

namespace RadCms.Web
{
    public partial class Startup
    {
        public void RegisterDependencies(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            builder.RegisterType<LayoutService>().As<ILayoutService>();
            //builder.RegisterType<PageBasedUrlHelper>().As<IPageUrlHelper>().SingleInstance();
            builder.RegisterType<IdBasedUrlHelper>().As<IPageUrlHelper>().SingleInstance();
            builder.RegisterType<CmsContext>().As<IDbContext>().InstancePerRequest();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
            builder.RegisterType<TreeModelService>().As<ITreeModelService<TreeModel>>();
            builder.RegisterType<JsTreeModelService>().As<ITreeModelService<JsTreeModel>>();

            builder.RegisterType<DriverCoordinator>().As<IDriverCoordinator>().InstancePerRequest();

            builder.RegisterAssemblyTypes(assemblies)
                   .Where(t=>t.Name.EndsWith("WebpartDriver") || t.Name.EndsWith("Service"))
                   //.Where(t => typeof(IWebpartDriver).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                   .AsImplementedInterfaces().InstancePerRequest();

#if PUB
            builder.RegisterType<PageEngine>()
                .As<IPageEngine>()
                .WithProperty(new NamedPropertyParameter("IsPublic", true));

#elif CMS
            builder.RegisterType<PageEngine>()
                .As<IPageEngine>()
                .WithProperty(new NamedPropertyParameter("IsPublic", false));

#endif

            // STANDARD MVC SETUP:

            // Register your MVC controllers.
            builder.RegisterControllers(assemblies);

            // Run other optional steps, like registering model binders,
            // web abstractions, etc., then set the dependency resolver
            // to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // OWIN MVC SETUP:

            // Register the Autofac middleware FIRST, then the Autofac MVC middleware.
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
        }
    }
}
