using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Eitan.Models;
using Eitan.Data;
using System.Web.Helpers;
using Eitan.Admin.Models;

namespace Eitan.Admin.Controllers
{   
    public class ReleasesController : EitanBaseController
    {
        private static readonly int pageSize = int.Parse(ConfigurationManager.AppSettings["ReleasePageSize"]);

        public ReleasesController(IEitanUow uow)
        {
            Uow = uow;
            ViewBag.ReleasesActive = "active";
        }
        //
        // GET: /News/

        public ViewResult Index()
        {
			ViewBag.ModelCount = Uow.ReleaseRepository.GetAll().Count();
            return View(Uow.ReleaseRepository.GetAllDesc().Take(pageSize));
        }

        //
        // GET: /News/Details/5

        public ViewResult Details(int id)
        {
            Release release = Uow.ReleaseRepository.GetByID(id, s => s.Songs);
            return View(release);
        }


        public ActionResult Create()
        {
            ViewBag.Labels = Uow.ReleaseRepository.GetAllLabels().ToList();
            return View();
        }

        //
        // POST: /News/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Release Entity, SEO SEOEntity, HttpPostedFileBase[] SEOfile)
        {
            if (ModelState.IsValid)
            {
                var mainImage = WebImage.GetImageFromRequest("UploadedMainImage");
                var rectImage = WebImage.GetImageFromRequest("UploadedRectImage");

                if (mainImage != null)
                    Entity.MainImage = Server.MapPath("/Images/Releases/").SaveImage(mainImage, true, 315, 315);
                if (rectImage != null)
                    Entity.RectImage = Server.MapPath("/Images/Releases/").SaveImage(rectImage, false);

                if (SEOEntity != null && SEOfile != null && SEOfile.Length > 0)
                {
                    if (SEOfile != null)
                        SEOEntity.ogImage = SEOfile.FBSaveImages(Server.MapPath("/Images/SEO/"), "News");
                }
                Entity.Date_Creation = DateTime.Now;
                Uow.ReleaseRepository.Add(Entity);


                Uow.Commit();

                return RedirectToAction("Index");
            }

            ViewBag.Labels = Uow.ReleaseRepository.GetAllLabels().ToList();

            return View(Entity);
        }

        //
        // GET: /News/Edit/5

        public ActionResult Edit(int id)
        {
            ViewBag.Labels = Uow.ReleaseRepository.GetAllLabels().ToList();

            var Entity = Uow.ReleaseRepository.GetByID(id);

            return View(Entity);
        }

        //
        // POST: /News/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Release EditEntity, int id, SEO POSTSEO, int SEOID = 0)
        {
            if (ModelState.IsValid)
            {
                var Entity = Uow.ReleaseRepository.GetByID(id);

                var mainImage = WebImage.GetImageFromRequest("UploadedMainImage");
                var rectImage = WebImage.GetImageFromRequest("UploadedRectImage");
                UpdateModel(Entity);

                if (mainImage != null)
                    Entity.MainImage = Server.MapPath("/Images/Releases/").SaveImage(mainImage);
                if (rectImage != null)
                    Entity.RectImage = Server.MapPath("/Images/Releases/").SaveImage(rectImage);

                Uow.Commit();

                return RedirectToAction("Index");
            }

            ViewBag.Labels = Uow.ReleaseRepository.GetAllLabels().ToList();

            return View(EditEntity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                Uow.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}