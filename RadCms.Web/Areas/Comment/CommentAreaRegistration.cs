using System.Web.Mvc;

namespace RadCms.Web.Areas.Comment
{
    public class CommentAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Comment";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Comment_default",
                "Comment/{controller}/{action}/{id}",
                new { controller = "Comment", action = "Index", id = UrlParameter.Optional}
            );
        }
    }
}