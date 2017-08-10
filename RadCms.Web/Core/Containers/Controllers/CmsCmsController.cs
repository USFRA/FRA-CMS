using System.Linq;
using System.Web.Mvc;
using RadCms.Entities;
using RadCms.Security;
using RadCms.Mvc;
using RadCms.Data;
using RadCms.Helpers;

namespace RadCms.Core.Containers.Controllers
{
    public class CmsCmsController: CmsControllerBase
    {
        private IDbContext _db;
        public CmsCmsController(IDbContext db)
        {
            _db = db;
        }
        public ActionResult Index()
        {
            ViewBag.IsUser = SecurityHelper.CurrentCmsUser(_db) != null;
            ViewBag.RoleId = SecurityHelper.CurrentCmsUserRole(_db);
            return View();
        }

        public JsonResult NaviAsJson(int id)
        {
            var naviNode = _db.Set<NaviNode>().Single(e => e.Id == id);

            return Json(new
            {
                Id = naviNode.Id,
                Title = naviNode.NodeName,
                ParentId = naviNode.Parent.Id
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDrafts()
        {
            var drafts = (_db.Set<CmsPage>().Where(e => e.ModifiedBy == User.Identity.Name.ToUpper()
                    && (e.Status != RadCms.Entities.CmsPage.STATUS_NORMAL
                    ))).Select(e => new
                    {
                        Id = e.Id,
                        Title = e.Title,
                        Modified = e.Modified,
                        ModifiedBy = e.ModifiedBy,
                        Status = e.Status,
                        Url = e.Url,
                        Type = "PAGE"
                    }).ToList();

            var otherDrafts = (_db.Set<CmsPage>().Where(e => e.ModifiedBy != User.Identity.Name.ToUpper()
                    && (e.Status != RadCms.Entities.CmsPage.STATUS_NORMAL
                    ))).Select(e => new
                    {
                        Id = e.Id,
                        Title = e.Title,
                        Modified = e.Modified,
                        ModifiedBy = e.ModifiedBy,
                        Status = e.Status,
                        Url = e.Url,
                        Type = "PAGE"
                    }).ToList();

            return Json(new
            {
                drafts = drafts,
                otherDrafts = otherDrafts
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Help()
        {
            return Redirect(CmsHelper.HelpPage);
        }
    }
}
