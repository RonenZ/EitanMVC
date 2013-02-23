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
using Eitan.Web.Models;

namespace Eitan.Web.Controllers
{   
    public class ProjectsController : EitanBaseController
    {
        private static readonly int pageSize = int.Parse(ConfigurationManager.AppSettings["projectPageSize"]);

       public ProjectsController(IEitanUow uow)
        {
            Uow = uow;
            ViewBag.ActiveProjects = "active";
        }
        //
        // GET: /Projects/

        public ViewResult Index(int page = 1)
        {
            int itemsleft = Uow.ProjectRepository.GetAll().Count() - (page * pageSize);
            ViewBag.isMoreItems = itemsleft > 0 ? true : false;

            var entities = Uow.ProjectRepository.GetAllDesc()
                                                .Skip(--page * pageSize)
                                                .Take(pageSize)
                                                .ProjectsToViewModelsWithImage();

            ViewBag.ModelCount = entities.Count();

            return View(entities);
        }


        //
        // GET: /Projects/Details/5

        public ViewResult Details(int id)
        {
            Project project = Uow.ProjectRepository.GetByID(id, p => p.Client, p => p.Type, r => r.SEO);

            ViewBag.SEO = project.SEO;

            return View(project);
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