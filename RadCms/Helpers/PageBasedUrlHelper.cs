using System.Collections.Generic;
using System.Linq;
using RadCms.Entities;
using System.Text.RegularExpressions;

namespace RadCms.Helpers
{
    public class PageBasedUrlHelper: IPageUrlHelper
    {
        public PageBasedUrlHelper()
        {
            System.Diagnostics.Debug.WriteLine("One instance of PageBasedUrlHelper was created.");
        }
        public void UpdatePageUrl(CmsPage p)
        {
            string title = string.IsNullOrEmpty(p.NaviTitle)? p.Title : p.NaviTitle;
            var urlFormat = GetBaseUrlFromNode(p.NaviNode) + "{0}";
            p.Url = string.Format(urlFormat, string.Join("-", getValidTokens(title)));
        }
        public string GetBaseUrlFromNode(NaviNode node)
        {
            if (node == null || node.Parent == null)
            {
                return "";
            }
            else
            {
                var urlFormat = GetBaseUrlFromNode(node.Parent) + "{0}/";
                return string.Format(urlFormat, string.Join("-", getValidTokens(node.NodeName)));
            }
        }

        private IList<string> getValidTokens(string title)
        {
            return Regex.Split(title, @"[^\w_\.]", RegexOptions.Compiled).Where(e=>!string.IsNullOrEmpty(e.Trim())).ToList();
        }

        public void UpdatePageUrl(NaviNode node)
        {
            if (node == null)
            {
                return;
            }
            var pages = node.Pages.ToList();
            foreach (var p in pages)
            {
                UpdatePageUrl(p);
            }

            var nodes = node.SubNodes.ToList();
            foreach (var n in nodes)
            {
                UpdatePageUrl(n);
            }
        }

        public string GetPageUrl(IPage page)
        {
            return page.Url;
        }
    }
}