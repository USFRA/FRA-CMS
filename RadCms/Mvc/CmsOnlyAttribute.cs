using RadCms.Helpers;
using System.Web.Mvc;

namespace RadCms.Mvc
{
    public class CmsOnlyAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // Prevent access to CMS controllers from public site
            if (!CmsHelper.IsCms)
            {
                throw new System.Exception("Page Not Found");
            }
        }
    }
}
