using RadCms.Data;
using RadCms.Mvc;
using RadCms.Security;
using System;
using System.Linq;
using System.Web.Mvc;

namespace RadCms.Web.Areas.ContentType.Controllers
{
    using Entities;

    public class ItemCmsController : CmsControllerBase
    {
        private IRepository<ContentType> _repo;
        private IRepository<NaviNode> _nodeRepo;
        private IRepository<CmsPage> _pageRepo;
        private IRepository<PageLayout> _layoutRepo;
        public ItemCmsController(IRepository<ContentType> repo, IRepository<NaviNode> nodeRepo, IRepository<CmsPage> pageRepo, IRepository<PageLayout> layoutRepo)
        {
            if (!SecurityHelper.IsAdmin())
            {
                throw new UnauthorizedAccessException();
            }
            _repo = repo;
            _nodeRepo = nodeRepo;
            _pageRepo = pageRepo;
            _layoutRepo = layoutRepo;
        }

        public ActionResult Index()
        {
            var items = _repo.GetAll();
            return View(items);
        }

        public JsonResult Nodes(int id)
        {
            var items = _nodeRepo.GetAll().Where(e => e.Type.Id == id).Select(e=> new
            {
                Id = e.Id,
                Title = e.NodeName,
                Type = e.Type.Title
            });
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Layouts(int id)
        {
            var items = _layoutRepo.GetAll().Where(e => e.Type.Id == id).Select(e=> new
            {
                Id = e.Id,
                Title = e.Title,
                Type = e.Type.Title
            });
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Pages(int id)
        {
            var items = _pageRepo.GetAll().Where(e => e.Type.Id == id).Select(e=> new {
                Id = e.Id,
                Title = e.Title,
                Url = e.Url,
                Type = e.Type.Title
            });
            return Json(items, JsonRequestBehavior.AllowGet);
        }
    }
}