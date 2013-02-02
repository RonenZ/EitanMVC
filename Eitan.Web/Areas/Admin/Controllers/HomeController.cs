using Eitan.Data;
using Eitan.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eitan.Web.Areas.Admin.Controllers
{
    public class HomeController : EitanBaseController
    {
        public HomeController(IEitanUow uow)
        {
            Uow = uow;
            ViewBag.HomeActive = "active";
            ViewBag.ProjectsCount = Uow.ProjectRepository.GetAll().Count();
            var ProjEntity = Uow.ProjectRepository.GetAllDesc().FirstOrDefault();
            if (ProjEntity != null)
            {
                ViewBag.ProjectID = ProjEntity.ID;
                ViewBag.ProjectTitle = ProjEntity.Title;
            }
            ViewBag.NewsCount = Uow.NewsRepository.GetAll().Count();
            var NewsEntity = Uow.NewsRepository.GetAllDesc().FirstOrDefault();
            if (NewsEntity != null)
            {
                ViewBag.NewsID = NewsEntity.ID;
                ViewBag.NewsTitle = NewsEntity.Title;
            }
            ViewBag.ReleasesCount = Uow.ReleaseRepository.GetAll().Count();
            var RelEntity = Uow.ReleaseRepository.GetAllDesc().FirstOrDefault();
            if (RelEntity != null)
            {
                ViewBag.ReleaseID = RelEntity.ID;
                ViewBag.ReleaseTitle = RelEntity.Title;
            }
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
