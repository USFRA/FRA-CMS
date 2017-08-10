using System.Web.Mvc;

namespace RadCms.Web.Areas.NavigationBar
{
    public class NavigationBarAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "NavigationBar";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "NavigationBar_default",
                "NavigationBar/{controller}/{action}/{id}",
                new { controller = "GroupCMS", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}