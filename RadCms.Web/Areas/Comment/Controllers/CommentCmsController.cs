using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace RadCms.Web.Areas.Comment.Controllers
{
    using Entities;
    using Mvc;
    using Helpers;
    using Data;
    using RadCms.Entities;
    using Models;

    public class CommentCmsController : CmsControllerBase
    {
        private IRepository<Comment> _commentRepo;
        private IDbContext _db;

        public CommentCmsController(IRepository<Comment> commentRepo, IDbContext db)
        {
            _commentRepo = commentRepo;
            _db = db;
        }
        
        public ViewResult Index(int? status, int page = -1)
        {
            var comments = (from c in _commentRepo.GetAll()
                           join p in _db.Set<CmsPage>() on c.PageId equals p.Id
                           select new PageComment
                           {
                               Page = p,
                               Comment = c
                           }).ToList();

            Dictionary<int, string> titleList =  comments.Select(e => new { e.Page.Id, e.Page.Title }).Distinct().ToDictionary(e => e.Id, e => e.Title);
            titleList.Add(-1, "ALL");

            comments = comments.Where(pageComment =>
                {
                    if (pageComment.Page == null)
                    {
                        return false;
                    }
                    var selected = true;
                    if (status != null)
                    {
                        selected = status == pageComment.Comment.Status;
                    }

                    if (page != -1)
                    {
                        selected = selected && page == pageComment.Comment.PageId;
                    }

                    return selected;
                }).OrderBy(e => e.Comment.Status).ToList();

            ViewBag.Page = page;
            ViewBag.Status = status;
            ViewBag.TitleList = titleList.OrderBy(e => e.Key);
            ViewBag.StatusList = CommentStatus.List;
            return View(comments);
        }

        private int ToInternal(string externalId)
        {
            return Convert.ToInt32(externalId.Substring(1));
        }

        [HttpPost]
        public JsonResult UpdateStatus(int id, int status)
        {
            Comment comment = _commentRepo.Get(id);
            comment.Status = status;
            _commentRepo.Save();
            return Json(new
            {
                Status = "Success"
            });
        }

        public ViewResult Details(int id)
        {
            Comment comment = _commentRepo.Get(id);
            return View(comment);
        }
 
        public ActionResult Edit(int id)
        {
            Comment comment = _commentRepo.Get(id);
            return View(comment);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.Modified = DateTime.Now;
                comment.ModifiedBy = User.Identity.Name.ToUpper();
                _commentRepo.Update(comment);
                _commentRepo.Save();
                return RedirectToAction("Index");
            }
            return View(comment);
        }

        public ActionResult Delete(int id)
        {
            Comment comment = _commentRepo.Get(id);
            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = _commentRepo.Get(id);
            comment.Status = CommentStatus.Deleted;
            _commentRepo.Save();
            return RedirectToAction("Index");
        }
    }
}