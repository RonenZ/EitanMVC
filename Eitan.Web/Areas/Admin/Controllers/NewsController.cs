using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eitan.Models;
using Eitan.Data;
using System.Configuration;
using System.Web.Helpers;
using Eitan.Web.Controllers;
using Eitan.Web.Models;

namespace Eitan.Web.Areas.Admin.Controllers
{
    public class NewsController : EitanBaseController
    {
        private static readonly int pageSize = int.Parse(ConfigurationManager.AppSettings["NewsPageSize"]);

        public NewsController(IEitanUow uow)
        {
            Uow = uow;
            ViewBag.NewsActive = "active";
        }
        //
        // GET: /News/

        public ViewResult Index()
        {
            ViewBag.ModelCount = Uow.NewsRepository.GetAll().Count();
            return View(Uow.NewsRepository.GetAllDesc().Take(pageSize));
        }

        //
        // GET: /News/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /News/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(News Entity, SEO SEOEntity, HttpPostedFileBase[] SEOfile)
        {
            if (ModelState.IsValid)
            {
                var image = WebImage.GetImageFromRequest("UploadedImage");

                if (image != null)
                    Entity.MainImage = Server.MapPath("/Images/News/").SaveImage(image);

                if (SEOEntity != null && SEOfile != null && SEOfile.Length > 0)
                {
                    if (SEOfile != null)
                        SEOEntity.ogImage = SEOfile.FBSaveImages(Server.MapPath("/Images/SEO/"), "News");

                }
                Entity.Date_Creation = DateTime.Now;
                Uow.NewsRepository.Add(Entity);

                Uow.Commit();

                return RedirectToAction("Index");
            }
            return View(Entity);
        }

        //
        // GET: /News/Edit/5

        public ActionResult Edit(int id)
        {
            var Entity = Uow.NewsRepository.GetByID(id);

            return View(Entity);
        }

        //
        // POST: /News/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(News EditEntity, int id, SEO POSTSEO, int SEOID = 0)
        {
            if (ModelState.IsValid)
            {
                var Entity = Uow.NewsRepository.GetByID(id);

                var image = WebImage.GetImageFromRequest("UploadedImage");
                UpdateModel(Entity);

                if (image != null)
                    Entity.MainImage = Server.MapPath("/Images/News/").SaveImage(image);

                //if (SEOID > 0)
                //{
                //    var SEOEntity = repo.GetSEOByID(SEOID);
                //    UpdateModel(SEOEntity);
                //}
                //else if (POSTSEO != null)
                //    Entity.SEO_ID = Uow.NewsRepository.InsertSEO(POSTSEO);

                //if (SEOfile != null)
                //    Entity.SEO.ogImage += SEOfile.FBSaveImages(Server.MapPath("/Images/SEO/"), "News");

                Uow.Commit();

                return RedirectToAction("Index");
            }

            return View(EditEntity);
        }


        public ActionResult Delete(int id)
        {
            News news = Uow.NewsRepository.GetByID(id);
            return View(news);
        }

        //
        // POST: /ProjectTypes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = Uow.NewsRepository.GetByID(id);

            Uow.NewsRepository.Delete(news);
            Uow.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Uow.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}