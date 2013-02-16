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

        public ViewResult Index()
        {
            return View(Uow.PagesRepository
                            .GetAllDesc()
                            .Take(pageSize));
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
            var Entity = Uow.PagesRepository.GetByID(id, p => p.SEO);

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

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                Uow.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}