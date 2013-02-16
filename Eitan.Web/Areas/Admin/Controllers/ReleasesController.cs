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
using Eitan.Web.Controllers;
using Eitan.Web.Models;
using PagedList;

namespace Eitan.Web.Areas.Admin.Controllers
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

        public ViewResult Index(int pagenum = 1, string searchbar = "")
        {
            if (string.IsNullOrEmpty(searchbar.Trim()))
            {
                return View(Uow.ReleaseRepository.GetAllDesc()
                                              .ToPagedList(pagenum, pageSize));
            }

            return View(Uow.ReleaseRepository.SearchStringInTitle(searchbar)
                                              .ToPagedList(pagenum, pageSize));
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
            ViewBagReleases();
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
                Entity.Date_Creation = DateTime.Now;

                ReleasesUpsert(Entity, SEOEntity, SEOfile);

                Uow.ReleaseRepository.Add(Entity);

                Uow.Commit();

                return RedirectToAction("Index");
            }

            ViewBagReleases();

            return View(Entity);
        }

        //
        // GET: /News/Edit/5

        public ActionResult Edit(int id)
        {
            ViewBagReleases();

            ViewBag.RelID = id;
            var Entity = Uow.ReleaseRepository.GetByID(id, s => s.Songs, s => s.SEO);

            return View(Entity);
        }

        private void ViewBagReleases()
        {
            ViewBag.Labels = Uow.ReleaseRepository.GetAllLabels().ToList();
            ViewBag.Genres = Uow.ReleaseRepository.GetAllGenres().ToList();
            ViewBag.ReleaseTypes = StaticCode.ReleaseTypes;

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

                UpdateModel(Entity);

                ReleasesUpsert(Entity, POSTSEO, null);

                Uow.Commit();

                return RedirectToAction("Index");
            }
            ViewBag.RelID = id;
            ViewBagReleases();

            return View(EditEntity);
        }

        #region Songs
        //
        // GET: /Clients/Create

        public ActionResult CreateSong(int RelId)
        {
            ViewBag.Genres = Uow.ReleaseRepository.GetAllGenres().ToList();
            ViewBag.RelId = RelId;
            return View("CreateSong");
        }

        //
        // POST: /Clients/Create

        [HttpPost]
        public ActionResult CreateSong(int HdnRelID, Song song, HttpPostedFileBase UploadedFile)
        {
            Release rel = Uow.ReleaseRepository.GetByID(HdnRelID);
            if (ModelState.IsValid && rel != null)
            {
                if (UploadedFile != null)
                    song.FilePath = Server.MapPath("/Files/Releases/").SaveFile(UploadedFile);
                
                song.Date_Creation = DateTime.Now;

                Uow.SongRepository.Add(song);

                rel.Songs.Add(song);

                Uow.Commit();
                return RedirectToAction("Edit", new { id = HdnRelID });
            }
            ViewBag.RelId = HdnRelID;
            ViewBag.Genres = Uow.ReleaseRepository.GetAllGenres().ToList();

            return View(song);
        }

        //
        // GET: /Clients/Edit/5

        public ActionResult EditSong(int id, int RelId)
        {
            ViewBag.RelId = RelId;
            ViewBag.Genres = Uow.ReleaseRepository.GetAllGenres().ToList();
            Song song = Uow.SongRepository.GetByID(id);

            return View(song);
        }

        //
        // POST: /Clients/Edit/5

        [HttpPost]
        public ActionResult EditSong(int HdnRelID, Song song, HttpPostedFileBase UploadedFile)
        {
            if (ModelState.IsValid)
            {
                Song Entity = Uow.SongRepository.GetByID(song.ID);
                UpdateModel(Entity);

                if (UploadedFile != null)
                    Entity.FilePath = Server.MapPath("/Files/Releases/").SaveFile(UploadedFile);

                Uow.Commit();
                return RedirectToAction("Edit", new { id = HdnRelID });
            }

            ViewBag.RelId = HdnRelID;
            return View(song);
        }

        public ActionResult DeleteSong(int id, int RelId)
        {
            ViewBag.RelId = RelId;
            Song song = Uow.SongRepository.GetByID(id);
            return View(song);
        }

        //
        // POST: /Clients/Delete/5

        [HttpPost, ActionName("DeleteSong")]
        public ActionResult DeleteSong(int id, int HdnRelID, bool isToDelete = true)
        {
            Song song = Uow.SongRepository.GetByID(id);

            Uow.SongRepository.Delete(song);
            Uow.Commit();

            return RedirectToAction("Edit", new { id = HdnRelID });
        }

        #endregion

        private void ReleasesUpsert(Release Entity, SEO SEOEntity, HttpPostedFileBase[] SEOfile)
        {
            var mainImage = WebImage.GetImageFromRequest("UploadedMainImage");

            if (mainImage != null)
                Entity.MainImage = Server.MapPath("/Images/Releases/").SaveImage(mainImage, true, 315, 315);

            InsertImage(Entity, "UploadedRectImage", "Releases");

            UpsertSEO(Entity, SEOEntity.SEO_ID, SEOEntity, SEOfile, "Releases");
        }


        public ActionResult Delete(int id)
        {
            Release rel = Uow.ReleaseRepository.GetByID(id);
            return View(rel);
        }

        //
        // POST: /ProjectTypes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Release rel = Uow.ReleaseRepository.GetByID(id);

            Uow.ReleaseRepository.Delete(rel);
            Uow.Commit();
            return RedirectToAction("Index");
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