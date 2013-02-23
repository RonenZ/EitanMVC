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
using PagedList;

namespace Eitan.Web.Areas.Admin.Controllers
{   
    public class PagesController : EitanBaseController
    {
        private static readonly int pageSize = int.Parse(ConfigurationManager.AppSettings["projectPageSize"]);

        public PagesController(IEitanUow uow)
        {
            Uow = uow;
            ViewBag.PagesActive = "active";
        }

        //
        // GET: /Projects/

        public ViewResult Index(int pagenum = 1, string searchbar = "")
        {
            if (string.IsNullOrEmpty(searchbar.Trim()))
            {
                return View(Uow.PagesRepository.GetAllDesc()
                                              .ToPagedList(pagenum, pageSize));
            }

            return View(Uow.PagesRepository.SearchStringInTitle(searchbar)
                                              .ToPagedList(pagenum, pageSize));
        }


        //
        // GET: /Projects/Details/5

        public ViewResult Details(int id)
        {
            Page page = Uow.PagesRepository.GetByID(id, s => s.SEO);

            return View(page);
        }

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /News/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Page Entity, SEO SEOEntity, HttpPostedFileBase[] SEOfile)
        {
            if (ModelState.IsValid)
            {
                UpsertSEO(Entity, 0, SEOEntity, SEOfile, "Pages");

                Entity.Date_Creation = DateTime.Now;
                Uow.PagesRepository.Add(Entity);

                Uow.Commit();

                return RedirectToAction("Index");
            }

            return View(Entity);
        }

        //
        // GET: /News/Edit/5

        public ActionResult Edit(int id)
        {
            var Entity = Uow.PagesRepository.GetByID(id, p => p.SEO, p => p.Images);

            return View(Entity);
        }

        //
        // POST: /News/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Page EditEntity, int id, SEO POSTSEO, HttpPostedFileBase[] SEOfile, int SEOID = 0)
        {
            if (ModelState.IsValid)
            {
                var Entity = Uow.PagesRepository.GetByID(id);
                UpdateModel(Entity);

                UpsertSEO(Entity, SEOID, POSTSEO, SEOfile, "Pages");

                Uow.Commit();

                return RedirectToAction("Index");
            }

            return View(EditEntity);
        }

        #region Pictures

        public ActionResult CreatePicture(int PageId)
        {
            ViewBag.PageId = PageId;
            return View();
        }

        //
        // POST: /Clients/Create

        [HttpPost]
        public ActionResult CreatePicture(int HdnPageId, Picture pic)
        {
            Page page = Uow.PagesRepository.GetByID(HdnPageId, s => s.Images);
            if (ModelState.IsValid && page != null)
            {
                var image = WebImage.GetImageFromRequest("UploadedImage");

                if (image != null)
                    pic.Source = Server.MapPath("/Images/Pages/").SaveImage(image);

                pic.Date_Creation = DateTime.Now;

                page.Images.Add(pic);

                Uow.Commit();

                return RedirectToAction("Edit", new { id = HdnPageId });
            }
            ViewBag.PageId = HdnPageId;
            ViewBag.Genres = Uow.ReleaseRepository.GetAllGenres().ToList();

            return View(pic);
        }

        public ActionResult EditPicture(int id)
        {
            var Entity = Uow.PagesRepository.GetPictureByID(id);
            if (Entity != null)
                ViewBag.PageId = Entity.PageId;
            return View(Entity);
        }


        [HttpPost]
        public ActionResult EditPicture(int HdnPageID, Picture pic, HttpPostedFileBase UploadedFile)
        {
            if (ModelState.IsValid)
            {
                Picture Entity = Uow.PagesRepository.GetPictureByID(pic.ID);
                UpdateModel(Entity);

                var image = WebImage.GetImageFromRequest("UploadedImage");

                if (image != null)
                    Entity.Source = Server.MapPath("/Images/Pages/").SaveImage(image);

                Uow.Commit();
                return RedirectToAction("Edit", new { id = HdnPageID });
            }

            ViewBag.PageId = HdnPageID;
            return View(pic);
        }

        public ActionResult DeletePicture(int id)
        {
            var Entity = Uow.PagesRepository.GetPictureByID(id);
            if (Entity != null)
                ViewBag.PageId = Entity.PageId;
            return View(Entity);
        }

        [HttpPost]
        public ActionResult DeletePicture(int id, int hdnPageID)
        {
            Uow.PagesRepository.DeletePicture(id);
            Uow.Commit();

            return RedirectToAction("Edit", new { id = hdnPageID });
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                Uow.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}