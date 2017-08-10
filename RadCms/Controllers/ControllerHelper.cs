using System;
using System.Web.Mvc;

namespace RadCms.Controllers
{
    public static class ControllerHelper
    {
        public static bool IsModified(this Controller controller, DateTime updatedAt)
        {
            var headerValue = controller.Request.Headers["If-Modified-Since"];
            if (headerValue != null)
            {
                var modifiedSince = DateTime.Parse(headerValue).ToLocalTime();
                if (modifiedSince >= updatedAt)
                {
                    return false;
                }
            }

            return true;
        }

        public static ActionResult NotModified(this Controller controller)
        {
            return new HttpStatusCodeResult(304, "Page has not been modified");
        }
    }
}
