using Eitan.Data;
using Eitan.Web.Models;
using System;
using System.Collections.Generic;
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
            var HomeModel = new HomePageViewModel();

            var Homepage = Uow.PagesRepository.GetByType(1000);
            ViewBag.SEO = Homepage.SEO;
            ViewBag.Images = Homepage.Images;

            HomeModel["Project"].Item = Uow.ProjectRepository
                                              .GetAllDesc()
                                              .ToViewModelWithImage();

            HomeModel["News"].Item = Uow.NewsRepository
                                              .GetAllDesc()
                                              .ToViewModelWithImage();

            HomeModel["Discography"].Item = Uow.ReleaseRepository
                                                        .GetAllDescWithIncludes()
                                                        .ReleaseToViewModelWithImage();

            HomeModel["Movie"].Item = Uow.ProjectRepository
                                                  .GetAllDesc()
                                                  .Where(w => w.TypeID == 1)
                                                  .ToViewModelWithImage();

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

            return View(StudioPage);
        }

        public ActionResult Contact()
        {
            var ContactPage = Uow.PagesRepository.GetByType(15);

            ViewBag.SEO = ContactPage.SEO;

            ViewBag.PageTitle = ContactPage.Title;
            ViewBag.PageContent = ContactPage.Content;
            //Left Side Images - of type 33
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
