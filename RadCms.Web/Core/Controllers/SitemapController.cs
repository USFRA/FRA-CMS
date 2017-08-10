using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using RadCms.Models;
using RadCms.Entities;
using System.Xml;
using RadCms.Data;
using RadCms.Services;
using RadCms.Mvc;

namespace RadCms.Controllers
{
    public class SitemapController : PubControllerBase
    {
        private IRepository<CmsPage> _cmsPageRepo;
        private ITreeModelService<TreeModel> _treeModelService;

        public SitemapController(IRepository<CmsPage> cmsPageRepo,
            ITreeModelService<TreeModel> treeModelService)
        {
            this._cmsPageRepo = cmsPageRepo;
            this._treeModelService = treeModelService;
        }

        public ActionResult Index()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");
            foreach(var p in _cmsPageRepo.GetAll().ToList())
            {
                sb.AppendLine("<url>");
                sb.AppendLine("<loc>");
                //sb.Append(Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/Page/" + p.FriendlyId);
                sb.Append(Request.Url.Scheme + "://" + Request.Url.Authority +
                    Request.ApplicationPath.TrimEnd('/') + "/" + p.Url);
                sb.Append("</loc>");
                sb.AppendLine("<lastmod>");
                sb.Append(p.Modified.ToString("yyyy-MM-dd"));
                sb.Append("</lastmod>");
                sb.AppendLine("<priority>");
                sb.Append("0.500");
                sb.Append("</priority>");
                sb.AppendLine("</url>");
            }
            
            sb.AppendLine("</urlset>");

            return new ContentResult
            {
                ContentType = "text/xml",
                Content = sb.ToString(),
                ContentEncoding = System.Text.Encoding.UTF8
            };
        }

        public ActionResult Page()
        {
            return View(_treeModelService.GetChildren(1));
        }
    }
}
