using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using RadCms.Entities;
using RadCms.Models;
using System.Text;
using System.Linq.Expressions;

namespace RadCms.Helpers
{
    public static partial class HtmlHelpers
    {
        static System.Globalization.TextInfo myTI = new System.Globalization.CultureInfo("en-US", false).TextInfo;
        
        public static MvcHtmlString FormatName(this HtmlHelper html, string name)
        {
            if (name == null)
            {
                return MvcHtmlString.Create("");
            }

            string[] n = name.Split('\\');

            if (n.Length > 0)
            {
                return MvcHtmlString.Create(myTI.ToTitleCase(name.Substring(6).Replace('.', ' ')));
            }
            else
            {
                return MvcHtmlString.Create(myTI.ToTitleCase(name));
            }
        }

        public static MvcHtmlString AccessMode(this HtmlHelper html, int accessMode)
        {
            string name = "";
            switch (accessMode)
            {
                case 0:
                    name = "Read";
                    break;
                case 1:
                    name = "Webpart";
                    break;
                case 2:
                    name = "Edit";
                    break;
                case 3:
                    name = "Publish";
                    break;
                case 4:
                    name = "Create";
                    break;
            }

            return MvcHtmlString.Create(name);
        }

        public static MvcHtmlString FormatAddress(this HtmlHelper html, string address1, string address2, string city, string state, string zip, string c)
        {
            if(String.IsNullOrEmpty(address1))
            {
                return MvcHtmlString.Create("");
            }
            return MvcHtmlString.Create(String.IsNullOrEmpty(c) ? "<p>" : "<p class='" + c + "'>" + address1 + "<br />" + (String.IsNullOrEmpty(address2) ? "" : (address2 + "<br />")) + city + ", " + state + " " + zip + "</p>");
        }

        public static MvcHtmlString FormatPOC(this HtmlHelper html, string name, string email, string emailSubject, string phone, string c)
        {
            if (String.IsNullOrEmpty(name))
            {
                return MvcHtmlString.Create("");
            }
            return MvcHtmlString.Create(String.IsNullOrEmpty(c) ? "<p>" : "<p class='" + c + "'>" + name + "<br />" + (String.IsNullOrEmpty(phone) ? "" : (phone + "<br />")) + (String.IsNullOrEmpty(email) ? "</p>" : "<a href='mailto:" + email + "?subject=" + emailSubject + "' target='_blank'>" + email + "</a></p>"));
        }

        public static MvcHtmlString ActionImage(this HtmlHelper html, string action,
            string controller, object routeValues, string imagePath, string alt, string buttonId)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            // build the <img> tag
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", url.Content(imagePath));
            imgBuilder.MergeAttribute("alt", alt);
            string imgHtml = imgBuilder.ToString(TagRenderMode.SelfClosing);

            // build the <a> tag
            var anchorBuilder = new TagBuilder("a");
            anchorBuilder.MergeAttribute("href", url.Action(action, controller, routeValues));
            if (buttonId != null)
            {
                anchorBuilder.MergeAttribute("id", buttonId);
            }

            anchorBuilder.InnerHtml = imgHtml; // include the <img> tag inside

            string anchorHtml = anchorBuilder.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(anchorHtml);
        }

        public static MvcHtmlString ActionBreadcrumb(this HtmlHelper html, IEnumerable<HrefLink> breadcrumb)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            StringBuilder sb = new StringBuilder();

            foreach (HrefLink node in breadcrumb)
            {
                var anchorBuilder = new TagBuilder("a");

                anchorBuilder.MergeAttribute("href", node.Url);
                anchorBuilder.MergeAttribute("title", node.Title);
                anchorBuilder.MergeAttribute("alt", node.Title);
                anchorBuilder.InnerHtml = System.Web.HttpUtility.HtmlEncode(node.Text);

                sb.Append(anchorBuilder.ToString(TagRenderMode.Normal));

                sb.Append("//");
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        /*
        public static MvcHtmlString ActionLeftNavi(this HtmlHelper html, IEnumerable<HrefLink> links)
        {

            var url = new UrlHelper(html.ViewContext.RequestContext);

            StringBuilder sb = new StringBuilder();

            sb.Append("<ul>");

            foreach (HrefLink node in links)
            {
                sb.Append("<li>");

                var anchorBuilder = new TagBuilder("a");

                anchorBuilder.MergeAttribute("href", node.Url);
                anchorBuilder.MergeAttribute("title", node.Title);
                anchorBuilder.MergeAttribute("alt", node.Title);
                anchorBuilder.InnerHtml = node.Text;

                sb.Append(anchorBuilder.ToString(TagRenderMode.Normal));
                sb.Append("</li>");
            }

            sb.Append("</ul>");

            return MvcHtmlString.Create(sb.ToString());
        }
        */

        public static MvcHtmlString ActionBreadcrumbCms(this HtmlHelper html, NaviNode naviNode)
        {

            return ActionBreadcrumbCms(html, naviNode, "Navi", "Section");

        }

        public static MvcHtmlString ActionBreadcrumbCms(this HtmlHelper html, NaviNode naviNode, string action, string controller)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            StringBuilder sb = new StringBuilder();

            List<HrefLink> breadcrumb = new List<HrefLink>();

            NaviNode currentNode = naviNode.Parent;
            int loop = 0;
            while (currentNode != null)
            {
                HrefLink link = new HrefLink();

                link.Text = System.Web.HttpUtility.HtmlEncode(currentNode.NodeName);
                link.Title = System.Web.HttpUtility.HtmlEncode(currentNode.NodeName);

                if (action == null)
                {
                    link.Url = null;
                }
                else
                {
                    link.Url = url.Action(action, controller, new { id = currentNode.Id });
                }

                breadcrumb.Insert(0, link);

                currentNode = currentNode.Parent;

                loop++;
                if (loop > 100)
                {
                    throw new Exception("Invalid Navigation");
                }
            }


            foreach (HrefLink node in breadcrumb)
            {
                if (node.Url == null)
                {
                    sb.Append(node.Text);
                }
                else
                {
                    var anchorBuilder = new TagBuilder("a");

                    anchorBuilder.MergeAttribute("href", node.Url);
                    anchorBuilder.MergeAttribute("title", node.Title);
                    anchorBuilder.MergeAttribute("alt", node.Title);
                    anchorBuilder.InnerHtml = node.Text;

                    sb.Append(anchorBuilder.ToString(TagRenderMode.Normal));
                }

                sb.Append(" // ");
            }

            sb.Append(naviNode.NodeName);


            /*
       sb.Append(@"<div id=""titleContainer"">
            <div id=""title"">
                <div id=""metaTitle"">
                    <div style=""position: relative"">
                        <div id=""C-Title"">");

            sb.Append(naviNode.NodeName);
                            
                                 sb.Append(@"</div>
                        <img width=""13"" height=""11"" alt=""more"" src=""Styles/details/dropArrow.png"" id=""titleDropdown""
                            style=""display: inline;"">
                    </div>
                </div>
                <div id=""moreTitles"" style=""display: none;"">";

                     sb.Append("<div>");
            sb.Append(naviNode.NodeName);
            sb.Append("</div>");
  
                   
                 sb.Append(@"</div>
            </div>
        </div>");
            */
            return MvcHtmlString.Create(sb.ToString());
        }

        private static string linkForNode(NaviNode naviNode, UrlHelper urlHelper, bool isPublic = false)
        {
            string href;

            if (naviNode.DefaultPageId.HasValue)
            {
                href = urlHelper.Action("Page", "Cms", new { id = CmsPage.ToFriendlyId(naviNode.DefaultPageId.Value) });
            }
            else
            {
                CmsPage p = null;
                if (isPublic)
                {
                    p = naviNode.Pages.Where(e => e.IsPublished).OrderBy(pg => pg.MenuOrder).ThenBy(pg => pg.Id).FirstOrDefault();
                }
                else
                {
                    p = naviNode.Pages.OrderBy(pg => pg.MenuOrder).ThenBy(pg => pg.Id).FirstOrDefault();
                }

                if (p != null)
                {
                    //href = urlHelper.Action("Page", "Cms", new { id = p.FriendlyId });
                    href = "/" + p.Url.ToLower();
                }
                else
                {
                    href = "#";
                }

            }

            return href;
        }

        public static MvcHtmlString ActionBreadcrumb(this HtmlHelper html, IPage page)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            StringBuilder sb = new StringBuilder();

            List<HrefLink> breadcrumb = new List<HrefLink>();

            if (page.NaviNode != null && page.NaviNode.Id != 1)
            {
                NaviNode naviNode = page.NaviNode;

                int loop = 0;
                while (naviNode != null)
                {
                    HrefLink link = new HrefLink();

                    link.Text = System.Web.HttpUtility.HtmlEncode(naviNode.NodeName);
                    link.Title = System.Web.HttpUtility.HtmlEncode(naviNode.NodeName);

                    link.Url = linkForNode(naviNode, url);


                    breadcrumb.Insert(0, link);

                    naviNode = naviNode.Parent;

                    loop++;
                    if (loop > 100)
                    {
                        throw new Exception("Invalid Navigation");
                    }
                }
            }


            foreach (HrefLink node in breadcrumb)
            {
                if (node.Url != "#")
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

                sb.Append(" // ");
            }

            sb.Append(page.NaviTitle == null ? page.Title : page.NaviTitle);

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString PageLinkWithBreadcrum(this HtmlHelper html, IPage page)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            StringBuilder sb = new StringBuilder();

            List<HrefLink> breadcrumb = new List<HrefLink>();

            if (page.NaviNode != null && page.NaviNode.Id != 1)
            {
                NaviNode naviNode = page.NaviNode;

                int loop = 0;

                while (naviNode != null && naviNode.Id != 1)
                {
                    HrefLink link = new HrefLink();

                    link.Text = System.Web.HttpUtility.HtmlEncode(naviNode.NodeName);
                    link.Title = System.Web.HttpUtility.HtmlEncode(naviNode.NodeName);

                    link.Url = linkForNode(naviNode, url);


                    breadcrumb.Insert(0, link);

                    naviNode = naviNode.Parent;

                    loop++;
                    if (loop > 100)
                    {
                        throw new Exception("Invalid Navigation");
                    }
                }
            }


            foreach (HrefLink node in breadcrumb)
            {
                if (node.Url != "#")
                {
                    sb.Append(node.Text);
                }
                else
                {
                    sb.Append(node.Text);
                }

                sb.Append(" // ");
            }

            return MvcHtmlString.Create(sb.ToString());
        }


        public static MvcHtmlString PageListIcon(this HtmlHelper html, int status)
        {
            StringBuilder sb = new StringBuilder();
            switch (status)
            {
                case CmsPage.STATUS_NORMAL:
                    sb.Append("<img src='/assetsCms/images/statusGreenList.png' alt='Status Green' />");
                    break;

                case CmsPage.STATUS_CHANGE_SAVED:
                    sb.Append("<img src='/assetsCms/images/statusYellowList.png' alt='Status Yellow' />");
                    break;

                default:
                    sb.Append("<img src='/assetsCms/images/statusRedList.png' alt='Status Red' />");
                    break;
            }

            return MvcHtmlString.Create(sb.ToString());
        }


        private static IEnumerable<TreeModel> GetChildren(NaviNode node, bool isPublic = false)
        {

            var folders = (from entity in node.SubNodes
                           select new TreeModel
                           {
                               Id = entity.Id,
                               MenuOrder = entity.MenuOrder,
                               hasChildren = true,
                               Item = entity,
                           }).ToList();

            if (isPublic)
            {
                var pages = (from entity in node.Pages
                             where entity.IsPublished
                             select new TreeModel
                             {
                                 Id = entity.Id,
                                 MenuOrder = entity.MenuOrder,
                                 hasChildren = false,
                                 Item = entity,
                             }).ToList();

                return (folders.Union(pages)).OrderBy(a => a.MenuOrder).ThenBy(a => a.Id);
            }
            else
            {
                var pages = (from entity in node.Pages
                             select new TreeModel
                             {
                                 Id = entity.Id,
                                 MenuOrder = entity.MenuOrder,
                                 hasChildren = false,
                                 Item = entity,
                             }).ToList();

                return (folders.Union(pages)).OrderBy(a => a.MenuOrder).ThenBy(a => a.Id);
            }
        }

        public static MvcHtmlString DisplayChildren(this HtmlHelper html, NaviNode currentNode)
        {
            StringBuilder sb = new StringBuilder();

            foreach (TreeModel m in GetChildren(currentNode))
            {
                sb.Append("<li>");

                if (m.hasChildren)
                {
                    NaviNode subNode = m.Item as NaviNode;

                    sb.Append(subNode.NodeName);
                }
                else
                {
                    CmsPage subPage = m.Item as CmsPage;

                    sb.Append(subPage.Title);
                }

                sb.Append("</li>");
            }

            return MvcHtmlString.Create(sb.ToString());
        }


        public static MvcHtmlString DisplayFolders(this HtmlHelper html, NaviNode currentNode)
        {
            StringBuilder sb = new StringBuilder();

            var folders = (from entity in currentNode.SubNodes
                           select new TreeModel
                           {
                               Id = entity.Id,
                               MenuOrder = entity.MenuOrder,
                               hasChildren = true,
                               Item = entity,
                           }).ToList().OrderBy(a => a.MenuOrder).ThenBy(a => a.Id);



            foreach (TreeModel m in folders)
            {
                NaviNode subNode = m.Item as NaviNode;
                sb.Append("<li value=\"" + subNode.Id + "\">");
                sb.Append(subNode.NodeName);
                sb.Append("</li>");
            }

            return MvcHtmlString.Create(sb.ToString());
        }


        public static MvcHtmlString DisplayParents(this HtmlHelper html, NaviNode currentNode)
        {
            StringBuilder sb = new StringBuilder();

            if (currentNode.Id != 1)
            {
                NaviNode parentNode = currentNode.Parent;

                int loop = 0;
                while (parentNode.Id != 1)
                {
                    StringBuilder ns = new StringBuilder();
                    ns.Append("<div class=\"parentNode\">");
                    ns.Append(System.Web.HttpUtility.HtmlEncode(parentNode.NodeName));
                    ns.Append("</div>");

                    sb.Insert(0, ns.ToString());
                    parentNode = parentNode.Parent;

                    loop++;
                    if (loop > 100)
                    {
                        throw new Exception("Invalid Navigation");
                    }
                }

                sb.Append("<div>");
                sb.Append(currentNode.NodeName);
                sb.Append("</div>");
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString ActionLeftNavi(this HtmlHelper html, NaviNode n, int? pageId, bool isPublic = false)
        {

            var url = new UrlHelper(html.ViewContext.RequestContext);

            StringBuilder sb = new StringBuilder();

            NaviNode currentNode = n;

            //no side menu for home page
            if (n.Id != 1)
            {
                NaviNode parentNode = currentNode.Parent;

                int loop = 0;
                while (parentNode.Id != 1)
                {
                    StringBuilder ns = new StringBuilder();
                    ns.Append("<div class=\"parentNode\">");
                    var anchorBuilder = new TagBuilder("a");
                    anchorBuilder.MergeAttribute("href", linkForNode(parentNode, url, isPublic));
                    anchorBuilder.InnerHtml = System.Web.HttpUtility.HtmlEncode(parentNode.NodeName);
                    ns.Append(anchorBuilder.ToString(TagRenderMode.Normal));
                    ns.Append("</div>");

                    sb.Insert(0, ns.ToString());
                    parentNode = parentNode.Parent;

                    loop++;
                    if (loop > 100)
                    {
                        throw new Exception("Invalid Navigation");
                    }
                }

                sb.Append("<div class=\"menuBlock\"><span class='leftNavTitle'>");
                sb.Append( System.Web.HttpUtility.HtmlEncode(currentNode.NodeName));
                sb.Append("</span><ul>");


                foreach (TreeModel m in GetChildren(currentNode, isPublic))
                {
                    if (m.hasChildren)
                    {
                        NaviNode subNode = m.Item as NaviNode;

                        StringBuilder subNodeSb = new StringBuilder();
                        //bool atSubSection = false;

                        subNodeSb.Append("<li class='section'><span>");
                        var anchorBuilder = new TagBuilder("a");

                        anchorBuilder.MergeAttribute("href", linkForNode(subNode, url, isPublic));
                        //anchorBuilder.MergeAttribute("href", "/" + subNode.Pages.SingleOrDefault(e=>e.MenuOrder == 0).Url);
                        anchorBuilder.InnerHtml = System.Web.HttpUtility.HtmlEncode(subNode.NodeName);
                        subNodeSb.Append(anchorBuilder.ToString(TagRenderMode.Normal));
                        subNodeSb.Append("</span></li>");

                        sb.Append(subNodeSb.ToString());
                    }
                    else
                    {
                        #region subPage

                        CmsPage subPage = m.Item as CmsPage;

                        if (subPage.Id == pageId)
                        {
                            sb.Append("<li class='selected page'><span class='selected'>");
                        }
                        else
                        {
                            sb.Append("<li class='page'><span>");
                        }

                        var anchorBuilder = new TagBuilder("a");

                        //anchorBuilder.MergeAttribute("href", url.Action("Page", "Cms", new { id = subPage.FriendlyId }));
                        anchorBuilder.MergeAttribute("href", "/" + subPage.Url.ToLower());

                        anchorBuilder.InnerHtml = subPage.NaviTitle != null ? subPage.NaviTitle : subPage.Title;

                        sb.Append(anchorBuilder.ToString(TagRenderMode.Normal));

                        sb.Append("</span></li>");
                        #endregion
                    }
                }

                sb.Append("</ul></div>");
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString ActionRightNavi(this HtmlHelper html, NaviNode naviNode)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            StringBuilder sb = new StringBuilder();

            if (naviNode != null)
            {
                foreach (NaviHeading heading in naviNode.NaviHeadings)
                {
                    sb.Append("<span class='rightNavTitle'>");
                    sb.Append(heading.Description);
                    sb.Append("</span><ul>");
                    foreach (NaviLink link in heading.NaviLinks)
                    {
                        sb.Append("<li><span>");

                        var anchorBuilder = new TagBuilder("a");

                        anchorBuilder.MergeAttribute("href", link.Url);
                        anchorBuilder.MergeAttribute("title", link.Description);
                        anchorBuilder.MergeAttribute("alt", link.Description);
                        anchorBuilder.InnerHtml = link.Description;

                        sb.Append(anchorBuilder.ToString(TagRenderMode.Normal));

                        sb.Append("</span></li>");
                    }

                    sb.Append("</ul>");
                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }


        public static MvcHtmlString FormtComment(this HtmlHelper html, string comment)
        {
            if (comment != null)
            {
                return MvcHtmlString.Create(comment.Replace(Environment.NewLine, "<br />"));
            }
            else
            {
                return MvcHtmlString.Create("");
            }

        }

        //public static string GenerateCaptcha(this HtmlHelper helper) 
        //{
        //    return Recaptcha.RecaptchaControlMvc.GenerateCaptcha(helper, "recaptcha", "clean"); 
        //}   
        
        //public static string GenerateCaptcha(this HtmlHelper helper, string id, string theme) 
        //{
        //    if (string.IsNullOrEmpty(Recaptcha.RecaptchaControlMvc.PublicKey) || string.IsNullOrEmpty(Recaptcha.RecaptchaControlMvc.PrivateKey))   
        //        throw new ApplicationException("reCAPTCHA needs to be configured with a public & private key.");
        //    Recaptcha.RecaptchaControl recaptchaControl1 = new Recaptcha.RecaptchaControl(); 
        //    recaptchaControl1.ID = id; 
        //    recaptchaControl1.Theme = theme;
        //    recaptchaControl1.PublicKey = Recaptcha.RecaptchaControlMvc.PublicKey;
        //    recaptchaControl1.PrivateKey = Recaptcha.RecaptchaControlMvc.PrivateKey;
        //    Recaptcha.RecaptchaControl recaptchaControl2 = recaptchaControl1; 
        //    System.Web.UI.HtmlTextWriter writer = new System.Web.UI.HtmlTextWriter((TextWriter)new StringWriter()); 
        //    recaptchaControl2.RenderControl(writer); 
        //    return writer.InnerWriter.ToString(); 
        //} 


        public static string GetSizeReadable(ulong i)
        {
            string sign = (i < 0 ? "-" : "");

            double readable = i;

            string suffix;
            if (i >= 0x1000000000000000) // Exabyte
            {
                suffix = "EB";
                readable = (double)(i >> 50);
            }
            else if (i >= 0x4000000000000) // Petabyte
            {
                suffix = "PB";
                readable = (double)(i >> 40);
            }
            else if (i >= 0x10000000000) // Terabyte
            {
                suffix = "TB";
                readable = (double)(i >> 30);
            }
            else if (i >= 0x40000000) // Gigabyte
            {
                suffix = "GB";
                readable = (double)(i >> 20);
            }
            else if (i >= 0x100000) // Megabyte
            {
                suffix = "MB";
                readable = (double)(i >> 10);
            }
            else if (i >= 0x400) // Kilobyte
            {
                suffix = "KB";
                readable = (double)i;
            }
            else
            {
                return i.ToString(sign + "0 B"); // Byte
            }
            readable = readable / 1024;

            return sign + readable.ToString("0.# ") + suffix;
        }

        public static string GetDocumentType(string mimeType)
        {
            //if (mimeType == null)
            //{
            //    throw new ArgumentNullException("mimeType");
            //}
            switch (mimeType)
            {
                case "application/pdf":
                    return "Pdf";

                case "application/x-zip-compressed":
                    return "Zip";

                case "application/vnd.ms-powerpoint":
                case "application/vnd.openxmlformats-officedocument.presentationml.presentation":
                case "application/vnd.openxmlformats-officedocument.presentationml.template":
                case "application/vnd.openxmlformats-officedocument.presentationml.slideshow":
                case "application/vnd.ms-powerpoint.addin.macroEnabled.12":
                case "application/vnd.ms-powerpoint.presentation.macroEnabled.12":
                case "application/vnd.ms-powerpoint.slideshow.macroEnabled.12":
                    return "PowerPoint";

                case "application/msword":
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.template":
                case "application/vnd.ms-word.document.macroEnabled.12":
                case "application/vnd.ms-word.template.macroEnabled.12":
                    return "Word";

                case "application/vnd.ms-excel":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.template":
                case "application/vnd.ms-excel.sheet.macroEnabled.12":
                case "application/vnd.ms-excel.template.macroEnabled.12":
                case "application/vnd.ms-excel.addin.macroEnabled.12":
                case "application/vnd.ms-excel.sheet.binary.macroEnabled.12":
                    return "Excel";

                default:
                    return "File";
            }
        }

        public static HtmlString EnumDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> modelExpression, string firstElement)
        {
            var typeOfProperty = modelExpression.ReturnType;
            if (!typeOfProperty.IsEnum)
                throw new ArgumentException(string.Format("Type {0} is not an enum", typeOfProperty));
            var enumValues = new SelectList(Enum.GetValues(typeOfProperty));
            return helper.DropDownListFor(modelExpression, enumValues, firstElement);
        }

        public static MvcHtmlString If(this MvcHtmlString value, bool evaluation)
        {
            return evaluation ? value : MvcHtmlString.Empty;
        }
    }
}