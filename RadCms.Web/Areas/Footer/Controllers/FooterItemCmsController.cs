using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace RadCms.Web.Areas.Footer.Controllers
{
    using Data;
    using Entities;
    using Mvc;

    public class FooterItemCmsController : CmsControllerBase
    {
        private IRepository<FooterItem> _footerItemRepo;
        private IRepository<FooterSection> _sectionRepo;
        
        public FooterItemCmsController(IRepository<FooterItem> footerItemRepo,
            IRepository<FooterSection> sectionRepo)
        {
            _footerItemRepo = footerItemRepo;
            _sectionRepo = sectionRepo;
        }

        public List<string> SECTIONS {
            get {
                return TYPES.Keys.ToList();
            }
        }
        public Dictionary<string, FooterSection.SectionType> TYPES
        {
            get
            {
                return _sectionRepo.GetAll().ToDictionary(e => e.Title, e => e.Type);
            }
        }
        public static Dictionary<string, string> TARGETS = new Dictionary<string, string>()
        { 
            {"Open in This Window/Frame", "_self"},
            {"Open in New Window", "_blank"},
            {"Open in Parent Window/Frame", "_parent"},
            {"Open in Top Frame(Replaces All Frames)", "_top"}
        };
        //
        // GET: /FooterItem/

        public ViewResult Index(string section)
        {
            List<FooterItem> items;
            if (String.IsNullOrEmpty(section))
            {
                items = _footerItemRepo.GetAll().ToList();
            }
            else
            {
                var s = _footerItemRepo.GetAll().SingleOrDefault(e => e.Title == section);
                if(s == null)
                    items = _footerItemRepo.GetAll().ToList();
                else
                    items = _footerItemRepo.GetAll().Where(e => e.Section.Id == s.Id).ToList();
            }
            ViewBag.Sections = SECTIONS;
            ViewBag.Targets = TARGETS;
            return View(items);
        }

        //
        // GET: /FooterItem/Details/5

        public ViewResult Details(int id)
        {
            FooterItem footeritem = _footerItemRepo.Get(id);
            ViewBag.Targets = TARGETS;
            ViewBag.Sections = SECTIONS;
            return View(footeritem);
        }
        public JsonResult FooterTypes()
        {
            return Json(TYPES, JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /FooterItem/Create

        public ActionResult Create()
        {
            ViewBag.Targets = TARGETS;
            ViewBag.Sections = SECTIONS;
            ViewBag.Types = TYPES;
            return View();
        } 

        //
        // POST: /FooterItem/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FooterItem footeritem, string header)
        {
            if (string.IsNullOrEmpty(header))
            {
                ModelState.AddModelError("", "Section is required.");
            }
            if (ModelState.IsValid)
            {
                var currentTime = DateTime.Now;
                footeritem.CreatedBy = User.Identity.Name;
                footeritem.Created = currentTime;
                footeritem.ModifiedBy = User.Identity.Name;
                footeritem.Modified = currentTime;
                footeritem.Index = 0;
                footeritem.IsPublished = true;
                footeritem.Section = _sectionRepo.GetAll().Single(e => e.Title == header);
                _footerItemRepo.Add(footeritem);
                _footerItemRepo.Save();
                return RedirectToAction("Index");  
            }
            ViewBag.Targets = TARGETS;
            ViewBag.Sections = SECTIONS;
            ViewBag.SectionTitle = header;
            return View(footeritem);
        }
        
        //
        // GET: /FooterItem/Edit/5
 
        public ActionResult Edit(int id)
        {
            FooterItem footeritem = _footerItemRepo.Get(id);
            ViewBag.Targets = TARGETS;
            ViewBag.Sections = SECTIONS;
            ViewBag.SectionTitle = footeritem.Section.Title;
            return View(footeritem);
        }

        //
        // POST: /FooterItem/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FooterItem footeritem, string header)
        {
            if (ModelState.IsValid)
            {
                FooterItem f = _footerItemRepo.Get(footeritem.Id);
                FooterSection s = _sectionRepo.GetAll().Single(e => e.Title == header);

                f.Modified = DateTime.Now;
                f.ModifiedBy = User.Identity.Name;
                f.Section = s;
                f.Target = footeritem.Target;
                f.Index = footeritem.Index;
                f.IsPublished = footeritem.IsPublished;
                f.Link = footeritem.Link;
                f.Title = footeritem.Title;

                _footerItemRepo.Save();
                return RedirectToAction("Index");
            }
            ViewBag.Targets = TARGETS;
            ViewBag.Sections = SECTIONS;
            ViewBag.SectionTitle = header;
            return View(footeritem);
        }

        //
        // GET: /FooterItem/Delete/5
 
        public ActionResult Delete(int id)
        {
            FooterItem footeritem = _footerItemRepo.Get(id);
            return View(footeritem);
        }

        //
        // POST: /FooterItem/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            FooterItem footeritem = _footerItemRepo.Get(id);
            _footerItemRepo.Delete(footeritem);
            _footerItemRepo.Save();
            return RedirectToAction("Index");
        }
    }
}