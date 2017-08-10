using System;
using System.Linq;
using System.Web.Mvc;

namespace RadCms.Web.Areas.Carousel.Controllers
{
    using RadCms.Data;
    using RadCms.Entities;
    using Mvc;

    public class ItemCmsController : CmsControllerBase
    {
        private IRepository<Carousel> _carouselRepo;

        public ItemCmsController(IRepository<Carousel> carouselRepo)
        {
            _carouselRepo = carouselRepo;
        }

        //
        // GET: /Carousel/

        public ViewResult Index()
        {
            return View(_carouselRepo.GetAll().ToList());
        }

        //
        // GET: /Carousel/Details/5

        public ViewResult Details(int id)
        {
            Carousel carousel = _carouselRepo.Get(id);
            return View(carousel);
        }

        [HttpPost]
        public ActionResult Do(string id, string act)
        {
            int sid = Convert.ToInt32(id);
            Carousel slide = _carouselRepo.Get(sid);

            switch (act)
            {
                case "delete":
                    _carouselRepo.Delete(slide);
                    _carouselRepo.Save();

                    break;

                case "edit":

                    //db.Entry(slide).State = EntityState.Modified;
                    //db.SaveChanges();

                    return RedirectToAction("Edit", new { id = id });

                case "cancel":

                    break;
            }

            return RedirectToAction("Index");
        }

        //
        // GET: /Carousel/Create

        public ActionResult Create()
        {
            return View(new Carousel());
        } 

        //
        // POST: /Carousel/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Carousel carousel)
        {
            if (ModelState.IsValid)
            {
                _carouselRepo.Add(carousel);
                _carouselRepo.Save();
                return RedirectToAction("Index");  
            }

            return View(carousel);
        }
        
        //
        // GET: /Carousel/Edit/5
 
        public ActionResult Edit(int id)
        {
            Carousel carousel = _carouselRepo.Get(id);
            return View(carousel);
        }

        //
        // POST: /Carousel/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Carousel carousel)
        {
            if (ModelState.IsValid)
            {
                _carouselRepo.Update(carousel);
                _carouselRepo.Save();
                return RedirectToAction("Index");
            }
            return View(carousel);
        }

        //
        // GET: /Carousel/Delete/5
 
        public ActionResult Delete(int id)
        {
            Carousel carousel = _carouselRepo.Get(id);
            return View(carousel);
        }

        //
        // POST: /Carousel/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Carousel carousel = _carouselRepo.Get(id);
            _carouselRepo.Delete(carousel);
            _carouselRepo.Save();
            return RedirectToAction("Index");
        }
    }
}