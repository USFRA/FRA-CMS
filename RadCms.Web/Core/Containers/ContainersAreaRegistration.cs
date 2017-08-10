using System.Web.Mvc;

namespace RadCms.Core.Containers
{
    public class ContainersAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Containers";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Error404",
                url: "Error404",
                defaults: new { controller = "Cms", action = "Error404" }
            );

            context.MapRoute(
                name: "Error500",
                url: "Error500",
                defaults: new { controller = "Cms", action = "Error500" }
            );

            context.MapRoute(
                name: "Page",
                url: "Page/{id}",
                defaults: new { controller = "Cms", action = "Page" },
                constraints: new { id = @"P\d+" }
            );

            context.MapRoute(
                name: "EditPage",
                url: "EditPage/{id}",
                defaults: new { controller = "Page", action = "Edit" },
                constraints: new { id = @"P\d+" },
                namespaces: new[] { "RadCms.Core.Containers.Controllers" }
            );

            context.MapRoute(
                name: "VPage",
                url: "VPage/{id}",
                defaults: new { controller = "Page", action = "Version" },
                constraints: new { id = @"\d+" },
                namespaces: new[] { "RadCms.Core.Containers.Controllers" }
            );

            context.MapRoute(
                name: "CreatePage",
                url: "CreatePage/{id}",
                defaults: new { controller = "Page", action = "Create" },
                constraints: new { id = @"\d+" },
                namespaces: new[] { "RadCms.Core.Containers.Controllers" }
            );

            context.MapRoute(
                name: "Containers_default",
                url : "Containers/{controller}/{action}/{id}",
                defaults : new { action = "Index", id = UrlParameter.Optional },
                namespaces : new[] { "RadCms.Core.Containers.Controllers" }
            );

            //context.MapRoute(
            //    name: "Default",
            //    url: "{*id}",
            //    defaults: new { controller = "Cms", action = "Page", id = "Home" },
            //    namespaces: new[] { "RadCms.Core.Containers.Controllers" }
            //);
        }
    }
}