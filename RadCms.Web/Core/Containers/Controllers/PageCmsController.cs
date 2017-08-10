using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Text;
using RadCms.Security;

namespace RadCms.Core.Containers.Controllers
{
    using Entities;
    using Models;
    using Helpers;
    using System.Web.Script.Serialization;
    using Mvc;
    using Data;

    public class PageCmsController : CmsControllerBase
    {
        private IPageUrlHelper _urlHelper;
        private IPageEngine _pageEngine;
        private IDbContext _db;

        public PageCmsController(IPageUrlHelper helper, IDbContext db, IPageEngine pageEngine)
        {
            _db = db;
            _pageEngine = pageEngine;
            _urlHelper = helper;
        }

        public ViewResult Manage(int id, int tab = 1)
        {
            var page = _db.Set<CmsPage>().SingleOrDefault(e => e.Id == id);
            if (page != null)
            {
                ViewBag.PageId = id;
                ViewBag.SectionId = page.NaviNode.Id;
            }
            else
            {
                ViewBag.PageId = 1;
                ViewBag.SectionId = 1;
                // TODO hard coded default page for manage page
                page = _db.Set<CmsPage>().Find(1);
            }
            ViewBag.Tab = tab;
            // TODO hard coded page type check
            ViewBag.Page = page;
            ViewBag.SectionType = page.NaviNode.Type.Title;
            ViewBag.IsUser = SecurityHelper.CurrentCmsUser(_db) != null;
            ViewBag.PagePermission = SecurityHelper.PageAccessMode(_db, page);
            return View();
        }

        public ViewResult Index()
        {
            return View(_db.Set<CmsPage>().ToList());
        }

        public ViewResult Details(int id)
        {
            CmsPage page = _db.Set<CmsPage>().Find(id);
            return View(page);
        }

        public ActionResult Version(int id)
        {
            VerPage vPage = _db.Set<VerPage>().SingleOrDefault(e => e.VerId == id);

            if (vPage == null)
            {
                return new HttpNotFoundResult("Page version not found");
            }

            CmsPage page = new CmsPage();

            CopyProperties(vPage, page);

            page.Id = vPage.Id;
            page.NaviNode = _db.Set<NaviNode>().SingleOrDefault(
                e => e.Id == vPage.NaviNodeId);

            StringBuilder webpartHeaders = new StringBuilder();

            bool havingWebPart = false;
            page.ContentHtml.Content = _pageEngine.ReplaceTokens(
                page: page,
                webpartHeaders: webpartHeaders,
                havingWebPart: out havingWebPart,
                controllerContext: this.ControllerContext);

            PermissionType accessMode = SecurityHelper.PageAccessMode(_db, page);
            ViewBag.AccessMode = accessMode;
            ViewBag.VerId = vPage.VerId;
            ViewBag.BaseNode = CmsPageBase.FindBaseNode(page);

            return View(page);
        }

        public ActionResult History(int id)
        {
            ViewData["PageId"] = id;
            return View();
        }
        
        public JsonResult GetHistories(int id)
        {
            var histories =
                (from v in _db.Set<VerPage>()
                 where v.Id == id
                 orderby v.VerId descending
                 select v).Select(e => new { e.IsPublished, e.Title, e.PublishedBy, e.Published, e.VerId }).ToList();

            return Json(new
            {
                histories = histories,
                total = histories.Count
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AnalyzeLinks(int id)
        {
            string friendlyId = CmsPage.ToFriendlyId(id);
            string idPattern = "/" + friendlyId;
            var thisPage = _db.Set<CmsPage>().SingleOrDefault(e => e.Id == id);
            string namePattern = "/" + thisPage.Url;

            List<CmsPage> pages = new List<CmsPage>();

            foreach (var p in _db.Set<CmsPage>().ToList())
            {
                if ((p.ContentHtml.Content != null && p.ContentHtml.Content.IndexOf(idPattern, StringComparison.OrdinalIgnoreCase) > 0)
                    || 
                    (p.ContentHtml.Sidebar != null && p.ContentHtml.Sidebar.IndexOf(idPattern, StringComparison.OrdinalIgnoreCase) > 0) 
                    || 
                    (p.ContentHtml.Content != null && p.ContentHtml.Content.IndexOf(namePattern, StringComparison.OrdinalIgnoreCase) > 0))
                {
                    pages.Add(p);
                }
            }
            ViewBag.ThisPage = thisPage;
            return View(pages);
        }

        public ActionResult Restore(int id)
        {
            VerPage verPage = _db.Set<VerPage>().SingleOrDefault(e => e.VerId == id);

            if (verPage == null)
            {
                throw new Exception("Version is not found.");
            }

            CmsPage cmsPage = _db.Set<CmsPage>().SingleOrDefault(e => e.Id == verPage.Id);

            if (cmsPage != null)
            {
                CopyProperties(verPage, cmsPage);

                PubPage pubpage = _db.Set<PubPage>().SingleOrDefault(e => e.Id == verPage.Id);

                if (pubpage != null && pubpage.Modified == verPage.Modified)
                    cmsPage.Status = CmsPage.STATUS_NORMAL;
                else
                    cmsPage.Status = CmsPage.STATUS_CHANGE_SAVED;

                ((DbContext)_db).Entry(cmsPage).State = EntityState.Modified;
                _db.SaveChanges();
            }

            return Redirect(PageHelper.Goto(verPage.Id, _db));
        }

        [HttpPost]
        public ActionResult Do(string id, string act)
        {
            int internalId = CmsPage.FromFriendlyId(id);
            CmsPage page = _db.Set<CmsPage>().Single(e => e.Id == internalId);

            var accessMode = SecurityHelper.PageAccessMode(_db, page);

            bool isModified = false;

            switch (act)
            {
                #region delete
                case "delete":
                    if (accessMode < PermissionType.Publish)
                    {
                        throw new Exception("Access Denided.");
                    }

                    NaviNode naviNode = page.NaviNode;

                    int parentId = naviNode.Id;

                    int numOfPages = _db.Set<CmsPage>().Where(x => x.NaviNode.Id == parentId).Count();

                    int numOfFolder =
                        _db.Set<NaviNode>().Where(x => x.Parent.Id == parentId).Count();
                    
                    if (numOfPages == 1 && numOfFolder == 0)
                    {
                        //delete both page and folder
                        var pubPages = _db.Set<PubPage>().Where(e=>e.NaviNode.Id == naviNode.Id).ToList();
                        var pages = _db.Set<CmsPage>().Where(e => e.NaviNode.Id == naviNode.Id).ToList();
                        ((DbSet<PubPage>)_db.Set<PubPage>()).RemoveRange(pubPages);
                        ((DbSet<CmsPage>)_db.Set<CmsPage>()).RemoveRange(pages);
                        _db.SaveChanges();
                        
                        NaviNode toBeDeleted = naviNode;
                        var naviParent = naviNode.Parent;
                        _db.Set<NaviNode>().Remove(toBeDeleted);
                        _db.SaveChanges();
                        naviNode = naviParent;
                    }
                    else
                    {
                        PubPage pubPage = _db.Set<PubPage>().SingleOrDefault(p => p.Id == page.Id);
                        if (pubPage != null)
                        {
                            _db.Set<PubPage>().Remove(pubPage);
                        }
                        _db.Set<CmsPage>().Remove(page);
                        _db.SaveChanges();
                    }

                    CmsPage defaultPage = naviNode.Pages.OrderBy(pg => pg.MenuOrder).ThenBy(pg => pg.Id).FirstOrDefault();
                    while (defaultPage == null)
                    {
                        if (naviNode.Id == 1 || naviNode.Parent == null)
                        {
                            throw new Exception("Navigation Error");
                        }

                        naviNode = naviNode.Parent;

                        defaultPage = naviNode.Pages.OrderBy(pg => pg.MenuOrder).ThenBy(pg => pg.Id).FirstOrDefault();
                    }

                    return RedirectToAction("Page", "Cms", new { id = defaultPage.FriendlyId });
                #endregion
                #region unlock
                case "unlock":
                    if (accessMode < PermissionType.Edit)
                    {
                        throw new Exception("Access Denided.");
                    }
                    if (page.Status == CmsPage.STATUS_EDITING_START)
                    {
                        page.Status = CmsPage.STATUS_NORMAL;
                        isModified = true;
                    }
                    else if (page.Status == CmsPage.STATUS_EDITING_AGAIN)
                    {
                        page.Status = CmsPage.STATUS_CHANGE_SAVED;
                        isModified = true;
                    }

                    //db.Entry(page).State = EntityState.Modified;
                    //db.SaveChanges();

                    //return RedirectToAction("Edit", new { id = id });

                    break;
                #endregion
                #region publish
                case "publish":
                    if (accessMode < PermissionType.Publish)
                    {
                        throw new Exception("Access Denided.");
                    }
                    if (page.Status == CmsPage.STATUS_NORMAL
                        || page.Status == CmsPage.STATUS_CHANGE_SAVED)
                    {
                        page.IsPublished = true;
                        page.Status = CmsPage.STATUS_NORMAL;
                        PublishDraft(page);
                        SaveVersion(page);
                        isModified = true;
                    }

                    break;
                #endregion
                #region unpublish
                case "unpublish":
                    if (accessMode < PermissionType.Publish)
                    {
                        throw new Exception("Access Denided.");
                    }
                    // Remove the published page from public site if it exists.
                    PubPage pageToUnpublish = _db.Set<PubPage>().SingleOrDefault(p => p.Id == page.Id);
                    if (pageToUnpublish != null)
                    {
                        _db.Set<PubPage>().Remove(pageToUnpublish);
                        _db.SaveChanges();
                    }

                    // Label most recent live version in history to unpublished.
                    VerPage currentLiveVer = _db.Set<VerPage>().SingleOrDefault(e => e.Id == page.Id && e.IsPublished);
                    if (currentLiveVer != null)
                    {
                        currentLiveVer.IsPublished = false;
                        currentLiveVer.Status = CmsPage.STATUS_UNPUBLISHED;
                        _db.SaveChanges();
                    }

                    // Change CmsPage status after remove PubPage and update VerPage
                    page.IsPublished = false;
                    page.Status = CmsPage.STATUS_CHANGE_SAVED;
                    isModified = true;

                    break;
                #endregion
                #region edit
                case "edit":
                    bool allowEdit = false;
                    
                    if (page.Status == CmsPage.STATUS_NORMAL)
                    {
                        page.Status = CmsPage.STATUS_EDITING_START;
                        allowEdit = true;
                    }
                    else if (page.Status == CmsPage.STATUS_CHANGE_SAVED)
                    {
                        page.Status = CmsPage.STATUS_EDITING_AGAIN;
                        allowEdit = true;
                    }
                    else if (page.Status != CmsPage.STATUS_EDITING_START &&
                        page.Status != CmsPage.STATUS_EDITING_AGAIN)
                    {
                        page.Status = CmsPage.STATUS_EDITING_AGAIN;
                        allowEdit = true;
                    }

                    if (User.Identity.Name.ToUpper() == page.ModifiedBy &&
                        accessMode >= PermissionType.Edit)
                    {
                        allowEdit = true;
                    }

                    if (allowEdit)
                    {
                        page.Modified = DateTime.Now;
                        page.ModifiedBy = User.Identity.Name.ToUpper();
                        ((DbContext)_db).Entry(page).State = EntityState.Modified;
                        _db.SaveChanges();

                        return RedirectToAction("Edit", new { id = id });
                    }

                    break;
                #endregion
                #region cancel
                case "cancel":
                    // TODO Restore previous status
                    if (page.Status == CmsPage.STATUS_EDITING_START)
                    {
                        page.Status = CmsPage.STATUS_NORMAL;
                        isModified = true;
                    }
                    else if (page.Status == CmsPage.STATUS_EDITING_AGAIN)
                    {
                        page.Status = CmsPage.STATUS_CHANGE_SAVED;
                        isModified = true;
                    }

                    break;
                #endregion
                #region restore
                case "restore":
                    if (accessMode < PermissionType.Edit)
                    {
                        throw new Exception("Access Denided.");
                    }
                    RestoreLastVersion(page);
                    isModified = true;
                    break;
                #endregion
            }

            if (isModified)
            {
                ((DbContext)_db).Entry(page).State = EntityState.Modified;
                _db.SaveChanges();
            }

            return Redirect("/" + page.Url);
            //return RedirectToAction("Page", "Cms", new { id = page.FriendlyId });
        }

        #region create
        //
        // GET: /Page/Create
        //id is the navigation id
        public ActionResult Create(int id, int layoutId = 1)
        {
            NaviNode naviNode = _db.Set<NaviNode>().Single(e => e.Id == id);

            PermissionType accessMode = SecurityHelper.NaviAccessMode(_db, naviNode);

            if (accessMode < PermissionType.Create)
            {
                throw new Exception("Access Denided.");
            }

            ViewBag.NaviNode = naviNode;
            ViewBag.AccessMode = accessMode;
            ViewBag.BaseNode = CmsPageBase.FindBaseNode(naviNode);

            EditPageModel editPageModel = new EditPageModel();
            editPageModel.NaviNodeId = naviNode.Id;

            var layout = _db.Set<PageLayout>().SingleOrDefault(e => e.Id == layoutId);

            if (layout == null)
            {
                layout = _db.Set<PageLayout>().First();
            }

            bool havingWebPart = false;

            StringBuilder webpartHeaders = new StringBuilder();
            string template = layout.Template;
            var tempPage = new CmsPage
            {
                Id = id,
                Html = new CmsPageHtml
                {
                    Content = layout.Template
                },
                Layout = layout.Id,
                NaviNode = naviNode
            };

            // hack
            if (tempPage.Layout == 10)
            {
                tempPage.Html.Content = new JavaScriptSerializer().Serialize(new
                {
                    Summaryregion = "Post Summary",
                    region1 = "Post Content"
                });
            }
            editPageModel.Content = _pageEngine.ReplaceTokens(
                page: tempPage,
                webpartHeaders: webpartHeaders,
                havingWebPart: out havingWebPart,
                controllerContext: ControllerContext,
                isEditState: true);
            editPageModel.Layout = layout.Id;
            editPageModel.Type = layout.Type.Title;
            ViewBag.HavingWebPart = havingWebPart;
            ViewBag.WebpartHeaders = webpartHeaders.ToString();

            // The first page in the menu is the return page
            // if create page action is canceled.
            CmsPage returnPage = naviNode.Pages.OrderBy(e => e.MenuOrder).FirstOrDefault();
            ViewBag.ReturnUrl = "/" + (returnPage == null ? "" : returnPage.Url);

            editPageModel.Status = CmsPage.STATUS_EDITING_START;
            editPageModel.IsCommentable = true;
            return View(editPageModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(EditPageModel editPage, string isCreateNewSection, string newSectionName)
        {
            NaviNode currentNode = _db.Set<NaviNode>().Single(e => e.Id == editPage.NaviNodeId);

            PermissionType accessMode = SecurityHelper.NaviAccessMode(_db, currentNode);

            if (accessMode < PermissionType.Create)
            {
                throw new Exception("Access Denided.");
            }

            // TODO: add site specical chc
            if (CmsHelper.Site == "fratalk") {
                if (string.IsNullOrEmpty(editPage.Keywords))
                {
                    ModelState.AddModelError("keywords", "The topics field is required.");
                }
            }


            if (ModelState.IsValid)
            {
                CmsPage page = new CmsPage();
                CopyProperties(editPage, page);

                page.Status = CmsPage.STATUS_CHANGE_SAVED;
                
                if(editPage.Layout == 8)
                {
                    //TODO HARD CODED
                    isCreateNewSection = "1";
                    newSectionName = editPage.Title;
                    page.Title = "Summary";
                    page.NaviTitle = "Summary";
                }

                if (isCreateNewSection == "1" && newSectionName != null && newSectionName.Length > 0)
                {
                    NaviNode naviNode = new NaviNode();
                    naviNode.NodeName = newSectionName;
                    naviNode.Parent = page.NaviNode;
                    naviNode.MenuOrder = CountChildren(page.NaviNode.Id);
                    naviNode.Type = page.NaviNode.Type;

                    naviNode.Modified = DateTime.Now;
                    naviNode.ModifiedBy = User.Identity.Name.ToUpper();
                    naviNode.Created = DateTime.Now;
                    naviNode.CreatedBy = User.Identity.Name.ToUpper();

                    page.NaviNode = naviNode;
                    page.MenuOrder = 0;
                    page.Type = _db.Set<ContentType>().SingleOrDefault(e => e.Title == "BLOG HOME") ?? naviNode.Type;
                }
                else
                {
                    page.MenuOrder = CountChildren(page.NaviNode.Id);
                    page.Type = page.NaviNode.Type;
                }
                _urlHelper.UpdatePageUrl(page);

                // Only save the page when it has a unique URL.
                CmsPage pageWithDuplicateUrl = _db.Set<CmsPage>().SingleOrDefault(e => e.Url.Equals(page.Url, StringComparison.InvariantCultureIgnoreCase));
                if (pageWithDuplicateUrl == null)
                {
                    _db.Set<CmsPage>().Add(page);
                    _db.SaveChanges();
                    return Redirect("/" + page.Url);
                }
                else
                {
                    ModelState.AddModelError("NaviTitle", "Cannot have same page URL in the same navigation section.");
                }
            }
            // The first page in the menu is the return page
            // if create page action is canceled.
            CmsPage returnPage = currentNode.Pages.OrderBy(e => e.MenuOrder).FirstOrDefault();
            ViewBag.ReturnUrl = "/" + (returnPage == null ? "" : returnPage.Url);

            ViewBag.NaviNode = currentNode;
            ViewBag.BaseNode = CmsPageBase.FindBaseNode(currentNode);
            ViewBag.AccessMode = accessMode;

            bool havingWebPart = false;
            editPage.Content = _pageEngine.ReplaceTokens(
                page: new CmsPage
                {
                    Id = editPage.Id,
                    Html = new CmsPageHtml
                    {
                        Content = editPage.Content
                    },
                    NaviNode = currentNode,
                    Layout = editPage.Layout
                },
                webpartHeaders: new StringBuilder(),
                havingWebPart: out havingWebPart,
                controllerContext: this.ControllerContext,
                isEditState: true);

            return View(editPage);
        }
        #endregion

        #region edit

        public ActionResult Edit(string id)
        {
            int internalId = CmsPage.FromFriendlyId(id);
            CmsPage page = _db.Set<CmsPage>().Single(e => e.Id == internalId);

            PermissionType accessMode = SecurityHelper.PageAccessMode(_db, page);

            if (accessMode < PermissionType.Edit)
            {
                //return RedirectToAction("Page", "Cms", new { id = id });
                return Redirect("/" + page.Url);
            }

            if ((page.Status == CmsPage.STATUS_EDITING_START ||
                page.Status == CmsPage.STATUS_EDITING_AGAIN)
                && page.ModifiedBy != User.Identity.Name.ToUpper())
            {
                //return RedirectToAction("Page", "Cms", new { id = page.FriendlyId });
                return Redirect("/" + page.Url);
            }

            EditPageModel editPage = new EditPageModel();

            CopyProperties(page, editPage);
            
            bool havingWebPart = false;
            
            StringBuilder webpartHeaders = new StringBuilder();
            editPage.Content = _pageEngine.ReplaceTokens(
                page: page,
                webpartHeaders: webpartHeaders,
                havingWebPart: out havingWebPart, 
                controllerContext: this.ControllerContext,
                isEditState: true);

            ViewBag.HavingWebPart = havingWebPart;
            ViewBag.WebpartHeaders = webpartHeaders.ToString();

            ViewBag.NaviNode = page.NaviNode;
            ViewBag.BaseNode = CmsPageBase.FindBaseNode(page);
            ViewBag.ModifiedBy = HttpContext.User.Identity.Name;
            ViewBag.AccessMode = accessMode;
            ViewBag.Modified = DateTime.Now;
            ViewBag.Status = CmsPage.STATUS_EDITING_START;
            editPage.Status = CmsPage.STATUS_EDITING_START;
            return View(editPage);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(EditPageModel editPage)
        {
            CmsPage page = _db.Set<CmsPage>().Single(e => e.Id == editPage.Id);
            PermissionType accessMode = SecurityHelper.PageAccessMode(_db, page);
            if (accessMode < PermissionType.Edit)
            {
                throw new Exception("Access Denided.");
            }

            // TODO Check config to see what the page content format is
            if (!editPage.Content.StartsWith("{"))
            {
                ModelState.AddModelError("Error", "Content format does not match. Please refresh your page.");
            }

            if (ModelState.IsValid)
            {
                CopyProperties(editPage, page);
                page.Status = CmsPage.STATUS_CHANGE_SAVED;

                if (editPage.Layout == 8)
                {
                    //TODO HARD CODED
                    page.Title = "Summary";
                }

                // Only save the page when it has a unique URL.
                CmsPage pageWithDuplicateUrl = _db.Set<CmsPage>().SingleOrDefault(e => e.Url.Equals(page.Url, StringComparison.InvariantCultureIgnoreCase));
                if (pageWithDuplicateUrl == null || pageWithDuplicateUrl.Id == editPage.Id)
                {
                    _db.SaveChanges();
                    return Redirect("/" + page.Url);
                }
                else
                {
                    ModelState.AddModelError("NaviTitle", "Cannot have same page URL in the same navigation section.");
                }
                //return RedirectToAction("Page", "Cms", new { id = page.FriendlyId });
            }

            var naviNode = page.NaviNode;
            ViewBag.NaviNode = naviNode;
            ViewBag.BaseNode = CmsPageBase.FindBaseNode(naviNode);
            ViewBag.AccessMode = accessMode;
            ViewBag.Status = CmsPage.STATUS_EDITING_START;
            ViewBag.Modified = DateTime.Now;

            bool havingWebPart = false;
            editPage.Content = _pageEngine.ReplaceTokens(
                page: page,
                webpartHeaders: new StringBuilder(),
                havingWebPart: out havingWebPart,
                controllerContext: this.ControllerContext,
                isEditState: true);

            return View(editPage);
        }
        #endregion

        [HttpPost, ActionName("Publish")]
        public ActionResult PublishConfirmed(int id)
        {
            CmsPage page = _db.Set<CmsPage>().Find(id);
            _db.Set<CmsPage>().Remove(page);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        
        public JsonResult CanDeletePage(int id)
        {
            //int internalId = CmsPage.FromFriendlyId(id);
            int internalId = id;

            CmsPage page = _db.Set<CmsPage>().Find(internalId);

            PermissionType accessMode = SecurityHelper.PageAccessMode(_db, page);

            if (accessMode < PermissionType.Create)
            {
                return Json(new { Message = "no" }, JsonRequestBehavior.AllowGet);
            }

            int parentId = page.NaviNode.Id;

            int numOfPages = _db.Set<CmsPage>().Where(x => x.NaviNode.Id == parentId).Count();

            int numOfFolder =
                _db.Set<NaviNode>().Where(x => x.Parent.Id == parentId).Count();


            if (numOfPages > 1)
            {
                return Json(new { Message = "yes" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (numOfFolder == 0)
                {
                    return Json(new { Message = "yes" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Message = "no" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        //
        // GET: /Page/Delete/5

        public ActionResult Delete(string id)
        {
            int internalId = CmsPage.FromFriendlyId(id);

            CmsPage page = _db.Set<CmsPage>().Find(internalId);

            PermissionType accessMode = SecurityHelper.PageAccessMode(_db, page);

            if (accessMode < PermissionType.Create)
            {
                throw new Exception("Access Denided.");
            }

            return View(page);
        }

        //
        // POST: /Page/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            int internalId = CmsPage.FromFriendlyId(id);
            CmsPage page = _db.Set<CmsPage>().Find(internalId);

            PermissionType accessMode = SecurityHelper.PageAccessMode(_db, page);

            if (accessMode < PermissionType.Create)
            {
                throw new Exception("Access Denided.");
            }

            int parentId = page.NaviNode.Id;

            int numOfPages = _db.Set<CmsPage>().Where(x => x.NaviNode.Id == parentId).Count();

            int numOfFolder =
                _db.Set<NaviNode>().Where(x => x.Parent.Id == parentId).Count();

            if (numOfPages > 1)
            {
                _db.Set<CmsPage>().Remove(page);
                _db.SaveChanges();
            }
            else
            {
                if (numOfFolder == 0)
                {
                    //delete both page and folder

                    _db.Set<CmsPage>().Remove(page);
                    _db.Set<NaviNode>().Remove(page.NaviNode);
                    _db.SaveChanges();
                }
            }

            return RedirectToAction("Navi", "Section", new { id = parentId });
            //return RedirectToAction("Index");
        }

        #region private methods
        private void RestoreLastVersion(CmsPage cmsPage)
        {
            int innerId = cmsPage.Id;
            VerPage verPage =
                (from v in _db.Set<VerPage>()
                 where v.Id == innerId
                 orderby v.VerId descending
                 select v).First();

            if (verPage == null)
            {
                return;
            }

            CopyProperties(verPage, cmsPage);
        }

        private void CopyProperties(VerPage verPage, CmsPage cmsPage)
        {
            cmsPage.Title = verPage.Title;
            cmsPage.NaviTitle = verPage.NaviTitle;

            if (cmsPage.Html == null)
            {
                cmsPage.Html = new CmsPageHtml();
            }

            cmsPage.Html.Content = verPage.Html.Content;
            cmsPage.Html.Sidebar = verPage.Html.Sidebar;
            cmsPage.Html.Header = verPage.Html.Header;

            cmsPage.Description = verPage.Description;
            cmsPage.Keywords = verPage.Keywords;
            cmsPage.Layout = verPage.Layout;

            //navinode does not need to be restored
            //pubPage.NaviNode = cmsPage.NaviNode;

            cmsPage.NaviTitle = String.IsNullOrEmpty(verPage.NaviTitle)? verPage.Title : verPage.NaviTitle;
            cmsPage.IsPublished = true;

            // TODO: why normal in the first place, should be archived all the time for verpage
            //cmsPage.Status = CmsPage.STATUS_NORMAL;
            cmsPage.Status = verPage.Status;

            cmsPage.Modified = verPage.Modified;
            cmsPage.ModifiedBy = verPage.ModifiedBy;
            cmsPage.Created = verPage.Created;
            cmsPage.CreatedBy = verPage.CreatedBy;

            cmsPage.Url = verPage.Url;
        }

        private void PublishDraft(CmsPage cmsPage)
        {
            int innerId = cmsPage.Id;
            PubPage pubPage = _db.Set<PubPage>().SingleOrDefault(e => e.Id == innerId);

            if (pubPage == null)
            {
                pubPage = new PubPage();
                pubPage.Id = cmsPage.Id;
                _db.Set<PubPage>().Add(pubPage);
            }
            else
            {
                ((DbContext)_db).Entry(pubPage).State = EntityState.Modified;
            }

            pubPage.Type = cmsPage.Type;
            pubPage.Commentable = cmsPage.Commentable;
            pubPage.Hidden = cmsPage.Hidden;
            pubPage.Title = cmsPage.Title;
            pubPage.NaviTitle = cmsPage.NaviTitle;
            pubPage.Layout = cmsPage.Layout;
            pubPage.MenuOrder = cmsPage.MenuOrder;

            if (pubPage.Html == null)
            {
                pubPage.Html = new PubPageHtml();
            }

            pubPage.Html.Content = cmsPage.Html.Content;
            pubPage.Html.Sidebar = cmsPage.Html.Sidebar;
            pubPage.Html.Header = cmsPage.Html.Header;

            pubPage.Description = cmsPage.Description;
            pubPage.Keywords = cmsPage.Keywords;
            pubPage.Layout = cmsPage.Layout;

            //pubPage.NaviNode = cmsPage.NaviNode;
            //don't save navi node to remove constraint
            pubPage.NaviNode = cmsPage.NaviNode;

            pubPage.NaviTitle = cmsPage.NaviTitle;

            pubPage.Modified = cmsPage.Modified;
            pubPage.ModifiedBy = cmsPage.ModifiedBy;
            pubPage.Created = cmsPage.Created;
            pubPage.CreatedBy = cmsPage.CreatedBy;

            pubPage.Url = cmsPage.Url;

            //db.SaveChanges();
            //will be save later
        }

        private void SaveVersion(CmsPage cmsPage)
        {
            var currentLiveVer = _db.Set<VerPage>().SingleOrDefault(e => e.Id == cmsPage.Id && e.IsPublished);
            if (currentLiveVer != null)
            {
                currentLiveVer.IsPublished = false;
            }
            int innerId = cmsPage.Id;
            VerPage verPage = new VerPage();
            verPage.Id = cmsPage.Id;

            _db.Set<VerPage>().Add(verPage);

            verPage.Title = cmsPage.Title;
            verPage.NaviTitle = cmsPage.NaviTitle;
            verPage.Layout = cmsPage.Layout;
            verPage.MenuOrder = cmsPage.MenuOrder;
            verPage.Type = cmsPage.Type;
            verPage.Commentable = cmsPage.Commentable;
            verPage.Hidden = cmsPage.Hidden;

            verPage.Html = new VerPageHtml();
            verPage.Html.Content = cmsPage.Html.Content;
            verPage.Html.Sidebar = cmsPage.Html.Sidebar;
            verPage.Html.Header = cmsPage.Html.Header;


            verPage.Description = cmsPage.Description;
            verPage.Keywords = cmsPage.Keywords;
            verPage.Layout = cmsPage.Layout;

            //pubPage.NaviNode = cmsPage.NaviNode;
            //don't save navi node to remove constraint
            verPage.NaviNodeId =
                cmsPage.NaviNode == null ? 0 : cmsPage.NaviNode.Id;

            verPage.NaviTitle = cmsPage.NaviTitle;

            verPage.Modified = cmsPage.Modified;
            verPage.ModifiedBy = cmsPage.ModifiedBy;
            verPage.Created = cmsPage.Created;
            verPage.CreatedBy = cmsPage.CreatedBy;

            verPage.Published = DateTime.Now;
            verPage.PublishedBy = User.Identity.Name.ToUpper();

            verPage.Status = CmsPage.STATUS_ARCHIVED;

            verPage.Url = cmsPage.Url;

            verPage.IsPublished = true;
            //db.SaveChanges();
            //will be save later
        }

        private void CopyProperties(EditPageModel editPage, CmsPage page)
        {
            //page.MenuOrder = editPage.MenuOrder;

            page.Title = editPage.Title;
            page.NaviTitle = String.IsNullOrEmpty(editPage.NaviTitle) ? editPage.Title : editPage.NaviTitle;
            page.Layout = editPage.Layout;

            if (page.Html == null)
            {
                page.Html = new CmsPageHtml();
                page.Html.Content = editPage.Content;
                page.Html.Sidebar = editPage.Sidebar;
                page.Html.Header = editPage.Header;
                page.Html.Summary = editPage.Summary;
            }
            else
            {
                page.Html.Content = editPage.Content;
                page.Html.Sidebar = editPage.Sidebar;
                page.Html.Header = editPage.Header;
                page.Html.Summary = editPage.Summary;
          
            }

            page.Description = editPage.Description;
            page.Keywords = string.Join(",", editPage.Keywords);

            //TODO HARD CODED
            if (page.NaviNode == null) 
            { 
                page.NaviNode = _db.Set<NaviNode>().Single(e => e.Id == (editPage.Layout == 8 ? 33 : editPage.NaviNodeId));
            }
            
            page.Modified = DateTime.Now;
            page.ModifiedBy = User.Identity.Name.ToUpper();

            if (page.CreatedBy == null)
            {
                page.Created = DateTime.Now;
                page.CreatedBy = User.Identity.Name.ToUpper();
            }

            if (editPage.Type == "BLOG" && editPage.Created.HasValue) 
            { 
                page.Created = editPage.Created.Value;
            }
            page.Type = _db.Set<ContentType>().SingleOrDefault(e => e.Title == editPage.Type) ??
                _db.Set<ContentType>().Single(e => e.Title == "PAGE");
            page.Commentable = editPage.IsCommentable;
            _urlHelper.UpdatePageUrl(page);
        }

        private void CopyProperties(CmsPage page, EditPageModel editPage)
        {
            editPage.Id = page.Id;
            //TODO HARD CODED
            if(page.Type.Title == "BLOG HOME")
            {
                editPage.Title = page.NaviNode.NodeName;
            }
            else
            {
                editPage.Title = page.Title;
            }
            editPage.NaviTitle = String.IsNullOrEmpty(page.NaviTitle) ? page.Title : page.NaviTitle;
            editPage.Layout = page.Layout;

            if (page.Html != null)
            {
                editPage.Content = page.Html.Content;
                editPage.Sidebar = page.Html.Sidebar;
                editPage.Header = page.Html.Header;
                editPage.Summary = page.Html.Summary;
            }

            editPage.Description = page.Description;
            editPage.Keywords = page.Keywords;
            editPage.NaviNodeId = page.NaviNode.Id;

            editPage.IsPublished = page.IsPublished;

            editPage.Created = page.Created;
            editPage.Type = page.Type == null? "PAGE" : page.Type.Title;
            editPage.IsCommentable = page.Commentable;

        }

        private int CountChildren(int parentId)
        {

            int numOfFolder =
                _db.Set<NaviNode>().Where(x => x.Parent.Id == parentId).Count();

            int numOfPages = _db.Set<CmsPage>().Where(x => x.NaviNode.Id == parentId).Count();


            return numOfFolder + numOfPages;
        }
        #endregion
    }
}