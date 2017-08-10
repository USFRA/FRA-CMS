using System.Web.Mvc;

namespace RadCms.Web.Areas.Carousel
{
    public class CarouselAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Carousel";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Carousel_default",
                "Carousel/{controller}/{action}/{id}",
                new { controller = "ItemCms", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}