using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using RadCms.Entities;
using RadCms.Models;
using RadCms.Helpers;
using RadCms.Web.Areas.ImageLibrary.Models;
using RadCms.Mvc;
using RadCms.Data;

namespace RadCms.Web.Areas.ImageLibrary.Controllers
{
    public class MediaCmsController : CmsControllerBase
    {
        private IRepository<Media> _mediaRepo;

        public MediaCmsController(IRepository<Media> mediaRepo)
        {
            _mediaRepo = mediaRepo;
        }


        public JsonResult Node(string key)
        {
            //Response.CacheControl = "no-cache";
            //Response.Cache.SetETag((Guid.NewGuid()).ToString());

            int parentId = Convert.ToInt32(key);

            return Json(GetChildren(parentId), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<TreeModel> GetChildren(int parentId)
        {
            var files = (from entity in _mediaRepo.GetAll().Where(x => x.NaviNodeId == parentId)
                         select new TreeModel
                         {
                             Id = entity.Id,
                             Type = "P",
                             title = entity.Title,
                             hasChildren = false,
                             isLazy = false,
                         }).ToList();


            return files;
        }

        //
        // GET: /Media/

        public ViewResult Index()
        {
            return View(_mediaRepo.GetAll().ToList());
        }

        //
        // GET: /Media/Details/5

        public ViewResult Details(int id)
        {
            Media media = _mediaRepo.Get(id);
            return View(media);
        }

        private void CopyProperties(EditMediaModel editMedia, Media media)
        {
            media.Title = editMedia.Title;

            media.Modified = DateTime.Now;
            media.ModifiedBy = User.Identity.Name.ToUpper();

            if (media.CreatedBy == null)
            {
                media.Created = DateTime.Now;
                media.CreatedBy = User.Identity.Name.ToUpper();
            }

            media.NaviNodeId = editMedia.NaviNodeId;

            foreach (string upload in Request.Files)
            {
                HttpPostedFileBase f = Request.Files[upload];
                if (f != null && f.ContentLength > 0)
                {
                    if (media.File == null)
                    {
                        media.File = new MediaFile();
                        media.File.Created = DateTime.Now;
                        media.File.CreatedBy = User.Identity.Name.ToUpper();
                    }

                    media.File.FileName = Path.GetFileName(f.FileName);
                    media.File.FileType = CmsHelper.GetMimeType(Path.GetExtension(f.FileName));

                    //fix mine type by IE
                    if (media.File.FileType == "image/pjpeg")
                    {
                        media.File.FileType = "image/jpeg";
                    }
                    else if (media.File.FileType == "image/x-png")
                    {
                        media.File.FileType = "image/png";
                    }

                    media.File.FileSize = f.ContentLength;

                    if (media.Title == null)
                    {
                        media.Title = media.File.FileName;
                    }

                    byte[] fileContent = new byte[f.ContentLength];
                    f.InputStream.Read(fileContent, 0, f.ContentLength);

                    media.File.FileContent = fileContent;

                    media.File.Modified = DateTime.Now;
                    media.File.ModifiedBy = User.Identity.Name.ToUpper();
                }

                break;
            }
        }

        private void CopyProperties(Media media, EditMediaModel editMedia)
        {
            editMedia.Id = media.Id;
            editMedia.Title = media.Title;
            editMedia.NaviNodeId = media.NaviNodeId;
        }

        //
        // GET: /Media/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Media/Create

        [HttpPost]
        public ActionResult Create(EditMediaModel editMedia)
        {
            if (ModelState.IsValid)
            {
                var media = new Media();

                CopyProperties(editMedia, media);

                _mediaRepo.Add(media);
                _mediaRepo.Save();

                return RedirectToAction("Index");
            }

            return View(editMedia);
        }

        [HttpPost]
        public ActionResult Upload(EditMediaModel editMedia)
        {
            //Modified by Eugene=======================

            //if (Request.Files[upload]==null)

            //=========================================
             Media media = null;

            if (ModelState.IsValid)
            {
                //overwrite file with the same name
                foreach (string upload in Request.Files)
                {
                    HttpPostedFileBase f = Request.Files[upload];
                    if (f != null && f.ContentLength > 0)
                    {
                        string fileName =  Path.GetFileName(f.FileName);
                        int? sectionId = editMedia.NaviNodeId;

                        var existingMedias = _mediaRepo.GetAll().Where(e => e.Title == fileName
                            && e.NaviNodeId == sectionId);
                        if (existingMedias.Count() > 0)
                        {
                            media = existingMedias.First();
                        }
                                        
                        break;
                    }
                }

                if (media == null)
                {
                    media = new Media();
                    _mediaRepo.Add(media);
                }
            
                CopyProperties(editMedia, media);
               
                _mediaRepo.Save();
            }

            return RedirectToAction("SelectMedia", "Cms", null);
        }

        //
        // GET: /Media/Edit/5

        public ActionResult Edit(int id)
        {
            Media media = _mediaRepo.Get(id);
            return View(media);
        }

        //
        // POST: /Media/Edit/5

        [HttpPost]
        public ActionResult Edit(Media media)
        {
            if (ModelState.IsValid)
            {
                _mediaRepo.Update(media);
                _mediaRepo.Save();
                return RedirectToAction("Index");
            }
            return View(media);
        }

        //
        // GET: /Media/Delete/5

        public ActionResult Delete(int id)
        {
            var media = _mediaRepo.Get(id);
            return View(media);
        }

        //
        // POST: /Media/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var media = _mediaRepo.Get(id);
            _mediaRepo.Delete(media);
            _mediaRepo.Save();
            return RedirectToAction("Index");
        }
    }
}