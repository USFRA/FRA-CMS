using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using RadCms.Data;
using RadCms.Entities;
using RadCms.Mvc;

namespace RadCms.Web.Areas.Footer.Controllers
{
    public class FooterSectionCmsController : CmsControllerBase
    {
        private IRepository<FooterSection> _sectionRepo;
        public FooterSectionCmsController(IRepository<FooterSection> sectionRepo)
        {
            _sectionRepo = sectionRepo;
        }

        public List<string> SECTIONS
        {
            get
            {
                return _sectionRepo.GetAll().Select(e => e.Title).ToList();
            }
        }

        //
        // GET: /FooterSection/

        public ViewResult Index()
        {
            return View(_sectionRepo.GetAll().ToList());
        }

        //
        // GET: /FooterSection/Details/5

        public ViewResult Details(int id)
        {
            FooterSection footersection = _sectionRepo.Get(id);
            return View(footersection);
        }

        //
        // GET: /FooterSection/Create

        public ActionResult Create()
        {
            ViewBag.Sections = SECTIONS;
            return View();
        } 

        //
        // POST: /FooterSection/Create

        [HttpPost]
        public ActionResult Create(FooterSection footersection)
        {
            if (ModelState.IsValid)
            {
                _sectionRepo.Add(footersection);
                _sectionRepo.Save();
                return RedirectToAction("Index");  
            }
            ViewBag.Sections = SECTIONS;
            return View(footersection);
        }
        
        //
        // GET: /FooterSection/Edit/5
 
        public ActionResult Edit(int id)
        {
            FooterSection footersection = _sectionRepo.Get(id);
            ViewBag.Sections = SECTIONS;
            return View(footersection);
        }

        //
        // POST: /FooterSection/Edit/5

        [HttpPost]
        public ActionResult Edit(FooterSection footersection)
        {
            if (ModelState.IsValid)
            {
                _sectionRepo.Update(footersection);
                _sectionRepo.Save();
                return RedirectToAction("Index");
            }
            ViewBag.Sections = SECTIONS;
            return View(footersection);
        }

        //
        // GET: /FooterSection/Delete/5
 
        public ActionResult Delete(int id)
        {
            FooterSection footersection = _sectionRepo.Get(id);
            return View(footersection);
        }

        //
        // POST: /FooterSection/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            FooterSection footersection = _sectionRepo.Get(id);
            _sectionRepo.Delete(footersection);
            _sectionRepo.Save();
            return RedirectToAction("Index");
        }
    }
}