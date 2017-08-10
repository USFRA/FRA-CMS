using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RadCms.Entities;
using RadCms.Models;
using System.Text;

namespace RadCms.Helpers
{
    public static partial class HtmlHelpers
    {
    
        private static string GeneratePageButtons(UrlHelper url, bool isPublished, int status, int sectionId, string modifiedBy, DateTime lastModified, int accessMode)
        {
            StringBuilder sb = new StringBuilder();

            string alt = "";

            if (modifiedBy != null)
            {
                alt = " - Modified by " + modifiedBy.Substring(6) + " at " + lastModified;

            }

            switch (status)
            {
                case CmsPage.STATUS_CHANGE_SAVED:
                    sb.Append("<img id=\"PageStatusIcon\" src=\"/Core/assetsCms/images/statusYellow.png\" alt=\"Yellow Status" +
                        alt + "\" title=\"Yellow Status" + alt + "\" />");

                    if (accessMode >= 3)
                    {
                        sb.Append("<a href=\"#\" onclick=\"publish();return false\"");
                        sb.Append("><img alt=\"Publish\" src=\"/Core/assetsCms/images/publishButton.png\" /></a>");
                    }

                    if (accessMode >= 2)
                    {
                        sb.Append("<a href=\"#\" onclick=\"edit();return false\"");
                        sb.Append("><img alt=\"Edit\" src=\"/Core/assetsCms/images/editDraftButton.png\" /></a>");
                    }

                    break;

                case CmsPage.STATUS_EDITING_START:
                case CmsPage.STATUS_EDITING_AGAIN:
                    sb.Append("<img id=\"PageStatusIcon\" src=\"/Core/assetsCms/images/statusRed.png\" alt=\"Red Status" +
                        alt + "\" title=\"Red Status" + alt + "\" />");

                    String userName = null;

                    if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.User != null)
                    {
                        userName = System.Web.HttpContext.Current.User.Identity.Name.ToUpper();

                    }

                    if (userName != null && userName == modifiedBy && accessMode >= 2)
                    {
                        sb.Append("<a href=\"#\" onclick=\"edit();return false\"");
                        sb.Append("><img alt=\"Edit\" src=\"/Core/assetsCms/images/editDraftButton.png\" /></a>");
                    }
                    else
                    {
                        if (accessMode >= 4)
                        {
                            sb.Append("<a href=\"#\" onclick=\"unlock();return false\"");
                            sb.Append("><img alt=\"Edit Locked\" src=\"/Core/assetsCms/images/editDisabledButton.png\" /></a>");
                        }
                        else if (accessMode >= 2)
                        {
                            sb.Append("<a href=\"#\" onclick=\"return false\"");
                            sb.Append("><img alt=\"Edit Locked\" src=\"/Core/assetsCms/images/editDisabledButton.png\" /></a>");
                        }
                    }

                    break;

                case CmsPage.STATUS_NORMAL:
                default:
                    sb.Append("<img id=\"PageStatusIcon\" src=\"/Core/assetsCms/images/statusGreen.png\" alt=\"Green Status" +
                        alt + "\" title=\"Green Status" + alt + "\" />");

                    if (accessMode >= 2)
                    {
                        sb.Append("<a href=\"#\" onclick=\"edit();return false\"");
                        sb.Append("><img alt=\"Edit\" src=\"/Core/assetsCms/images/editButton.png\" /></a>");
                    }

                    break;
            }

            return sb.ToString();
        }


        public static MvcHtmlString GeneratePageSubButtons(this HtmlHelper html, int status, int sectionId, int accessMode = 0)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            StringBuilder sb = new StringBuilder();
            if (accessMode > 3)
            {
                sb.Append("<a id=\"createPageButton\" href=\"/CreatePage/").Append(sectionId).Append("\"");
                sb.Append("><img alt=\"Create Page\" src=\"/Core/assetsCms/images/addNewPage.png\" /></a>");

                sb.Append("<a id=\"deletePageButton\" href=\"#\" onclick=\"deletePage();return false\"");
                sb.Append("><img alt=\"Delete Page\" src=\"/Core/assetsCms/images/deleteButton.png\" /></a>");

                sb.Append("<a id=\"movePagesButton\" href=\"");
                sb.Append(url.Action("MovePages", "Section", new { id = sectionId }));
                sb.Append("\"><img alt=\"Move Pages\" src=\"/Core/assetsCms/images/movePagesButton.png\" /></a>");

                sb.Append("<a id=\"renamePagesButton\" href=\"");
                sb.Append(url.Action("Navi", "Section", new { id = sectionId }));
                sb.Append("\"><img alt=\"Rename Pages\" src=\"/Core/assetsCms/images/renameButton.png\" /></a>");
            }

            sb.Append("<a href=\"#\" onclick=\"showHistory();return false\">")
                .Append("<img alt=\"Show History\" src=\"/Core/assetsCms/images/viewPageHistory.png\"></a>");

            sb.Append("<a id=\"analyzeLinksButton\" href=\"#\" onclick=\"analyzeLinks();return false\"");
            sb.Append("><img alt=\"View Relationships\" src=\"/Core/assetsCms/images/viewRelationships.png\" /></a>");

            if (accessMode == 4)
            {
                sb.Append("<a id=\"permission\" href=\"#\" onclick=\"managePermissions();return false\"");
                sb.Append("><img alt=\"Permissions\" src=\"/Core/assetsCms/images/viewAccess.png\" /></a>");
            }

            //sb.Append("<a id=\"movePageButton\" href=\"#\" onclick=\"movePages();return false\"");
            //sb.Append("><img alt=\"Move Pages\" src=\"/Core/assetsCms/images/movePagesButton.png\" /></a>");

            //sb.Append("<a id=\"structureBtn\" href=\"");
            //sb.Append(url.Action("Navi", "Section", new { id = sectionId }));
            //sb.Append("\"><img alt=\"Settings\" src=\"/Core/assetsCms/images/pageSettingsDownButton.png\" /></a>");

            //sb.Append("<a id=\"renamePageButton\" href=\"#\" onclick=\"renamePages();return false\"");
            //sb.Append("><img alt=\"Rename Pages\" src=\"/Core/assetsCms/images/renameButton.png\" /></a>");

            //sb.Append("<a href=\"#\" onclick=\"viewPermission();return false\"");
            //sb.Append("><img alt=\"View Permission\" src=\"/Core/assetsCms/images/viewPermissionsButton.png\" /></a>");

            //<img src="../../assetsCms/images/addNewPage.png" />
            //<img src="../../assetsCms/images/deleteButton.png" />
            //<img src="../../assetsCms/images/movePagesButton.png" />
            //<img src="../../assetsCms/images/viewPermissionsButton.png" />

            return MvcHtmlString.Create(sb.ToString());
        }


        public static MvcHtmlString PageButtons(this HtmlHelper html, bool isPublished, int status,
            int sectionId, int accessMode)
        {
            return MvcHtmlString.Create(GeneratePageButtons(new UrlHelper(html.ViewContext.RequestContext),
                isPublished, status, sectionId, null, DateTime.Now, accessMode
                ));
        }


        public static MvcHtmlString PageButtons(this HtmlHelper html, bool isPublished, int status, int sectionId,
            string modifiedBy, DateTime lastModified, int accessMode)
        {
            return MvcHtmlString.Create(GeneratePageButtons(new UrlHelper(html.ViewContext.RequestContext),
                isPublished, status, sectionId, modifiedBy, lastModified, accessMode
                ));
        }

    }
}