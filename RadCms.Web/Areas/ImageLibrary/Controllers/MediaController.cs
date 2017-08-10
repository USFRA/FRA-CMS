using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using RadCms.Entities;
using RadCms.Controllers;
using RadCms.Data;
using RadCms.Mvc;
using RadCms.Web.Areas.ImageLibrary.Helpers;

namespace RadCms.Web.Areas.ImageLibrary.Controllers
{
    public class MediaController : PubControllerBase
    {
        private IRepository<Media> _mediaRepo;

        public MediaController(IRepository<Media> mediaRepo)
        {
            _mediaRepo = mediaRepo;
        }

        public ActionResult File(string path)
        {
            var nodeId = NodeHelper.GetNodeIdFromPath(path);
            var fileName = NodeHelper.GetNameFromPath(path);
            var mediaList = _mediaRepo.GetAll().Where(e => e.NaviNodeId == nodeId && e.File.FileName.Equals(fileName, StringComparison.InvariantCultureIgnoreCase)).Select(e => e).ToList();
            Media m;
            if (mediaList.Count > 0)
            {
                m = mediaList[0];
            }
            else
            {
                return new HttpNotFoundResult("File not found");
            }

            if (!this.IsModified(m.Modified))
                return this.NotModified();

            Response.AddHeader("Last-Modified", m.Modified.ToUniversalTime().ToString("R"));
            return File(m.File.FileContent, m.File.FileType);
        }
    }
}