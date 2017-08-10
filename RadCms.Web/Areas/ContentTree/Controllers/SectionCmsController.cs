using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RadCms.Entities;
using Newtonsoft.Json;
using RadCms.Helpers;
using RadCms.Data;
using RadCms.Mvc;

namespace RadCms.Web.Areas.ContentTree.Controllers
{
    public class SectionCmsController : CmsControllerBase
    {

        private IPageUrlHelper _urlHelper;
        private IDbContext _db;
        public SectionCmsController(IPageUrlHelper helper, IDbContext db)
        {
            this._db = db;
            this._urlHelper = helper;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Navi(int id)
        {
            NaviNode naviNode = _db.Set<NaviNode>().Single(e => e.Id == id);

            return View(naviNode);
        }

        public ActionResult MovePages(int id)
        {
            var page = _db.Set<CmsPage>().Find(id);
            NaviNode naviNode = page.NaviNode;
            List<string> pathToExpand = new List<string>();
            InsertPath(pathToExpand, naviNode);
            ViewBag.PathToExpand = JsonConvert.SerializeObject(pathToExpand);
            ViewBag.ExpandTo = CmsPage.ToFriendlyId(id);
            return View(naviNode);
        }

        public JsonResult RenameSection(int id, string name)
        {
            NaviNode naviNode = _db.Set<NaviNode>().SingleOrDefault(e => e.Id == id);
            if (naviNode != null)
            {
                var siblings = naviNode.Parent.SubNodes.Where(e => e.Id != naviNode.Id);
                if (siblings.Where(e => e.NodeName.Equals(name, StringComparison.InvariantCultureIgnoreCase)).Count() > 0)
                {
                    return Json(new
                    {
                        status = "error",
                        message = "Duplicate section name."
                    });
                }
                else
                {
                    naviNode.NodeName = name;
                    _urlHelper.UpdatePageUrl(naviNode);
                    _db.SaveChanges();

                    return Json(new
                    {
                        status = "success",
                        message = "Section name is updated."
                    });
                }
            }
            else
            {
                return Json(new
                {
                    status = "error",
                    message = "Section does not exist. Please refresh the page."
                });
            }
        }

        public ActionResult RenamePages(int id)
        {
            NaviNode naviNode = _db.Set<NaviNode>().Single(e => e.Id == id);

            return View(naviNode);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Section/Edit/5

        //public ActionResult Edit(int id)
        //{
        //    ViewPageModel viewPage = new ViewPageModel();

        //    viewPage.Page = db.Pages.Single(e => e.Id == id);

        //    List<HrefLink> breadcrumb = new List<HrefLink>();
        //    List<HrefLink> leftNavi = new List<HrefLink>();

        //    if (viewPage.Page.NaviNodeId.HasValue)
        //    {
        //        var url = new UrlHelper(Request.RequestContext);

        //        NaviNode naviNode =
        //            db.NaviNodes.SingleOrDefault(e => e.Id == viewPage.Page.NaviNodeId);

        //        while (naviNode != null)
        //        {
        //            HrefLink link = new HrefLink();

        //            link.Text = naviNode.NodeName;
        //            link.Title = naviNode.NodeName;

        //            if (naviNode.DefaultPageId.HasValue)
        //            {
        //                link.Url = url.Action("Page", "ELib", new { id = naviNode.DefaultPageId });
        //            }
        //            else
        //            {
        //                link.Url = "#";
        //            }

        //            breadcrumb.Insert(0, link);

        //            naviNode = db.NaviNodes.SingleOrDefault(e => e.Id == naviNode.ParentId);
        //        }

        //        //left navigation

        //        //Pages under current navigation node
        //        var pages = from p in db.Pages
        //                    where p.NaviNodeId == viewPage.Page.NaviNodeId
        //                    select new
        //                    {
        //                        Id = p.Id,
        //                        Title = p.Title
        //                    };

        //        foreach (var p in pages)
        //        {
        //            if (p.Id == id) continue;

        //            HrefLink link = new HrefLink();

        //            link.Text = p.Title;
        //            link.Title = p.Title;

        //            link.Url = url.Action("Page", "ELib", new { id = p.Id });

        //            leftNavi.Insert(0, link);
        //        }

        //        //Children navi nodes
        //        var children = from p in db.NaviNodes
        //                       where p.ParentId == viewPage.Page.NaviNodeId
        //                       select new
        //                       {
        //                           Id = p.DefaultPageId,
        //                           Title = p.NodeName
        //                       };

        //        foreach (var p in children)
        //        {
        //            if (p.Id == id) continue;

        //            HrefLink link = new HrefLink();

        //            link.Text = p.Title;
        //            link.Title = p.Title;

        //            if (p.Id.HasValue)
        //            {
        //                link.Url = url.Action("Page", "ELib", new { id = p.Id });
        //            }
        //            else
        //            {
        //                link.Url = "#";
        //            }

        //            leftNavi.Insert(0, link);
        //        }

        //    }

        //    viewPage.Breadcrumb = breadcrumb;
        //    viewPage.LeftNavi = leftNavi;

        //    return View(viewPage);
        //}

        //
        // POST: /Section/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private void InsertPath(List<string> pathToExpand, NaviNode node)
        {
            pathToExpand.Insert(0, "N" + node.Id);
            if (node.Parent != null && node.Parent.Parent != null)
            {
                InsertPath(pathToExpand, node.Parent);
            }
        }
    }
}
