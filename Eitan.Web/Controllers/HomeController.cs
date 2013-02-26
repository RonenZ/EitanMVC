using Eitan.Data;
using Eitan.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eitan.Web.Controllers
{
    public class HomeController : EitanBaseController
    {
        public HomeController(IEitanUow uow)
        {
            Uow = uow;
        }

        public ActionResult Index()
        {
            ViewBag.ActiveHome = "active";
            var HomeModel = new HomePageViewModel();

            var Homepage = Uow.PagesRepository.GetByType(1000);

            //Image Paths:
            string newsImgPath = ConfigurationManager.AppSettings["ImageFolder_News"];
            string projectsImgPath = ConfigurationManager.AppSettings["ImageFolder_Projects"];
            string releasesImgPath = ConfigurationManager.AppSettings["ImageFolder_Releases"];    

            //var results = Uow.PagesRepository.GetHomePageTopItems(4);
            ViewBag.SEO = Homepage.SEO;
            ViewBag.Images = Homepage.Images;

            var Results = Uow.ProjectRepository.GetAllDesc().ToViewModelsWithImage_Queryable(_controller: "Projects", _imagePath: projectsImgPath);

            Results = Results.Union(Uow.NewsRepository.GetAllDesc().ToViewModelsWithImage_Queryable(_controller: "News", _imagePath: newsImgPath));
            var ReleasesResults = Uow.ReleaseRepository.GetAllDesc("Label")
                                                        .ReleasesToViewModelsWithImage_Queryable("Discography", _controller: "Releases", _imagePath: releasesImgPath).Take(4).ToList();

            HomeModel.Items = Results.OrderByDescending(o => o.CreationDate).Take(4).ToList().Union(ReleasesResults)
                                     .OrderByDescending(o => o.CreationDate).Take(4);

            return View(HomeModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Studio()
        {
            var StudioPage = Uow.PagesRepository.GetByType(11);

            ViewBag.SEO = StudioPage.SEO;

            //Top Slider Images - of type 22
            ViewBag.SliderImages = StudioPage.Images.Where(w => w.PictureType == 22).ToList();
            //Left Side Images - of type 33
            ViewBag.LeftImages = StudioPage.Images.Where(w => w.PictureType == 33).ToList();
            ViewBag.ActiveStudio = "active";
            return View(StudioPage);
        }

        public ActionResult Contact()
        {
            var ContactPage = Uow.PagesRepository.GetByType(15);

            ViewBag.SEO = ContactPage.SEO;

            ViewBag.PageTitle = ContactPage.Title;
            ViewBag.PageContent = ContactPage.Content;
            ViewBag.ActiveContact = "active";
            ViewBag.LeftImages = ContactPage.Images.ToList();

            return View();
        }

        [HttpPost]
        public JsonResult Contact(ContactModel Contact)
        {
            var jsonResult = new Dictionary<string, int>();

            jsonResult["code"] = 401;

            if (ModelState.IsValid)
            {
                var result = StaticCode.SendContact(Contact);

                if (result)
                    jsonResult["code"] = 100;
                else
                    jsonResult["code"] = 500;
            }
            else
                jsonResult["code"] = 404;

            return Json(jsonResult);
        }
    }
}
