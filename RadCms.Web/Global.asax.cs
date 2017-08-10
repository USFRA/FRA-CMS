using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using RadCms.Core;
using RadCms.Mvc.ViewEngines.Razor;
using RadCms.Mvc;

namespace RadCms.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ControllerBuilder.Current.SetControllerFactory(typeof(RadCmsControllerFactory));

            ViewEngines.Engines.Clear();
            var viewEngine = new RadCmsRazorViewEngine();
            ViewEngines.Engines.Add(viewEngine);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}