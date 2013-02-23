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
using PagedList;

namespace Eitan.Web.Controllers
{   
    public class NewsController : EitanBaseController
    {
       private static readonly int pageSize = int.Parse(ConfigurationManager.AppSettings["NewsPageSize"]);

       public NewsController(IEitanUow uow)
        {
            Uow = uow;
            ViewBag.ActiveNews = "active";
        }
        //
        // GET: /News/

        public ViewResult Index(int page = 1)
        {
            int itemsleft = Uow.NewsRepository.GetAll().Count() - (page * pageSize);
            ViewBag.isMoreItems = itemsleft > 0 ? true : false;

            return View(Uow.NewsRepository.GetAllDesc()
                                          .Skip(--page * pageSize)
                                          .Take(pageSize)
                                          .ToViewModelsImageDetail());
        }

        //
        // GET: /News/Details/5

        public ViewResult Details(int id)
        {
            var news = Uow.NewsRepository.GetByID(id, r => r.SEO);

            ViewBag.SEO = news.SEO;
            return View(news);
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