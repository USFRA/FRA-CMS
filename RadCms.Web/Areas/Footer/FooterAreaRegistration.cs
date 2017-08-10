using System.Web.Mvc;

namespace RadCms.Web.Areas.Footer
{
    public class FooterAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Footer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Footer_default",
                "Footer/{controller}/{action}/{id}",
                new { controller = "FooterItemCms", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}