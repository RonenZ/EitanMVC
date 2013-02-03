using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eitan.Models;
using Eitan.Data;
using System.Web.Helpers;
using Eitan.Web.Models;

namespace Eitan.Web.Areas.Admin.Controllers
{   
    public class GenresController : Controller
    {
        private EitanDbContext context = new EitanDbContext();

        public GenresController()
        {
            ViewBag.GenresActive = "active";
        }

        //
        // GET: /Genres/

        public ViewResult Index()
        {
            return View(context.Genres.Where(w => w.isDeleted == false).ToList());
        }

        //
        // GET: /Genres/Details/5

        public ViewResult Details(int id)
        {
            Genre genre = context.Genres.Single(x => x.ID == id);
            return View(genre);
        }

        //
        // GET: /Genres/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Genres/Create

        [HttpPost]
        public ActionResult Create(Genre genre)
        {
            if (ModelState.IsValid)
            {

                genre.Date_Creation = DateTime.Now;

                context.Genres.Add(genre);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(genre);
        }
        
        //
        // GET: /Genres/Edit/5
 
        public ActionResult Edit(int id)
        {
            Genre genre = context.Genres.Single(x => x.ID == id);
            return View(genre);
        }

        //
        // POST: /Genres/Edit/5

        [HttpPost]
        public ActionResult Edit(Genre genre)
        {
            if (ModelState.IsValid)
            {
                context.Entry(genre).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(genre);
        }

        //
        // GET: /Genres/Delete/5
 
        public ActionResult Delete(int id)
        {
            Genre genre = context.Genres.Single(x => x.ID == id);
            return View(genre);
        }

        //
        // POST: /Clients/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Genre genre = context.Genres.Single(x => x.ID == id);
            //context.Genres.Remove(genre);
            genre.isDeleted = true;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}