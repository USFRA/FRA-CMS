using System.Web.Mvc;
using System.Collections.Generic;
using RadCms.Entities;
using System.Linq;
using System;
using RadCms.Data;
using RadCms.Mvc;
using System.Configuration;
using RadCms.Helpers;

namespace RadCms.Controllers
{
    public class CmsAppController : CmsControllerBase
    {
        private IRepository<CmsPage> _pageRepo;
        private IRepository<CmsUser> _userRepo;
        private IRepository<VerPage> _verPageRepo;
        public CmsAppController(IRepository<CmsPage> pageRepo, IRepository<VerPage> verPageRepo, IRepository<CmsUser> userRepo)
        {
            _pageRepo = pageRepo;
            _userRepo = userRepo;
            _verPageRepo = verPageRepo;
        }

        public ViewResult LinksReport(string type)
        {
            string siteRoot = CmsHelper.SiteRoot;
            List<CmsPage> Old = new List<CmsPage>();
            //page.html.content, sidebar
            var pages = _pageRepo.GetAll().ToList();
            foreach (var p in pages)
            {
                if ((p.ContentHtml.Content != null && p.ContentHtml.Content.IndexOf(siteRoot) > 0) 
                    || 
                    (p.ContentHtml.Sidebar != null && p.ContentHtml.Sidebar.IndexOf(siteRoot) > 0))
                {
                    Old.Add(p);
                }
            }
            ViewBag.Old = Old;

            //Match m;
            //string HRefPattern = "href\\s*=\\s*(?:[\"'](?<1>[^\"']*)[\"']|(?<1>\\S+))";

            List<CmsPage> Inners = new List<CmsPage>();
            /*
            foreach (var p in pages)
            {
                string content = p.ContentHtml.Content == null ? "" : p.ContentHtml.Content;
                string sidebar = p.ContentHtml.Sidebar == null ? "" : p.ContentHtml.Sidebar;
                string input = content + sidebar;
                m = Regex.Match(input, HRefPattern,
                                RegexOptions.IgnoreCase | RegexOptions.Compiled);
                while (m.Success)
                {
                    HttpWebRequest webRequest;
                    if (m.Groups[1].ToString().ToLower().Contains("http"))
                    {
                        webRequest = (HttpWebRequest)WebRequest.Create(new Uri(m.Groups[1].ToString()));
                    }
                    else
                    {
                        webRequest = (HttpWebRequest)WebRequest.Create(new Uri(Request.Url.Scheme + "://" + Request.Url.Authority +
                                                    Request.ApplicationPath.TrimEnd('/') + "/" + m.Groups[1]));
                    }
                    webRequest.Method = "GET";
                    try
                    {
                        HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

                        HttpStatusCode c = response.StatusCode;
                        if (c != HttpStatusCode.OK)
                            Inners.Add(p);
                    }
                    catch (Exception)
                    {
                        Inners.Add(p);
                    }
                    m = m.NextMatch();
                }

            }
             */
            ViewBag.Inners = Inners;
            return View();
        }

        public ViewResult AuditReport() {
            return View();
        }

        [HttpPost]
        public JsonResult AuditReport(string start, string end) {
            DateTime sDate;
            DateTime eDate;

            if (!DateTime.TryParse(start, out sDate) || !DateTime.TryParse(end, out eDate))
            {
                sDate = new DateTime(2010, 1, 1);
                eDate = DateTime.Now;
            }

            var createPages = _pageRepo.GetAll().ToList()
                .Where(e => e.Created.CompareTo(sDate) >= 0
                    && e.Created.CompareTo(eDate) < 0)    
                .GroupBy(e => e.CreatedBy)
                .Select(e => new { user = e.Key.ToUpper(), pageCreations = e.Count(), pagePublications = 0, libCreations = 0, libPublications = 0 });

            var publishPages = _verPageRepo.GetAll().ToList()
                .Where(e => e.Published.CompareTo(sDate) >= 0
                    && e.Published.CompareTo(eDate) < 0)
                .GroupBy(e => e.PublishedBy)
                .Select(e => new { user = e.Key.ToUpper(), pageCreations = 0, pagePublications = e.Count(), libCreations = 0, libPublications = 0 });

            
            var report = publishPages.Union(createPages).GroupBy(e => e.user).Select(g => new { user = g.Key, pageCreations = g.Sum(e => e.pageCreations), pagePublications = g.Sum(e => e.pagePublications), libCreations = g.Sum(e => e.libCreations), libPublications = g.Sum(e => e.libPublications) });
            return Json(new
            {
                report = report,
                total = report.Count()
            });
        }

        [HttpGet]
        public ViewResult SingleUser(string username, string start, string end) {
            DateTime sDate = Convert.ToDateTime(start);
            DateTime eDate = Convert.ToDateTime(end);
            CmsUser user = _userRepo.GetAll().Single(e => e.AdName == username);
            var publishPages = _verPageRepo.GetAll()
                .Where(e => e.Published.CompareTo(sDate) >= 0
                    && e.Published.CompareTo(eDate) < 0 && e.PublishedBy == user.AdName)
                .Select(e => e.Title + ", " + e.Published).ToList();

            var createPages = _pageRepo.GetAll()
                .Where(e => e.Created.CompareTo(sDate) >= 0
                    && e.Created.CompareTo(eDate) < 0 && e.CreatedBy == user.AdName)
                .Select(e => e.Title + ", " + e.Created).ToList();

            ViewBag.UserName = user.UserName;
            ViewBag.PublishPages = publishPages;
            ViewBag.CreatePages = createPages;
            return View();
        }
    }
}