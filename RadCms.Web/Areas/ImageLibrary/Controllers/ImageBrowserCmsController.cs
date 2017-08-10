using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RadCms.Entities;
using System.IO;
using RadCms.Mvc;
using RadCms.Data;
using RadCms.Web.Areas.ImageLibrary.Helpers;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace RadCms.Web.Areas.ImageLibrary.Controllers
{
    public class ImageBrowserCmsController : CmsControllerBase
    {
        private const int ThumbnailHeight = 80;
        private const int ThumbnailWidth = 80;

        private IRepository<Media> _mediaRepo;

        public ImageBrowserCmsController(IRepository<Media> mediaRepo)
        {
            this._mediaRepo = mediaRepo;
        }

        public ActionResult Index(string type, string path)
        {
            var nodeId = NodeHelper.GetNodeIdFromPath(path);
            var imageName = NodeHelper.GetNameFromPath(path);
            var medias = _mediaRepo.GetAll().Where(e => e.NaviNodeId == nodeId).ToList();
            ViewBag.NodeId = nodeId;
            return View(medias);
        }

        [OutputCache(NoStore = true, Duration = 0, Location = System.Web.UI.OutputCacheLocation.None, VaryByParam = "*")]
        public ActionResult Thumbnail(string path)
        {
            var nodeId = NodeHelper.GetNodeIdFromPath(path);
            var imageName = NodeHelper.GetNameFromPath(path);
            var media = _mediaRepo.GetAll().Where(e => e.NaviNodeId == nodeId && e.Title.Equals(imageName, StringComparison.InvariantCultureIgnoreCase)).Select(e => e).ToList()[0];
            var ms = new MemoryStream(media.File.FileContent);
            var image = Image.FromStream(ms);
            var thumbnail = ResizeImage(image, ThumbnailWidth, ThumbnailHeight);
            var thumbnailStream = new MemoryStream();
            thumbnail.Save(thumbnailStream, image.RawFormat);
            return File(thumbnailStream.ToArray(), media.File.FileType, media.File.FileName);
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.Clamp);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public ActionResult Destroy(string path, string name, string type)
        {
            if(!String.IsNullOrEmpty(type) && type.Equals("f", StringComparison.InvariantCultureIgnoreCase))
            {
                var nodeId = NodeHelper.GetNodeIdFromPath(path);
                var m = _mediaRepo.GetAll().Where(e => e.NaviNodeId == nodeId && e.Title.Equals(name, StringComparison.InvariantCultureIgnoreCase)).ToList();
             
                if(m != null)
                {
                    _mediaRepo.DeleteAll(m);
                    _mediaRepo.Save();
                }
            }

            return Json(new { 
                status = "success"
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">{sectionId}/{folders}</param>
        /// <returns></returns>
        public JsonResult Read(string path)
        {
            var nodeId = NodeHelper.GetNodeIdFromPath(path);
            var images = _mediaRepo.GetAll().Where(e => e.NaviNodeId == nodeId).Select(e => new
            {
                name = e.Title,
                type = "f",
                size = e.File.FileSize
            });

            return Json(images, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Upload(string path, HttpPostedFileBase file)
        {
            var nodeId = NodeHelper.GetNodeIdFromPath(path);
            var created = DateTime.Now;
            var createdBy = HttpContext.User.Identity.Name;

            var mediaFile = new MediaFile{
                Created = created,
                CreatedBy = createdBy,
                FileName = file.FileName,
                FileSize = file.ContentLength,
                FileType = file.ContentType,
                Modified = created,
                ModifiedBy = createdBy
            };
            try
            {
                using (MemoryStream target = new MemoryStream(file.ContentLength))
                {
                    file.InputStream.CopyTo(target);
                    mediaFile.FileContent = target.ToArray();
                }
            }
            catch (Exception e)
            {
                return Json(new { 
                    status = "error",
                    message = e.Message
                });
            }

            var existedFiles = _mediaRepo.GetAll().Where(e => e.NaviNodeId == nodeId && e.Title == file.FileName);
            if(existedFiles.Any())
            {
                var m = existedFiles.First();
                m.Modified = created;
                m.ModifiedBy = createdBy;
                m.File = mediaFile;
                _mediaRepo.Update(m);
            }
            else
            {
                var m = new Media
                {
                    Created = created,
                    CreatedBy = createdBy,
                    File = mediaFile,
                    Modified = created,
                    ModifiedBy = createdBy,
                    NaviNodeId = nodeId,
                    Title = mediaFile.FileName
                };
                _mediaRepo.Add(m);
            }

            try
            {
                _mediaRepo.Save();
            }
            catch(Exception e)
            {
                return Json(new
                {
                    status = "error",
                    message = e.Message
                });
            }
            return Json(new
            {
                size = mediaFile.FileSize,
                name = mediaFile.FileName,
                type = "f"
            });
        }
    }
}
