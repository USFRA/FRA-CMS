using System.Web.Mvc;

namespace RadCms.Web.Areas.ImageLibrary
{
    public class ImageLibraryAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ImageLibrary";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ImageLibrary_default",
                "ImageLibrary/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                name: "ImageLib",
                url: "ImageBrowser/{action}/{id}",
                defaults: new { Controller = "ImageBrowser", id = UrlParameter.Optional }
            );
        }
    }
}