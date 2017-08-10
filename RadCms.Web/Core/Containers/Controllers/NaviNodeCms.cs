using System;
using System.Linq;
using System.Web.Mvc;
using RadCms.Entities;
using RadCms.Data;
using RadCms.Core.Containers.Models;
using RadCms.Mvc;

namespace RadCms.Core.Containers.Controllers
{
    public class NaviNodeCmsController : CmsControllerBase
    {
        private IRepository<NaviNode> _naviNodeRepo;

        public NaviNodeCmsController(IRepository<NaviNode> naviNodeRepo)
        {
            _naviNodeRepo = naviNodeRepo;
        }

        //
        // GET: /NaviNode/

        public ViewResult Index()
        {
            return View(_naviNodeRepo.GetAll().ToList());
        }

        //
        // GET: /NaviNode/Details/5

        public ViewResult Details(int id)
        {
            NaviNode naviNode = _naviNodeRepo.Get(id);
            return View(naviNode);
        }


        public ViewResult DetailsPanel(int id)
        {
            NaviNode naviNode = _naviNodeRepo.Get(id);
            return View(naviNode);
        }

        public JsonResult DetailsAsJson(int id)
        {
            NaviNode naviNode = _naviNodeRepo.Get(id);

            return Json(new
            {
                Id = naviNode.Id,
                Title = naviNode.NodeName,
                ParentId = naviNode.Parent.Id
            }, JsonRequestBehavior.AllowGet);
        }

        private void CopyProperties(EditNaviNodeModel editNaviNode, NaviNode naviNode)
        {
            naviNode.Id = editNaviNode.Id;
            naviNode.Parent = _naviNodeRepo.Get(editNaviNode.ParentId.Value);
            naviNode.NodeName = editNaviNode.NodeName;
            naviNode.DefaultPageId = editNaviNode.DefaultPageId;
            //naviNode.Breadcrumb = editNaviNode.Breadcrumb;

            naviNode.Modified = DateTime.Now;
            naviNode.ModifiedBy = User.Identity.Name.ToUpper();

            if (naviNode.CreatedBy == null)
            {
                naviNode.Created = DateTime.Now;
                naviNode.CreatedBy = User.Identity.Name.ToUpper();
            }
        }

        private void CopyProperties(NaviNode naviNode, EditNaviNodeModel editNaviNode)
        {
            editNaviNode.Id = naviNode.Id;
            editNaviNode.ParentId = naviNode.Parent.Id;
            editNaviNode.NodeName = naviNode.NodeName;
            editNaviNode.DefaultPageId = naviNode.DefaultPageId;
            //editNaviNode.Breadcrumb = naviNode.Breadcrumb;
        }

        //
        // GET: /NaviNode/Create
        // id is naviNodeId
        public ActionResult Create(int id)
        {
            NaviNode naviNode = _naviNodeRepo.Get(id);
            ViewBag.naviNode = naviNode;

            EditNaviNodeModel editNaviNodeModel = new EditNaviNodeModel();
            editNaviNodeModel.ParentId = naviNode.Id;

            return View(editNaviNodeModel);
        }

        //
        // POST: /NaviNode/Create

        [HttpPost]
        public ActionResult Create(EditNaviNodeModel editNaviNode)
        {
            if (ModelState.IsValid)
            {
                NaviNode naviNode = new NaviNode();
                CopyProperties(editNaviNode, naviNode);
                
                _naviNodeRepo.Add(naviNode);
                _naviNodeRepo.Save();

                return RedirectToAction("Navi", "Section", new { id = editNaviNode.ParentId });
            }

            return View(editNaviNode);
        }

        //
        // GET: /NaviNode/Edit/5

        public ActionResult Edit(int id)
        {
            NaviNode naviNode = _naviNodeRepo.Get(id);

            ViewBag.NaviNode = naviNode;

            EditNaviNodeModel editNaviNode = new EditNaviNodeModel();

            CopyProperties(naviNode, editNaviNode);

            return View(editNaviNode);
        }


        //
        // POST: /NaviNode/Edit/5

        [HttpPost]
        public ActionResult Edit(EditNaviNodeModel editNaviNode)
        {
            if (ModelState.IsValid)
            {
                NaviNode naviNode = _naviNodeRepo.Get(editNaviNode.Id);
                CopyProperties(editNaviNode, naviNode);

                _naviNodeRepo.Update(naviNode);
                _naviNodeRepo.Save();
                return RedirectToAction("Navi", "Section", new { id = editNaviNode.ParentId });
            }
            return View(editNaviNode);
        }

        public ActionResult EditPanel(int id)
        {
            NaviNode naviNode = _naviNodeRepo.Get(id);
            EditNaviNodeModel editNaviNode = new EditNaviNodeModel();

            CopyProperties(naviNode, editNaviNode);

            return View(editNaviNode);
        }

        [HttpPost]
        public ActionResult EditPanel(EditNaviNodeModel editNaviNode)
        {
            if (ModelState.IsValid)
            {
                NaviNode naviNode = _naviNodeRepo.Get(editNaviNode.Id);
                CopyProperties(editNaviNode, naviNode);

                _naviNodeRepo.Update(naviNode);
                _naviNodeRepo.Save();
                return RedirectToAction("DetailsPanel", new { id = editNaviNode.Id });
            }
            return View(editNaviNode);
        }

        //
        // GET: /NaviNode/Delete/5

        public ActionResult Delete(int id)
        {
            NaviNode navinode = _naviNodeRepo.Get(id);
            return View(navinode);
        }

        //
        // POST: /NaviNode/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            NaviNode navinode = _naviNodeRepo.Get(id);
            int parentId = navinode.Parent.Id;
            _naviNodeRepo.Delete(navinode);
            _naviNodeRepo.Save();
            return RedirectToAction("Navi", "Section", new { id = parentId });
        }

        public ActionResult Tree()
        {
            return View();
        }

        public JsonResult NaviAsJson(int id)
        {
            NaviNode naviNode = _naviNodeRepo.Get(id);

            return Json(new
            {
                Id = naviNode.Id,
                Title = naviNode.NodeName,
                ParentId = naviNode.Parent.Id
            }, JsonRequestBehavior.AllowGet);
        }

    }
}