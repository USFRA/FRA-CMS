using RadCms.Data;
using RadCms.Entities;
using System.Linq;
using System.Web.Mvc;

namespace RadCms.Security
{
    public class CmsAuthorizeAttribute : AuthorizeAttribute
    {
        public string Target { get; set; }
        public PermissionType AccessMode { get; set; }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            using(var db = new CmsContext())
            {
                var user = db.Set<CmsUser>().SingleOrDefault(e => e.AdName.ToUpper() == httpContext.User.Identity.Name.ToUpper());
                if (user == null)
                {
                    return false;
                }

                return AccessMode <= SecurityHelper.TargetAccessMode(db, user, Target);
            }
        }
    }
}
