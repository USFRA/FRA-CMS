using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using RadCms.Entities;
using RadCms.Core.Containers.Models;
using RadCms.Data;
using RadCms.Mvc;

namespace RadCms.Core.Containers.Controllers
{
    public class NaviHeadingCmsController : CmsControllerBase
    {
        private IDbContext db;

        public NaviHeadingCmsController(IDbContext db)
        {
            this.db = db;
        }

        //
        // GET: /NaviHeading/

        public ViewResult Index()
        {
            return View(db.Set<NaviHeading>().ToList());
        }

        //
        // GET: /NaviHeading/Details/5

        public ViewResult Details(int id)
        {
            NaviHeading naviheading = db.Set<NaviHeading>().Find(id);
            return View(naviheading);
        }

        private void CopyProperties(EditNaviHeadingModel editNaviHeading, NaviHeading naviHeading)
        {
            naviHeading.Url = editNaviHeading.Url;
            naviHeading.Description = editNaviHeading.Description;
            naviHeading.HeadingOrder = editNaviHeading.HeadingOrder;
            naviHeading.NaviNode = db.Set<NaviNode>().SingleOrDefault(e => e.Id == editNaviHeading.NaviNodeId);
        }

        private void CopyProperties(NaviHeading naviHeading, EditNaviHeadingModel editNaviHeading)
        {
            editNaviHeading.Url = naviHeading.Url;
            editNaviHeading.Description = naviHeading.Description;
            editNaviHeading.HeadingOrder = editNaviHeading.HeadingOrder;
            editNaviHeading.NaviNodeId = naviHeading.NaviNode.Id;
        }

        //
        // GET: /NaviHeading/Create
        // id is naviNodeId
        public ActionResult Create(int id)
        {
            NaviNode naviNode = db.Set<NaviNode>().Single(e => e.Id == id);
            ViewBag.naviNode = naviNode;

            EditNaviHeadingModel editNaviHeading = new EditNaviHeadingModel();
            editNaviHeading.NaviNodeId = naviNode.Id;

            return View(editNaviHeading);
        }

        //
        // POST: /NaviHeading/Create

        [HttpPost]
        public ActionResult Create(EditNaviHeadingModel editNaviHeading)
        {
            if (ModelState.IsValid)
            {
                NaviHeading naviHeading = new NaviHeading();

                CopyProperties(editNaviHeading, naviHeading);

                db.Set<NaviHeading>().Add(naviHeading);
                db.SaveChanges();

                return RedirectToAction("Navi", "Section", new { id = editNaviHeading.NaviNodeId });
            }

            return View(editNaviHeading);
        }

        //
        // GET: /NaviHeading/Edit/5

        public ActionResult Edit(int id)
        {
            NaviHeading naviHeading = db.Set<NaviHeading>().Single(e => e.Id == id);
            ViewBag.NaviNode = naviHeading.NaviNode;

            EditNaviHeadingModel editNaviHeading = new EditNaviHeadingModel();

            CopyProperties(naviHeading, editNaviHeading);

            return View(editNaviHeading);
        }

        //
        // POST: /NaviHeading/Edit/5

        [HttpPost]
        public ActionResult Edit(EditNaviHeadingModel editNaviHeading)
        {
            if (ModelState.IsValid)
            {
                NaviHeading naviHeading = db.Set<NaviHeading>().Single(e => e.Id == editNaviHeading.Id);
                CopyProperties(editNaviHeading, naviHeading);
                ((DbContext)db).Entry(naviHeading).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Navi", "Section", new { id = editNaviHeading.NaviNodeId });
            }
            return View(editNaviHeading);
        }

        //
        // GET: /NaviHeading/Delete/5

        public ActionResult Delete(int id)
        {
            NaviHeading naviheading = db.Set<NaviHeading>().Find(id);
            return View(naviheading);
        }

        //
        // POST: /NaviHeading/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            NaviHeading naviheading = db.Set<NaviHeading>().Find(id);
            int parentId = naviheading.NaviNode.Id;
            db.Set<NaviHeading>().Remove(naviheading);
            db.SaveChanges();
            return RedirectToAction("Navi", "Section", new { id = parentId });
        }
    }
}