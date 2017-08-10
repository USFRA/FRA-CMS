using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using RadCms.Entities;
using RadCms.Models;
using System.Globalization;

namespace RadCms.Helpers
{
    public static partial class HtmlHelpers
    {
        //public static MvcHtmlString ActionRightNavi(this HtmlHelper html, NaviNode n, int? pageId, bool isPublic = false)
        //{
        //    if (n != null && n.Parent != null && n.Parent.Parent != null)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        n = CmsPageBase.FindBaseNode(n);
            
        //        var parentId = n.Id;
                
        //        sb.AppendLine("<aside>");

        //        sb.AppendLine("<div class='menu-header-wrapper'>");
        //        sb.AppendLine("<span class='menu-header'>Learn More</span>");
        //        sb.AppendLine("</div>");

        //        using (CmsContext db = new CmsContext())
        //        {
        //            var nodeList = db.NaviNodes.Where(e => e.Parent.Id == parentId);
        //            var isFirst = true;
        //            foreach (var section in nodeList)
        //            {
        //                if (!isFirst)
        //                {
        //                    sb.AppendLine("<div class='section-divider'></div>");
        //                }
        //                else
        //                {
        //                    isFirst = false;
        //                }
        //                sb.AppendLine("<section class='menu-body-wrapper'>");
        //                sb.AppendFormat("<a class='section-header' href='#'>{0}</a>", CultureInfo.InvariantCulture.TextInfo.ToTitleCase(section.NodeName.ToLower()));
        //                var pages = db.Pages.Where(e => e.NaviNode.Id == section.Id);
        //                foreach (var p in pages.Select(e => new { e.Id, e.Url, e.Title }))
        //                {
        //                    if (p.Id == pageId)
        //                    {
        //                        sb.AppendFormat("<a class='selected' href='/{0}' target='_self'>{1}</a>", p.Url, CultureInfo.InvariantCulture.TextInfo.ToTitleCase(p.Title));
        //                    }
        //                    else
        //                    {
        //                        sb.AppendFormat("<a href='/{0}' target='_self'>{1}</a>", p.Url, CultureInfo.InvariantCulture.TextInfo.ToTitleCase(p.Title));
        //                    }
        //                }
        //                sb.AppendLine("</section>");
        //            }
        //        }
        //        sb.AppendLine("</aside>");

        //        return MvcHtmlString.Create(sb.ToString());
        //    }
        //    else{
        //        return MvcHtmlString.Create("");
        //    }

        //}
    }
}
