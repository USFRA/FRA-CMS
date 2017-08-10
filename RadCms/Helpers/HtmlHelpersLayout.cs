using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RadCms.Entities;
using RadCms.Models;
using System.Text;
using System.Web;
using RadCms.Data;

namespace RadCms.Helpers
{
    public static partial class HtmlHelpers
    {
        public static MvcHtmlString GetHeaderNav(this HtmlHelper html, bool isCms = false)
        {
            var webparts = new Dictionary<string, string>
            {
                { "default", "NavigationBar" },
            };
            string webpartId;
            webparts.TryGetValue(CmsHelper.Site, out webpartId);

            if (webpartId == null)
            {
                webpartId = "NavigationBar";
            }

            string content = string.Empty;
            var headerNavDriver = DependencyResolver.Current.GetServices<IWebpartDriver>().SingleOrDefault(e => e.WebpartId == webpartId);
            
            if(headerNavDriver != null)
            {
                headerNavDriver.Apply(new DriverContext
                {
                    WebpartId = webpartId,
                    ControllerContext = html.ViewContext.Controller.ControllerContext,
                    IsPublic = !CmsHelper.IsCms
                });

                content = headerNavDriver.BuildDisplay().Content;
            }

            return MvcHtmlString.Create(content);
        }

        #region Nav
        
        public static MvcHtmlString GetNav(this HtmlHelper html, NaviNode node, bool withinCms = true)
        {
            string nodeName = "";
            if (node != null)
            {
                int count = 0;
                while (node.Parent != null && node.Parent.Parent != null && count < 100)
                {
                    node = node.Parent;
                    count++;
                }
                nodeName = node.NodeName.ToLower();
            }

            return MvcHtmlString.Create(GetNavWebpart(html.ViewContext.HttpContext, nodeName, withinCms));
        }
        public static MvcHtmlString GetNav(this HtmlHelper html, IPage page, bool withinCms = true)
        {
            string nodeName = "";
            if (page != null)
            {
                var node = page.NaviNode;
                if (node != null)
                {
                    int count = 0;
                    while (node.Parent != null && node.Parent.Parent != null && count < 100)
                    {
                        node = node.Parent;
                        count++;
                    }
                    nodeName = node.NodeName.Substring(0, 5).ToLower();
                }
            }

            return MvcHtmlString.Create(GetNavWebpart(html.ViewContext.HttpContext, nodeName, withinCms));
        }
        public static MvcHtmlString GetNavElib(this HtmlHelper html, bool withinCms = true)
        {
            //FRA<br />eLibrary 
            return MvcHtmlString.Create(GetNavWebpart(html.ViewContext.HttpContext, "fra<b", withinCms));
        }
        private static string GetNavWebpart(HttpContextBase context, string nodeName, bool withinCms = true)
        {
            using (CmsContext db = new CmsContext())
            {
                StringBuilder sb = new StringBuilder();
                if (withinCms)
                {
                    sb.Append("<div id='headerNavWrapper' class='headerNavWebpart webpart' webpartId='headerNav'>");
                }
                else
                {
                    sb.Append("<div id='headerNavWrapper' class='headerNavWebpart webpart'>");
                }

                sb.Append("<div id='headerNavItems'><ul>");
                var items = db.Set<NavGroup>().ToList();
                foreach (var nav in items)
                {
                    string title = nav.Title.ToLower();
                    if (title == nodeName)
                    {
                        sb.Append("<li class='currentSection' id='globalNav");
                    }
                    else
                    {
                        sb.Append("<li id='globalNav");
                    }

                    sb.Append(nav.Index);
                    sb.Append("'>");
                    sb.Append("<a href='");
                    sb.Append(UrlHelper.GenerateContentUrl("~/", context));
                    sb.Append(nav.Link);
                    sb.Append("'><strong>");
                    sb.Append(nav.Title);
                    sb.Append("</strong></a>");
                    sb.Append("</li>");
                }
                sb.Append("</ul></div>");

                sb.Append("<div id='navMenuDetails'>");

                foreach (var nav in items)
                {
                    sb.Append("<div class='navDetailsContent' id='globalNav");
                    sb.Append(nav.Index);
                    sb.Append("Detail'>");
                    if (nav.Items.Count > 0)
                    {
                        sb.Append("<ul>");
                        foreach (var i in nav.Items)
                        {
                            sb.Append("<li>");
                            sb.AppendFormat("<a href='{0}'>{1}</a>", i.Link, i.Text);
                            sb.Append("</li>");
                        }
                        sb.Append("</ul>");
                    }
                    sb.Append("</div>");
                }

                sb.Append("</div>");

                sb.Append("</div>");
                return sb.ToString();
            }
        }

        #endregion

        #region Footer
        
        public static MvcHtmlString GetFooter(this HtmlHelper html)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id='footerWrapper' class='footerWebpart webpart' webpartId='footer' >");
            for (int i = 1; i <= 4; i++)
            {
                sb.Append(getFooter(i));
            }
            sb.Append("<div style='clear:both;'></div>");
            sb.Append("</div>");
            return MvcHtmlString.Create(sb.ToString());
        }
        private static StringBuilder getFooter(int column)
        {
            string columnId;
            switch (column)
            {
                case 1:
                    columnId = "footerA";
                    break;
                case 2:
                    columnId = "footerB";
                    break;
                case 3:
                    columnId = "footerC";
                    break;
                case 4:
                    columnId = "footerD";
                    break;
                default:
                    return new StringBuilder("");
            }
            StringBuilder footer = new StringBuilder();
            footer.Append("<div id='");
            footer.Append(columnId);
            footer.Append("' class='footerCol'>");

            using (CmsContext db = new CmsContext())
            {
                List<FooterSection> sections = db.Set<FooterSection>().Where(e => e.Column == column).OrderBy(e => e.Order).ToList();
                foreach (var s in sections)
                {
                    if (s.Type == FooterSection.SectionType.Vertical)
                    {
                        footer.Append("<ul class='verticalSection'>");
                    }
                    else
                    {
                        footer.Append("<ul class='horizontalSection'>");
                    }
                    footer.Append("<li class='footerTitle'>");
                    footer.Append(s.Title);
                    footer.Append("</li>");
                    List<FooterItem> items = db.Set<FooterItem>().Where(e => e.IsPublished == true && e.Section.Id == s.Id).OrderBy(e => e.Index).ToList();

                    foreach (FooterItem f in items)
                    {
                        footer.Append("<li class='footerItem'>");
                        if (String.IsNullOrEmpty(f.Link))
                        {
                            footer.Append(f.Title);
                        }
                        else
                        {
                            footer.Append("<a href='");
                            footer.Append(f.Link);
                            footer.Append("'  target='");
                            footer.Append(f.Target);
                            footer.Append("'>");
                            footer.Append(f.Title);
                            footer.Append("</a>");

                        }
                        footer.Append("</li>");
                    }

                    footer.Append("</ul>");
                }
            }
            footer.Append("</div>");
            return footer;
        }
        
        #endregion

        public static MvcHtmlString BackgroundStyle(this HtmlHelper html, int status)
        {
            StringBuilder sb = new StringBuilder();

            switch (status)
            {
                case CmsPage.STATUS_CHANGE_SAVED:
                    sb.Append("html { background-color: #000; background-image: url('" + UrlHelper.GenerateContentUrl("~/Core/assetsCms/images/statusBackYellow.png", html.ViewContext.HttpContext) + "'); background-repeat:repeat-x}");

                    break;

                case CmsPage.STATUS_EDITING_START:
                case CmsPage.STATUS_EDITING_AGAIN:
                case CmsPage.STATUS_EDITING_BY_OTHERS:
                    sb.Append("html { background-color: #000; background-image: url('" + UrlHelper.GenerateContentUrl("~/Core/assetsCms/images/statusBackRed.png", html.ViewContext.HttpContext) + "'); background-repeat:repeat-x}");
                    break;
                case CmsPage.STATUS_ARCHIVED:
                    sb.Append("html { background-color: #000; background-image: url('" + UrlHelper.GenerateContentUrl("~/Core/assetsCms/images/statusBackGrey.png", html.ViewContext.HttpContext) + "'); background-repeat:repeat-x}");
                    break;
                case CmsPage.STATUS_NORMAL:
                default:
                    sb.Append("html { background-color: #000; background-image: url('" + UrlHelper.GenerateContentUrl("~/Core/assetsCms/images/statusBackGreen.png", html.ViewContext.HttpContext) + "'); background-repeat:repeat-x}");
                    break;
            }

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}