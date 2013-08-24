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
using Eitan.Admin.Models;

namespace Eitan.Admin.Controllers
{   
    public class ProjectsController : EitanBaseController
    {
        private static readonly int pageSize = int.Parse(ConfigurationManager.AppSettings["projectPageSize"]);

       public ProjectsController(IEitanUow uow)
        {
            Uow = uow;
            ViewBag.ProjectsActive = "active";
        }

       private void ViewBagProjects(int id = 0)
       {
           ViewBag.ProjectTypes = Uow.ProjectRepository
                                     .GetAllProjectTypes()
                                     .ToList()
                                     .Select(s => new SelectListItem() { Text = s.Title, Value = s.ID.ToString()});

           ViewBag.Clients = Uow.ProjectRepository.GetAllClients()
                                                  .ToList()
                                                  .Select(s => new SelectListItem() { Text = s.Title, Value = s.ID.ToString()});
       }

        //
        // GET: /Projects/

        public ViewResult Index()
        {
			ViewBag.ModelCount = Uow.ProjectRepository
                                    .GetAll()
                                    .Count();

            return View(Uow.ProjectRepository
                            .GetAllDesc()
                            .Take(pageSize));
        }


        //
        // GET: /Projects/Details/5

        public ViewResult Details(int id)
        {
            Project project = Uow.ProjectRepository.GetByID(id);

            return View(project);
        }

        public ActionResult Create()
        {
            ViewBagProjects();
            return View();
        }

        //
        // POST: /News/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Project Entity, SEO SEOEntity, HttpPostedFileBase[] SEOfile)
        {
            if (ModelState.IsValid)
            {
                var image = WebImage.GetImageFromRequest("UploadedImage");

                if (image != null)
                    Entity.MainImage = Server.MapPath("/Images/Projects/").SaveImage(image);

                if (SEOEntity != null && SEOfile != null && SEOfile.Length > 0)
                {
                    if (SEOfile != null)
                        SEOEntity.ogImage = SEOfile.FBSaveImages(Server.MapPath("/Images/SEO/"), "News");
                }
                Entity.Date_Creation = DateTime.Now;
                Uow.ProjectRepository.Add(Entity);

                Uow.Commit();

                return RedirectToAction("Index");
            }

            return View(Entity);
        }

        //
        // GET: /News/Edit/5

        public ActionResult Edit(int id)
        {
            var Entity = Uow.ProjectRepository.GetByID(id);

            ViewBagProjects();

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
                var Entity = Uow.ProjectRepository.GetByID(id);

                var image = WebImage.GetImageFromRequest("UploadedImage");
                UpdateModel(Entity);

                if (image != null)
                    Entity.MainImage = Server.MapPath("/Images/Projects/").SaveImage(image);

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