using System.Web.Mvc;
using System.Web.Routing;

namespace RadCms.Core
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("robots.txt");

            AreaRegistration.RegisterAllAreas();

            routes.MapRoute(
                name: "Search",
                url: "Search",
                defaults: new { controller = "Search", action = "Search" }
            );

            routes.MapRoute(
                name: "Sitemap",
                url: "Sitemap.xml",
                defaults: new { controller = "Sitemap", action = "Index" }
            );

            routes.MapRoute(
                name: "CmsApp",
                url: "CmsApp/{action}/{id}",
                defaults: new { Controller = "CmsApp", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "RadCms.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{*id}",
                defaults: new { controller = "Cms", action = "Page", id = "Home" },
                namespaces: new[] { "RadCms.Core.Containers.Controllers" }
            );
        }
    }
}