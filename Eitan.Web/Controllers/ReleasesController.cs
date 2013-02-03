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
using Eitan.Web.Models;

namespace Eitan.Web.Controllers
{   
    public class ReleasesController : EitanBaseController
    {
        private static readonly int pageSize = int.Parse(ConfigurationManager.AppSettings["ReleasePageSize"]);

        public ReleasesController(IEitanUow uow)
        {
            Uow = uow;
        }
        //
        // GET: /News/

        public ViewResult Index(int page = 1)
        {
            int itemsleft = Uow.ReleaseRepository.GetAll().Count() - (page * pageSize);
            ViewBag.isMoreItems = itemsleft > 0 ? true : false;

            var entities = Uow.ReleaseRepository.GetAllDesc("Label")
                                                .Skip(--page * pageSize)
                                                .Take(pageSize)
                                                .ReleasesToViewModelsWithImage();

            return View(entities);
        }

        //
        // GET: /News/Details/5

        public ViewResult Details(int id)
        {
            Release release = Uow.ReleaseRepository.GetByID(id, r => r.Genre, r => r.Label, r => r.Songs);
            return View(release);
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