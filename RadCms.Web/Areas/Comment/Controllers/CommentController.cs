using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RadCms.Data;
using RadCms.Models;
using RadCms.Recaptcha;

namespace RadCms.Web.Areas.Comment.Controllers
{
    using Entities;
    using Helpers;

    public class CommentController : Controller
    {
        private IRepository<Comment> _commentRepo;
        private IDbContext _db;
        
        public CommentController(IRepository<Comment> commentRepo, IDbContext db)
        {
            _commentRepo = commentRepo;
            _db = db;
        }

        [HttpPost]
        [ValidateInput(false)]
        [RecaptchaControlMvc.CaptchaValidator]
        public ActionResult Add(Comment comment, string veryspecialname, string returnUrl, bool captchaValid = false, string captchaErrorMessage = "")
        {
            if (!string.IsNullOrEmpty(veryspecialname))
            {
                return View(comment);
            }

            if (!captchaValid)
            {
                ModelState.AddModelError("recaptcha", "The reCAPTCHA is not verified. Please try again.");
            }

            if (ModelState.IsValid)
            {
                var createBy = "Public User";
                if(User != null)
                {
                    createBy = User.Identity.Name;
                }
                comment.Created = DateTime.Now;
                comment.CreatedBy = createBy;
                comment.Modified = comment.Created;
                comment.ModifiedBy = comment.CreatedBy;
                comment.OrigContent = comment.Content;
                comment.Status = CommentStatus.Pending;

                _commentRepo.Add(comment);
                _commentRepo.Save();

                ViewBag.ReturnUrl = returnUrl;
                return View(comment);
            }
            
            // Pass ViewData to redirect, so webpart will have model state and data.
            TempData["ViewData"] = ViewData;
            return Redirect(returnUrl);
        }
    }
}