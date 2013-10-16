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
using Eitan.Web.Areas.Admin.Models;

namespace Eitan.Web.Areas.Admin.Controllers
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

        public ViewResult Index(int pagenum = 1, string searchbar = "")
        {
            if (string.IsNullOrEmpty(searchbar.Trim()))
            {
                return View(Uow.ProjectRepository.GetAllDesc()
                                              .ToPagedList(pagenum, pageSize));
            }

            return View(Uow.ProjectRepository.SearchStringInTitle(searchbar)
                                              .ToPagedList(pagenum, pageSize));
        }

        [HttpGet]
        public ViewResult Priority()
        {
            return View(Uow.ProjectRepository.GetAll().OrderBy(o => o.Priority));
        }

        [HttpPost]
        public JsonResult PrioritySubmit(IEnumerable<ProjectPriority> priorities)
        {
            Dictionary<string, string> response = new Dictionary<string,string>();
            var totalProjects = Uow.ProjectRepository.GetAll();
            
            int prio = int.MaxValue;

            try
            {
                foreach (var proj in totalProjects)
                {
                    var currentPrio = priorities.Where(w => w.ID == proj.ID).SingleOrDefault();
                    if (currentPrio == null)
                        continue;

                    proj.Priority = currentPrio.Priority;
                }

                Uow.Commit();
            }
            catch (Exception ex)
            {
                response.Add("responseCode", "500");
                response.Add("error", ex.Message);
                response.Add("message", "somethingwentwrong");
                return Json(response);
            }

            response.Add("responseCode", "200");
            response.Add("message", "prioritiesupdatedsuccessfuly");
            return Json(response);
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
                InsertImage(Entity, "UploadedImage", "Projects");
                UpsertSEO(Entity, SEOEntity.SEO_ID, SEOEntity, SEOfile, "Projects");

                //Entity.Date_Creation = DateTime.Now;
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
            var Entity = Uow.ProjectRepository.GetByID(id, s => s.SEO);

            ViewBagProjects();

            return View(Entity);
        }

        //
        // POST: /News/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Project EditEntity, int id, SEO POSTSEO, HttpPostedFileBase[] SEOfile, int SEOID = 0)
        {
            if (ModelState.IsValid)
            {
                var Entity = Uow.ProjectRepository.GetByID(id);
                UpdateModel(Entity);

                InsertImage(Entity, "UploadedImage", "Projects");
                UpsertSEO(Entity, POSTSEO.SEO_ID, POSTSEO, SEOfile, "Projects");

                Uow.Commit();

                return RedirectToAction("Index");
            }

            return View(EditEntity);
        }

        public ActionResult Delete(int id)
        {
            Project pro = Uow.ProjectRepository.GetByID(id);
            return View(pro);
        }

        //
        // POST: /ProjectTypes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Project pro = Uow.ProjectRepository.GetByID(id);

            Uow.ProjectRepository.Delete(pro);
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