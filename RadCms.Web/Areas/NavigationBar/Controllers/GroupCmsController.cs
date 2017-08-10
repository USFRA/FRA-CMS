using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using RadCms.Data;

namespace RadCms.Web.Areas.NavigationBar.Controllers
{
    using RadCms.Entities;
    using Mvc;
    using Models;

    public class GroupCmsController: CmsControllerBase
    {
        private IRepository<NavGroup> _navGroupRepo;
        private IRepository<NavItem> _navItemRepo;

        public GroupCmsController(IRepository<NavGroup> navGroupRepo, 
            IRepository<NavItem> navItemRepo)
        {
            _navGroupRepo = navGroupRepo;
            _navItemRepo = navItemRepo;
        }

        public ViewResult Index()
        {
            return View(_navGroupRepo.GetAll().Where(e => e.Parent == null).OrderBy(e => e.Index).ToList());
        }

        public ViewResult Details(int id)
        {
            NavGroup headernav = _navGroupRepo.Get(id);
            return View(headernav);
        }

        public ViewResult Items(int id)
        {
            var group = _navGroupRepo.Get(id);
            var items = GetSubItems(group);
            ViewBag.Items = items;
            return View(group);
        }

        public ActionResult Create(int id = -1)
        {
            var parent = _navGroupRepo.Get(id);
            ViewBag.Parent = parent;
            return View();
        }

        public ActionResult AddItem(int id, bool isGroup)
        {
            if (isGroup)
            {
                return RedirectToAction("Create", new { id = id });
            }
            else
            {
                return View(new NavItem
                {
                    Group = _navGroupRepo.Get(id)
                });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddItem(NavItem item, int groupId)
        {
            if (ModelState.IsValid)
            {
                var group = _navGroupRepo.Get(groupId);
                if (group == null)
                {
                    ModelState.AddModelError("Group", "Nav Group is missing.");
                }
                else
                {
                    item.Group = group;
                    _navItemRepo.Add(item);
                    _navItemRepo.Save();
                    return RedirectToAction("Items", new { id = item.Group.Id });
                }
            }
            return View(item);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(NavGroup headernav, int parentId)
        {
            if (ModelState.IsValid)
            {
                if (parentId != -1)
                {
                    headernav.Parent = _navGroupRepo.Get(parentId);
                }
                _navGroupRepo.Add(headernav);
                _navGroupRepo.Save();
                return RedirectToAction("Index");
            }

            return View(headernav);
        }
        
        public ActionResult Edit(int id, bool isGroup)
        {
            if (isGroup)
            {
                NavGroup headernav = _navGroupRepo.Get(id);
                return View(headernav);
            }
            else
            {
                NavItem item = _navItemRepo.Get(id);
                return View("EditItem", item);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(NavGroup headernav, NavItem item, int parentId, bool isGroup)
        {
            if (isGroup)
            {
                headernav.Parent = _navGroupRepo.Get(parentId);
                _navGroupRepo.Update(headernav);
                _navGroupRepo.Save();
                return RedirectToAction("Index");
            }
            else
            {
                item.Group = _navGroupRepo.Get(parentId);
                _navItemRepo.Update(item);
                _navItemRepo.Save();
                return RedirectToAction("Items", new {id = parentId });
            }
        }

        public ActionResult Delete(int id, bool isGroup)
        {
            if (isGroup)
            {
                NavGroup headernav = _navGroupRepo.Get(id);
                return View(headernav);
            }
            else
            {
                NavItem item = _navItemRepo.Get(id);
                return View("DeleteItem", item);
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, bool isGroup)
        {
            if (isGroup)
            {
                NavGroup group = _navGroupRepo.Get(id);
                RemoveAllChildren(group);
            }
            else
            {
                NavItem item = _navItemRepo.Get(id);
                _navItemRepo.Delete(item);
            }

            // Transaction problem?
            _navItemRepo.Save();
            _navGroupRepo.Save();
            return RedirectToAction("Index");
        }

        private void RemoveAllChildren(NavGroup group)
        {
            if (group != null)
            {
                var subitems = _navItemRepo.GetAll().Where(e => e.Group.Id == group.Id);
                _navItemRepo.DeleteAll(subitems);

                var subgroups = _navGroupRepo.GetAll().Where(e => e.Parent.Id == group.Id);
                foreach (var g in subgroups)
                {
                    RemoveAllChildren(g);
                }

                _navGroupRepo.Delete(group);
            }

        }

        private static IList<MenuItem> GetSubItems(NavGroup pageNode)
        {
            var pages = pageNode.Items.Select(e => new MenuItem { Type = MenuItem.ItemType.Page, Id = e.Id, Url = e.Link, MenuOrder = e.Index, Title = e.Text }).OrderBy(e => e.MenuOrder).ToList();
            var nodes = pageNode.SubGroups.Select(e =>
            {
                var overviewPage = e.Items.OrderBy(p => p.Index).FirstOrDefault();
                var url = "";
                var overviewId = -1;
                if (overviewPage != null)
                {
                    overviewId = overviewPage.Id;
                    url = overviewPage.Link;
                }
                var item = new MenuItem { Type = MenuItem.ItemType.Section, OverviewId = overviewId, Id = e.Id, Url = url, MenuOrder = e.Index, Title = e.Title };

                return item;
            }).ToList();
            return pages.Union(nodes).OrderBy(e => e.MenuOrder).ToList();
        }
    }
}