using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using RadCms.Entities;
using RadCms.Helpers;
using RadCms.Models;

namespace RadCms.Core.Containers.Drivers
{
    public class BreadcrumbWebpartDriver:IWebpartDriver
    {
        private DriverContext _context;
        public string WebpartId
        {
            get { return "BREADCRUMB"; }
        }

        public void Apply(DriverContext context)
        {
            _context = context;
        }

        public DriverResult BuildDisplay()
        {
            return new DriverResult
            {
                Content = BuildContent(_context.Page)
            };
        }

        public DriverResult BuildEditor()
        {
            return BuildDisplay();
        }

        private static string BuildContent(IPage page)
        {
            var n = page.NaviNode;
            var sb = new StringBuilder();
            sb.Append("<div data-replace=\"[$webpart(breadcrumb)$]\" class=\"cms-replaceable breadcrumbWrapper\">");

            List<HrefLink> breadcrumb = new List<HrefLink>();

            if(n != null)// && n.Id != 1)
            {
                NaviNode naviNode = n;

                int loop = 0;
                while(naviNode != null)
                {
                    HrefLink link = new HrefLink();

                    link.Text = HttpUtility.HtmlEncode(naviNode.NodeName);
                    link.Title = HttpUtility.HtmlEncode(naviNode.NodeName);
                    var subpages = naviNode.Pages;
                    if(subpages != null && subpages.Count > 0)
                    {
                        var defaultPage = subpages.OrderBy(e => e.MenuOrder).First();
                        link.Url = "/" + defaultPage.Url;
                    }
                    else
                    {
                        link.Url = "#";
                    }

                    breadcrumb.Insert(0, link);

                    naviNode = naviNode.Parent;

                    loop++;
                    if(loop > 100)
                    {
                        throw new Exception("Invalid Navigation");
                    }
                }
            }

            foreach(HrefLink node in breadcrumb)
            {
                if(node.Url != "#")
                {
                    var anchorBuilder = new TagBuilder("a");

                    anchorBuilder.MergeAttribute("href", node.Url);
                    //anchorBuilder.MergeAttribute("title", node.Title);
                    //anchorBuilder.MergeAttribute("alt", node.Title);
                    anchorBuilder.InnerHtml = node.Text;

                    sb.Append(anchorBuilder.ToString(TagRenderMode.Normal));
                }
                else
                {
                    sb.Append(node.Text);
                }

                sb.Append("<span class='breadcrumb-arrow'></span>");
            }
            if(page == null)
            {
                sb.Append("[Navi Title]");
            }
            else
            {
                sb.Append(page.NaviTitle == null ? page.Title : page.NaviTitle);
            }

            sb.Append("</div>");
            return sb.ToString();
        }
    }
}