using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data;
using RadCms.Models;
using RadCms.Entities;
using RadCms.Helpers;
using RadCms.Services;
using RadCms.Data;
using RadCms.Mvc;

namespace RadCms.Web.Areas.ContentTree.Controllers
{
    public class TreeCmsController : CmsControllerBase
    {
        private IPageUrlHelper _urlHelper;
        private IDbContext _db;
        private ITreeModelService<JsTreeModel> _treeModelService;

        public TreeCmsController(IPageUrlHelper helper,
            IDbContext db,
            ITreeModelService<JsTreeModel> treeModelService)
        {
            _db = db;
            _treeModelService = treeModelService;
            _urlHelper = helper;
        }

        [OutputCache(Duration = 3600)]
        public JsonResult Node(string id)
        {
            //Response.CacheControl = "no-cache";
            //Response.Cache.SetETag((Guid.NewGuid()).ToString());
            if (String.IsNullOrEmpty(id))
            {
                id = "n1";
            }
            int parentId = Convert.ToInt32(id.Substring(1));

            return Json(_treeModelService.GetChildren(parentId), JsonRequestBehavior.AllowGet);
        }
        /*
        public JsonResult NodeFolder(string key)
        {
            //Response.CacheControl = "no-cache";
            //Response.Cache.SetETag((Guid.NewGuid()).ToString());

            int parentId = Convert.ToInt32(key.Substring(1));

            return Json(_treeModelService.GetChildrenFolders(parentId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult MoveToSection(string sourceNode, string newSectionName)
        {

            var sourceTreeNode = new JsTreeModel
            {
                Id = Convert.ToInt32(sourceNode.Substring(1)),
                Type = sourceNode.Substring(0, 1)
            };

            if (sourceTreeNode.Type == "P")
            {
                CmsPage page = _db.Set<CmsPage>().Find(sourceTreeNode.Id);
                NaviNode naviNode = new NaviNode();
                naviNode.NodeName = newSectionName;
                naviNode.Parent = page.NaviNode;
                naviNode.MenuOrder = page.MenuOrder;

                naviNode.Modified = DateTime.Now;
                naviNode.ModifiedBy = User.Identity.Name.ToUpper();
                naviNode.Created = DateTime.Now;
                naviNode.CreatedBy = User.Identity.Name.ToUpper();

                page.NaviNode = naviNode;
                page.MenuOrder = 0;

                _urlHelper.UpdatePageUrl(page);

                ((DbContext)_db).Entry(page).State = EntityState.Modified;
                _db.SaveChanges();
            }

            return Json(new { Message = "success" });
        }
        */
        public ActionResult MoveJsTree(string parentId, int position, string nodeId)
        {
            if(parentId == "#")
            {
                parentId = "n1";
            }

            int naviNodeid = Convert.ToInt32(parentId.Substring(1));
            var children = _treeModelService.GetChildren(naviNodeid).OrderBy(e => e.MenuOrder);

            if (position == 0)
            {
                if(children.Count() > 0)
                {
                    return RedirectToAction("Move", new { targetNode = children.First().id, sourceNode = nodeId, hitMode = "before" });
                }

                return RedirectToAction("Move", new { targetNode = parentId, sourceNode = nodeId, hitMode = "over" });
            }
            else
            {
                return RedirectToAction("Move", new { targetNode = children.Where(e=>e.id != nodeId).ElementAt(position - 1).id, sourceNode = nodeId, hitMode = "after" });
            }
        }

        /// <summary>
        /// Move tree view elements and change menu item order
        /// </summary>
        /// <param name="targetNode">Tree node is being dropped upon</param>
        /// <param name="sourceNode">Tree node is being dropped</param>
        /// <param name="hitMode">Hit mode: over, before, or after</param>
        /// <returns></returns>
        public JsonResult Move(string targetNode, string sourceNode, string hitMode)
        {
            NaviNode parent;

            var targetTreeNode = new JsTreeModel
            {
                Id = Convert.ToInt32(targetNode.Substring(1)),
                Type = targetNode.Substring(0, 1)
            };

            var sourceTreeNode = new JsTreeModel
            {
                Id = Convert.ToInt32(sourceNode.Substring(1)),
                Type = sourceNode.Substring(0, 1)
            };

            if (targetTreeNode.Type == "P")
            {
                // Dropped to a page
                parent = _db.Set<CmsPage>().Find(targetTreeNode.Id).NaviNode;
            }
            else
            {
                // Dropped to a node
                if (hitMode == "over")
                {
                    parent = _db.Set<NaviNode>().Find(targetTreeNode.Id);
                    targetTreeNode = null;
                }
                else
                {
                    parent = _db.Set<NaviNode>().Find(targetTreeNode.Id).Parent;
                }
            }

            if (sourceTreeNode.Type == "P")
            {
                //move around a page
                CmsPage p = _db.Set<CmsPage>().Find(sourceTreeNode.Id);
                if (p.NaviNode.Id != parent.Id)
                {
                    p.NaviNode = parent;
                    _urlHelper.UpdatePageUrl(p);
                    ((DbContext)_db).Entry(p).State = EntityState.Modified;
                }
            }
            else
            {
                //move around a node
                NaviNode n = _db.Set<NaviNode>().Find(sourceTreeNode.Id);
                if (n.Parent.Id != parent.Id)
                {
                    n.Parent = parent;
                    ((DbContext)_db).Entry(n).State = EntityState.Modified;

                    var subpages = _db.Set<CmsPage>().Where(e => e.NaviNode.Id == n.Id).ToList();
                    subpages.ForEach(e => _urlHelper.UpdatePageUrl(e));
                }
            }

            _db.SaveChanges();

            MoveTo(parent, targetTreeNode, sourceTreeNode, hitMode);

            _db.SaveChanges();

            return Json(new { Message = "success" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ToList()
        {
            return Json(_db.Set<NaviNode>().ToList<NaviNode>().OrderBy(e => e.MenuOrder));
        }

        //public JsonResult RemoveSection(int id)
        //{
        //    var node = _db.Set<NaviNode>().SingleOrDefault(e => e.Id == id);
        //    if(node!= null)
        //    {
        //        RemoveNode(node);
        //        _db.SaveChanges();
        //    }
        //    return Json(new { });
        //}
        //private void RemoveNode(NaviNode node)
        //{
        //    node.SubNodes.ToList().ForEach(n =>
        //    {
        //        RemoveNode(n);
        //    });

        //    _db.Set<NaviNode>().Remove(node);
        //}

        private void UpdateMenuOrder(JsTreeModel t)
        {
            if (t.Type == "N")
            {
                NaviNode n = _db.Set<NaviNode>().Find(t.Id);

                if (n.MenuOrder != t.MenuOrder)
                {
                    n.MenuOrder = t.MenuOrder;
                    ((DbContext)_db).Entry(n).State = EntityState.Modified;
                }
            }
            else
            {
                CmsPage p = _db.Set<CmsPage>().Find(t.Id);

                if (p.MenuOrder != t.MenuOrder)
                {
                    p.MenuOrder = t.MenuOrder;
                    ((DbContext)_db).Entry(p).State = EntityState.Modified;
                }
            }
        }

        private void MoveTo(NaviNode parent, JsTreeModel targetTreeNode, JsTreeModel sourceTreeNode, string hitMode)
        {
            updateMenuOrders(parent, targetTreeNode, sourceTreeNode, hitMode);
            updateUrlForMovedNode(sourceTreeNode);
            _db.SaveChanges();
        }

        private void updateUrlForMovedNode(JsTreeModel sourceTreeNode)
        {
            // TODO Need optimization
            if (sourceTreeNode.Type == "N")
            {
                var n = _db.Set<NaviNode>().Find(sourceTreeNode.Id);
                _urlHelper.UpdatePageUrl(n);
            }
            else
            {
                var p = _db.Set<CmsPage>().Find(sourceTreeNode.Id);
                _urlHelper.UpdatePageUrl(p);
            }
        }

        private void updateMenuOrders(NaviNode parent, JsTreeModel targetTreeNode, JsTreeModel sourceTreeNode, string hitMode)
        {
            List<JsTreeModel> children = _treeModelService.GetChildren(parent.Id).ToList<JsTreeModel>();

            int index = 0;

            if (targetTreeNode == null)
            {
                index = children.Count;
            }
            else
            {
                int pos = 0;

                foreach (var t in children)
                {
                    if (t.id == targetTreeNode.id)
                    {
                        switch (hitMode)
                        {
                            case "before":
                                index = pos;
                                break;

                            case "after":
                                index = pos + 1;
                                break;
                        }

                        break;
                    }

                    pos++;
                }
            }

            sourceTreeNode.MenuOrder = 9999;

            int count = index + 1;
            for (int i = index; i < children.Count; i++)
            {
                if (children[i].id == sourceTreeNode.id)
                {
                    continue;
                }

                children[i].MenuOrder = count;
                UpdateMenuOrder(children[i]);
                count++;
            }

            sourceTreeNode.MenuOrder = index;
            UpdateMenuOrder(sourceTreeNode);
        }
    }
}
