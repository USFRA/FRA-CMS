using System.Web;
using System.Web.Mvc;
using RadCms.Entities;
using System.IO;
using RadCms.Services;
using RadCms.Mvc;
using RadCms.Security;
using System;

namespace RadCms.Web.Areas.Template.Controllers
{
    public class LayoutCmsController : CmsControllerBase
    {
        private ILayoutService _layoutService;

        public LayoutCmsController(ILayoutService service)
        {
            if (!SecurityHelper.IsAdmin())
            {
                throw new UnauthorizedAccessException();
            }
            _layoutService = service;
        }

        // id is SectionType
        public ActionResult Items(string sectionType)
        {
            var items = _layoutService.GetList(sectionType);
            return PartialView("_ItemsPartial", items);
        }

        public ViewResult Index()
        {
            var items = _layoutService.GetList();
            return View(items);
        }

        public FileResult Image(int id)
        {
            var layout = _layoutService.Find(id);

            if (layout == null)
            {
                //there is no default page layout set
                return null;
            }

            if(layout.Image == null)
            {
                var placeHolderPath = Server.MapPath("~/Core/assetsCms/images/imagePlaceHolder.jpg");
                return File(System.IO.File.ReadAllBytes(placeHolderPath), "image/jpg");
            }
            else
            {
                return File(layout.Image, "image/png");
            }
        }

        //
        // GET: /Layout/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Layout/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(PageLayout pagelayout, HttpPostedFileBase ImageFile)
        {
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                using (var ms = new MemoryStream())
                {
                    ImageFile.InputStream.CopyTo(ms);
                    pagelayout.Image = ms.ToArray();
                }
            }

            if (ModelState.IsValid)
            {
                _layoutService.AddLayout(pagelayout);
                return RedirectToAction("Index");  
            }

            return View(pagelayout);
        }
        
        //
        // GET: /Layout/Edit/5
 
        public ActionResult Edit(int id)
        {
            var layout = _layoutService.Find(id);

            if (layout == null)
            {
                //there is no default page layout set
                return null;
            }

            return View(layout);
        }

        //
        // POST: /Layout/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(PageLayout pagelayout, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        ImageFile.InputStream.CopyTo(ms);
                        pagelayout.Image = ms.ToArray();
                    }

                }
                else
                {
                    var originalLayout = _layoutService.Find(pagelayout.Id);
                    pagelayout.Image = originalLayout.Image;
                }

                _layoutService.UpdateLayout(pagelayout);
                
                return RedirectToAction("Index");
            }
            return View(pagelayout);
        }

        //
        // GET: /Layout/Delete/5
 
        public ActionResult Delete(int id)
        {
            var layout = _layoutService.Find(id);

            if (layout == null)
            {
                //there is no default page layout set
                return null;
            }
            return View(layout);
        }

        //
        // POST: /Layout/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _layoutService.DeleteLayout(id);
            return RedirectToAction("Index");
        }

    }
}