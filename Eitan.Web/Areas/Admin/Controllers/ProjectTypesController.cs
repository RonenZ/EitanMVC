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

namespace Eitan.Web.Areas.Admin.Controllers
{   
    public class ProjectTypeController : Controller
    {
        private EitanDbContext context = new EitanDbContext();

        public ProjectTypeController()
        {
            ViewBag.ClientsActive = "active";
        }

        //
        // GET: /ProjectTypes/

        public ViewResult Index()
        {
            return View(context.ProjectTypes.ToList());
        }

        //
        // GET: /ProjectTypes/Details/5

        public ViewResult Details(int id)
        {
            ProjectType ptype = context.ProjectTypes.Single(x => x.ID == id);
            return View(ptype);
        }

        //
        // GET: /Clients/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /ProjectTypes/Create

        [HttpPost]
        public ActionResult Create(ProjectType ptype)
        {
            if (ModelState.IsValid)
            {
                ptype.Date_Creation = DateTime.Now;

                context.ProjectTypes.Add(ptype);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(ptype);
        }
        
        //
        // GET: /ProjectTypes/Edit/5
 
        public ActionResult Edit(int id)
        {
            ProjectType ptype = context.ProjectTypes.Single(x => x.ID == id);
            return View(ptype);
        }

        //
        // POST: /ProjectTypes/Edit/5

        [HttpPost]
        public ActionResult Edit(ProjectType ptype)
        {
            if (ModelState.IsValid)
            {
                context.Entry(ptype).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ptype);
        }

        //
        // GET: /ProjectTypes/Delete/5
 
        public ActionResult Delete(int id)
        {
            ProjectType ptype = context.ProjectTypes.Single(x => x.ID == id);
            return View(ptype);
        }

        //
        // POST: /ProjectTypes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectType ptype = context.ProjectTypes.Single(x => x.ID == id);
            context.ProjectTypes.Remove(ptype);
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