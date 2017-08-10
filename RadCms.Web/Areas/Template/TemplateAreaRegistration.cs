using System.Web.Mvc;

namespace RadCms.Web.Areas.Template
{
    public class TemplateAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Template";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Template_default",
                "Template/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                name: "Layout",
                url: "Layout/{action}/{id}",
                defaults: new { controller = "Layout", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}