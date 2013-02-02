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

namespace Eitan.Web.Areas.Admin.Controllers
{
    public class ArtistsController : EitanBaseController
    {
        private static readonly int pageSize = int.Parse(ConfigurationManager.AppSettings["NewsPageSize"]);

        public ArtistsController(IEitanUow uow)
        {
            Uow = uow;
            ViewBag.ArtistsActive = "active";
        }
        //
        // GET: /News/

        public ViewResult Index()
        {
            ViewBag.ModelCount = Uow.ArtistRepository.GetAll().Count();
            return View(Uow.ArtistRepository.GetAllDesc().Take(pageSize));
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
        public ActionResult Create(Artist Entity)
        {
            if (ModelState.IsValid)
            {

                Entity.Date_Creation = DateTime.Now;
                Uow.ArtistRepository.Add(Entity);

                Uow.Commit();

                return RedirectToAction("Index");
            }
            return View(Entity);
        }

        //
        // GET: /News/Edit/5

        public ActionResult Edit(int id)
        {
            var Entity = Uow.ArtistRepository.GetByID(id);

            return View(Entity);
        }

        //
        // POST: /News/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Artist EditEntity, int id, SEO POSTSEO, int SEOID = 0)
        {
            if (ModelState.IsValid)
            {
                var Entity = Uow.ArtistRepository.GetByID(id);

                UpdateModel(Entity);

                Uow.Commit();

                return RedirectToAction("Index");
            }

            return View(EditEntity);
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