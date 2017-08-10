using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using RadCms.Data;
using RadCms.Entities;
using RadCms.Helpers;
using RadCms.Mvc.ViewEngines.Razor;
using RadCms.Security;
using RadCms.Services;
using RadCms.Mvc;

namespace RadCms.Core.Containers.Controllers
{
    public class CmsController: PubControllerBase
    {
        private IDbContext _db;
        private IPageEngine _pageEngine;

        public CmsController(IDbContext db, IPageEngine pageEngine)
        {
            _pageEngine = pageEngine;
            _db = db;
        }

        public ActionResult Error404()
        {
            Response.StatusCode = 404;
            return View();
        }

        public ActionResult Error500()
        {
            Response.StatusCode = 500;
            return View();
        }

        public JsonResult Find()
        {
            Response.CacheControl = "no-cache";
            Response.Cache.SetETag((Guid.NewGuid()).ToString());

            string term = Request["term"];

            if (User.Identity.IsAuthenticated)
            {

                var result1 = from m in
                                  ((from p in _db.Set<PubPage>()
                                    where p.Title.Contains(term)
                                    orderby p.Modified descending
                                    select new { p.Title, p.Id }
                                      ).Take(5).ToList())
                              select new
                              {
                                  label = m.Title,
                                  value = CmsPage.ToFriendlyId(m.Id),
                                  pageType = ""
                              };

                return Json(result1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result1 = from m in
                                  ((from p in _db.Set<PubPage>()
                                    where p.Title.Contains(term) && !p.NaviNode.IsSecure
                                    orderby p.Modified descending
                                    select new { p.Title, p.Id }
                                      ).Take(5).ToList())
                              select new
                              {
                                  label = m.Title,
                                  value = CmsPage.ToFriendlyId(m.Id),
                                  pageType = ""
                              };

                return Json(result1, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Page(string id)
        {
#if PUB
            var page = new PageModelBuilder<PubPage>(_db).GetPage(id);
#elif CMS
            var page = new PageModelBuilder<CmsPage>(_db).GetPage(id);
#endif
            if(page == null)
            {
                throw new HttpException(404, "Page not found");
            }

            var webpartHeaders = new StringBuilder();
            var havingWebPart = false;

            page.ContentHtml.Content = _pageEngine.ReplaceTokens(
                page : page,
                webpartHeaders : webpartHeaders,
                havingWebPart : out havingWebPart,
                controllerContext : ControllerContext);

            ViewBag.HavingWebPart = havingWebPart;
            ViewBag.WebpartHeaders = webpartHeaders.ToString();

#if CMS
            ViewBag.AccessMode = SecurityHelper.PageAccessMode(_db, page);
#endif

            ViewBag.NaviNode = page.NaviNode;
            ViewBag.BaseNode = CmsPageBase.FindBaseNode(page);

            var viewHtml = ViewRenderer.RenderViewToString(ControllerContext, "~/Core/Containers/Views/Cms/Page.cshtml", page);
            return Content(viewHtml, "text/html");
        }
    }
}