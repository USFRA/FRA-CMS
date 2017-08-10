using System;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using RadCms.Data;
using RadCms.Entities;
using RadCms.Web.Areas.ImageLibrary.Helpers;
using RadCms.Mvc;

namespace RadCms.Web.Areas.ImageLibrary.Controllers
{
    public class ImageBrowserController : PubControllerBase
    {
        private IRepository<Media> _mediaRepo;

        public ImageBrowserController(IRepository<Media> mediaRepo)
        {
            this._mediaRepo = mediaRepo;
        }

        public ActionResult Image(string path)
        {
            var nodeId = NodeHelper.GetNodeIdFromPath(path);
            var imageName = NodeHelper.GetNameFromPath(path);
            var media = _mediaRepo.GetAll().Where(e => e.NaviNodeId == nodeId && e.Title.Equals(imageName, StringComparison.InvariantCultureIgnoreCase)).Select(e => e).ToList()[0];
            var ms = new MemoryStream(media.File.FileContent);
            return File(ms, media.File.FileType);
        }
        
    }
}
