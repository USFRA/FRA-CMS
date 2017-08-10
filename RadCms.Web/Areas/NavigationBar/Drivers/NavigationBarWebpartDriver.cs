using System.Collections.Generic;
using System.Linq;
using RadCms.Data;
using RadCms.Entities;
using RadCms.Helpers;
using RadCms.Mvc.ViewEngines.Razor;
using RadCms.Web.Areas.NavigationBar.Models;

namespace RadCms.Web.Areas.HeaderNav.Drivers
{
    public class NavigationBarWebpartDriver: IWebpartDriver
    {
        private DriverContext _context;
        private IRepository<NavGroup> _repo;

        public NavigationBarWebpartDriver(IRepository<NavGroup> repo)
        {
            _repo = repo;
        }

        public string WebpartId
        {
            get
            {
                return "NavigationBar";
            }
        }

        public void Apply(DriverContext context)
        {
            _context = context;
        }

        public DriverResult BuildDisplay()
        {
            var items = GetItems();
            return new DriverResult
            {
                Content = BuildContent(_context, items)
            };
        }

        public DriverResult BuildEditor()
        {
            return BuildDisplay();
        }

        private static string BuildContent(DriverContext context, List<MenuItem> items)
        {
            return ViewRenderer.RenderViewToString(context.ControllerContext, "~/Areas/NavigationBar/Views/Item/_BarPartial.cshtml", items, true);
        }

        private List<MenuItem> GetItems()
        {
            var rootGroup = _repo.GetAll().Where(e=>e.Parent == null).ToList();
            var rootItems = new List<MenuItem>();
            foreach(var g in rootGroup.OrderBy(e => e.Index))
            {
                rootItems.Add(new MenuItem
                {
                    Id = g.Id,
                    Type = MenuItem.ItemType.Section,
                    MenuOrder = g.Index,
                    SubItems = GetSubItems(g),
                    Title = g.Title,
                    Url = g.Link
                });
            }
            return rootItems;
        }

        private static IList<MenuItem> GetSubItems(NavGroup group)
        {
            var pages = group.Items.Select(e => new MenuItem { Type = MenuItem.ItemType.Page, Id = e.Id, Url = e.Link, MenuOrder = e.Index, Title = e.Text }).OrderBy(e => e.MenuOrder).ToList();
            var nodes = group.SubGroups.Select(e =>
            {
                var item = new MenuItem { Type = MenuItem.ItemType.Section, Id = e.Id, Url = e.Link, MenuOrder = e.Index, Title = e.Title, SubItems = GetSubItems(e) };

                return item;
            }).ToList();
            return pages.Union(nodes).OrderBy(e => e.MenuOrder).ToList();
        }
    }
}