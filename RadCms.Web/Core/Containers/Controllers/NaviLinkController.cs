using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using RadCms.Core.Containers.Models;
using RadCms.Data;
using RadCms.Entities;
using RadCms.Mvc;

namespace RadCms.Core.Containers.Controllers
{
    public class NaviLinkCmsController : CmsControllerBase
    {
        private IDbContext db;

        public NaviLinkCmsController(IDbContext db)
        {
            this.db = db;
        }

        //
        // GET: /NaviLink/

        public ViewResult Index()
        {
            return View(db.Set<NaviLink>().ToList());
        }

        //
        // GET: /NaviLink/Details/5

        public ViewResult Details(int id)
        {
            NaviLink navilink = db.Set<NaviLink>().Find(id);
            return View(navilink);
        }

        private void CopyProperties(EditNaviLinkModel editNaviLink, NaviLink naviLink)
        {
            naviLink.Url = editNaviLink.Url;
            naviLink.Description = editNaviLink.Description;
            naviLink.LinkOrder = editNaviLink.LinkOrder;
            naviLink.NaviHeading = db.Set<NaviHeading>().SingleOrDefault(e => e.Id == editNaviLink.NaviHeadingId);
        }

        private void CopyProperties(NaviLink naviLink, EditNaviLinkModel editNaviLink)
        {
            editNaviLink.Url = naviLink.Url;
            editNaviLink.Description = naviLink.Description;
            editNaviLink.LinkOrder = editNaviLink.LinkOrder;
            editNaviLink.NaviHeadingId = naviLink.NaviHeading.Id;
        }

        //
        // GET: /NaviLink/Create
        // id is naviHeadingId
        public ActionResult Create(int id)
        {
            NaviNode naviNode = db.Set<NaviNode>().Single(e => e.Id == id);
            ViewBag.NaviNode = naviNode;

            EditNaviLinkModel editNaviLink = new EditNaviLinkModel();

            ViewBag.Headings = naviNode.NaviHeadings.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Description
            });

            return View(editNaviLink);
        } 

        //
        // POST: /NaviLink/Create

        [HttpPost]
        public ActionResult Create(EditNaviLinkModel editNaviLink)
        {
            if (ModelState.IsValid)
            {
                NaviLink naviLink = new NaviLink();

                CopyProperties(editNaviLink, naviLink);

                db.Set<NaviLink>().Add(naviLink);
                db.SaveChanges();
                return RedirectToAction("Navi", "Section", new { id = naviLink.NaviHeading.NaviNode.Id });  
            }

            return View(editNaviLink);
        }
        
        //
        // GET: /NaviLink/Edit/5
 
        public ActionResult Edit(int id)
        {
            NaviLink naviLink = db.Set<NaviLink>().Single(e => e.Id == id);
            ViewBag.NaviNode = naviLink.NaviHeading.NaviNode;

            EditNaviLinkModel editNaviLink = new EditNaviLinkModel();

            CopyProperties(naviLink, editNaviLink);

            ViewBag.Headings = naviLink.NaviHeading.NaviNode.NaviHeadings.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Description
            });

            return View(editNaviLink);
        }

        //
        // POST: /NaviLink/Edit/5

        [HttpPost]
        public ActionResult Edit(EditNaviLinkModel editNaviLink)
        {
            if (ModelState.IsValid)
            {
                NaviLink naviLink = db.Set<NaviLink>().Single(e => e.Id == editNaviLink.Id);
                CopyProperties(editNaviLink, naviLink);

                ((DbContext)db).Entry(naviLink).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Navi", "Section", new { id = naviLink.NaviHeading.NaviNode.Id });
            }
            return View(editNaviLink);
        }

        //
        // GET: /NaviLink/Delete/5
 
        public ActionResult Delete(int id)
        {
            NaviLink navilink = db.Set<NaviLink>().Find(id);
            return View(navilink);
        }

        //
        // POST: /NaviLink/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            NaviLink navilink = db.Set<NaviLink>().Find(id);
            int parentId = navilink.NaviHeading.NaviNode.Id;

            db.Set<NaviLink>().Remove(navilink);
            db.SaveChanges();
            return RedirectToAction("Navi", "Section", new { id = parentId });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}