using System.Collections.Generic;
using System.Linq;
using System.Text;
using RadCms.Entities;
using RadCms.Data;
using RadCms.Helpers;

namespace RadCms.Models
{
    public class MenuBuilder
    {
        public int PageId { set; get; }
        public NaviNode PageNode { set; get; }
        public bool IsPublic { get; set; }
        private IRepository<PubPage> _pubPageRepo;
        private IPageUrlHelper _urlHelper;

        public MenuBuilder(IRepository<PubPage> pubPageRepo, IPageUrlHelper urlHelper)
        {
            _pubPageRepo = pubPageRepo;
            _urlHelper = urlHelper;
        }
        public MenuBuilder(int pageId, NaviNode pageNode, IPageUrlHelper urlHelper)
        {
            PageId = pageId;
            PageNode = pageNode;
            _urlHelper = urlHelper;
        }

        private StringBuilder sb { set; get; }

        public string ToHtmlString()
        {
            var expandedSectionId = CmsPageBase.FindExpandableNode(PageNode);
            NaviNode baseNode = CmsPageBase.FindBaseNodeForMenu(PageNode);
            sb = sb ?? new StringBuilder();
            appendAllChildSections(expandedSectionId, baseNode);
            return sb.ToString();
        }

        //Amar Added for blogs Side menu 02/23/2015
        public string SectionsToHtmlString()
        {
            //NaviNode baseNode = CmsPage.FindBaseNodeForMenu(this.PageNode);
            sb = sb ?? new StringBuilder();
            appendAllChildSections(PageNode);
            return sb.ToString();
        }

        private IEnumerable<NaviNode> getSameLevelNodes(NaviNode node)
        {
            if (node == null)
            {
                return new List<NaviNode>();
            }
            else if (node.Parent == null)
            {
                var result = new List<NaviNode>();
                result.Add(node);
                return result;
            }
            else
            {
                
                return node.Parent.SubNodes.OrderBy(e => e.MenuOrder );
            }
        }
        private void appendAllChildSections(int expandedSectionId, NaviNode baseNode)
        {
            sb.AppendLine("<ul>");
            var parentNodes = baseNode.Parent.SubNodes.OrderBy(e => e.MenuOrder);
            var isFirstSection = true;
            foreach (var n in parentNodes)
            {
                IPage sectionDefaultPage = getSubPages(_pubPageRepo, n, IsPublic)
                        .OrderBy(e => e.MenuOrder)
                        .FirstOrDefault();
                if (sectionDefaultPage == null)
                {
                    continue;
                }
                if (isFirstSection)
                {
                    isFirstSection = false;
                }
                else
                {
                    sb.AppendLine("<div class='section-divider'></div>");
                }

                if (n.Id == baseNode.Id)
                {
                    #region current section

                    var menuItems = getAllMenuItems(_pubPageRepo, baseNode, IsPublic);
                    if (menuItems != null)
                    {
                        sb.AppendLine("<li class='menu-body-wrapper'>");

                        if (sectionDefaultPage.Id == PageId)
                        {
                            sb.AppendFormat("<a class='section-header selected' href='/{0}'>{1}</a>", _urlHelper.GetPageUrl(sectionDefaultPage), baseNode.NodeName);
                        }
                        else
                        {
                            sb.AppendFormat("<a class='section-header' href='/{0}'>{1}</a>", _urlHelper.GetPageUrl(sectionDefaultPage), baseNode.NodeName);
                        }

                        appendMenuItems(PageId, expandedSectionId, sb, menuItems);
                        sb.AppendLine("</li>");
                    }

                    #endregion
                }
                else
                {
                    sb.AppendLine("<li class='menu-body-wrapper'>");
                    sb.AppendFormat("<a class='section-header' href='/{0}'>{1}</a>", _urlHelper.GetPageUrl(sectionDefaultPage), n.NodeName);
                    sb.AppendLine("</li>");
                }
            }

            sb.AppendLine("</ul>");
        }


        //Amar Added for blogs Side menu 02/23/2015
        private void appendAllChildSections(NaviNode baseNode)
        {
            sb.AppendLine("<ul>");
            var parentNodes = getSameLevelNodes(baseNode);
            var isFirstSection = true;
            foreach (var n in parentNodes)
            {
                if (n.Type.Title != "BLOG")
                    continue;
                if (n.Hidden ==true)
                    continue;
                if (isFirstSection)
                {
                    isFirstSection = false;
                }
                else
                {
                    sb.AppendLine("<div class='section-divider'></div>");
                }

                var sectionDefaultPage = n.Pages.OrderBy(e => e.MenuOrder).FirstOrDefault();
               
                if (n.Id == baseNode.Id)
                {
                    #region current section
                    sb.AppendLine("<li class='menu-body-wrapper' style='background-color: #eafaff;'>");
           
                    #endregion
                }
                else
                {
                    sb.AppendLine("<li class='menu-body-wrapper'>");
           

                }
                sb.AppendFormat("<a class='section-header' href='/{0}'>{1}</a>", sectionDefaultPage == null ? "#" : _urlHelper.GetPageUrl(sectionDefaultPage), n.NodeName);
              
                 sb.AppendLine("</li>");
            }

            sb.AppendLine("</ul>");
        }

        /// <summary>
        /// Append all menu items to stringbuilder
        /// </summary>
        /// <param name="pageId">Selected page id, which will be highlighted in the menu</param>
        /// <param name="nodeName">Current section title</param>
        /// <param name="sb">Menu html builder</param>
        /// <param name="menuItems">Menu item list</param>
        private void appendMenuItems(int pageId, int expandId, StringBuilder sb, IList<MenuItem> menuItems)
        {
            if (sb == null || menuItems == null)
            {
                return;
            }

            if (menuItems.Count > 0)
            {
                sb.AppendLine("<ul>");
                foreach (var item in menuItems)
                {
                    if (item.Type == MenuItem.ItemType.Section)
                    {
                        sb.Append("<li class='");
                        sb.AppendLine("subsection");

                        if (item.Id == expandId)
                        {
                            sb.AppendLine(" expanded");
                        }

                        sb.Append("'>");

                        if (item.OverviewId == pageId)
                        {
                            sb.AppendFormat("<a class='selected' href='/{0}' target='_self'>{1}</a>", item.Url, item.Title);
                        }
                        else
                        {
                            sb.AppendFormat("<a href='/{0}' target='_self'>{1}</a>", item.Url, item.Title);
                        }

                        appendMenuItems(pageId, expandId, sb, item.SubItems);
                    }
                    else
                    {
                        sb.AppendLine("<li>");
                        if (item.Id == pageId)
                        {
                            sb.AppendFormat("<a class='selected' href='/{0}' target='_self'>{1}</a>", item.Url, item.Title);
                        }
                        else
                        {
                            sb.AppendFormat("<a href='/{0}' target='_self'>{1}</a>", item.Url, item.Title);
                        }
                    }

                    sb.AppendLine("</li>");
                }
                sb.AppendLine("</ul>");
            }
        }

        private IList<MenuItem> getAllMenuItems(IRepository<PubPage> pubPageRepo, NaviNode pageNode, bool isPublic)
        {

            var pages = getSubPages(pubPageRepo, pageNode, isPublic).Select(e => new MenuItem { Type = MenuItem.ItemType.Page, Id = e.Id, Url = _urlHelper.GetPageUrl(e), MenuOrder = e.MenuOrder, Title = e.Title }).OrderBy(e => e.MenuOrder).ToList();
            var usedAsOverview = pages.FirstOrDefault();

            if (usedAsOverview == null)
            {
                return null;
            }

            pages.Remove(usedAsOverview);

            var nodes = pageNode.SubNodes.Where(e =>
            {
                return getSubPages(pubPageRepo, e, isPublic).Count() > 0;
            }).Select(e =>
            {
                var overviewPage = getSubPages(pubPageRepo, e, isPublic).OrderBy(p => p.MenuOrder).FirstOrDefault();
                var overviewId = overviewPage.Id;
                var url = _urlHelper.GetPageUrl(overviewPage);
                var item = new MenuItem { Type = MenuItem.ItemType.Section, OverviewId = overviewId, Id = e.Id, Url = url, MenuOrder = e.MenuOrder, Title = e.NodeName };
                item.SubItems = getAllMenuItems(pubPageRepo, e, isPublic);
                return item;
            }).ToList();
            return pages.Union(nodes).OrderBy(e => e.MenuOrder).ToList();
        }

        private class MenuItem
        {
            internal enum ItemType
            {
                Page,
                Section
            }
            internal int Id { get; set; }
            internal int OverviewId { get; set; }
            internal string Url { get; set; }
            internal string Title { get; set; }
            internal int MenuOrder { get; set; }
            internal ItemType Type { get; set; }
            internal virtual IList<MenuItem> SubItems { get; set; }
        }
        private static IQueryable<IPage> getSubPages(IRepository<PubPage> pubPageRepo, NaviNode node, bool isPublic)
        {
            if (isPublic)
            {
                return pubPageRepo.GetAll().Where(e => e.NaviNode.Id == node.Id);
            }
            else
            {
                return node.Pages.AsQueryable<IPage>();
            }
        }
    }
}
