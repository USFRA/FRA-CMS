using RadCms.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RadCms.Helpers
{
    public class IdBasedUrlHelper: IPageUrlHelper
    {
        public IdBasedUrlHelper()
        {
            System.Diagnostics.Debug.WriteLine("One instance of IdBasedUrlHelper was created.");
        }
        public void UpdatePageUrl(CmsPage p)
        {
            string title = String.IsNullOrEmpty(p.NaviTitle) ? p.Title : p.NaviTitle;
            var urlFormat = GetBaseUrlFromNode(p.NaviNode) + "{0}";
            p.Url = String.Format(urlFormat, String.Join("-", getValidTokens(title)));
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
                return String.Format(urlFormat, String.Join("-", getValidTokens(node.NodeName)));
            }
        }

        private IList<String> getValidTokens(string title)
        {
            return Regex.Split(title, @"[^\w_\.]", RegexOptions.Compiled).Where(e => !String.IsNullOrEmpty(e.Trim())).ToList();
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
            return "Page/" + page.FriendlyId;
        }
    }
}