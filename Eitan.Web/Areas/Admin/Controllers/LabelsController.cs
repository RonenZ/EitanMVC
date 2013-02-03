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
    public class LabelsController : Controller
    {
        private EitanDbContext context = new EitanDbContext();

        public LabelsController()
        {
            ViewBag.LabelsActive = "active";
        }

        //
        // GET: /Clients/

        public ViewResult Index()
        {
            return View(context.Labels.Where(w => w.isDeleted == false).ToList());
        }

        //
        // GET: /Clients/Details/5

        public ViewResult Details(int id)
        {
            Label label = context.Labels.Single(x => x.ID == id);
            return View(label);
        }

        //
        // GET: /Clients/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Clients/Create

        [HttpPost]
        public ActionResult Create(Label label)
        {
            if (ModelState.IsValid)
            {
                label.Date_Creation = DateTime.Now;

                context.Labels.Add(label);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(label);
        }
        
        //
        // GET: /Clients/Edit/5
 
        public ActionResult Edit(int id)
        {
            Label label = context.Labels.Single(x => x.ID == id);
            return View(label);
        }

        //
        // POST: /Clients/Edit/5

        [HttpPost]
        public ActionResult Edit(Label label)
        {
            if (ModelState.IsValid)
            {
                context.Entry(label).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(label);
        }

        //
        // GET: /Clients/Delete/5
 
        public ActionResult Delete(int id)
        {
            Label label = context.Labels.Single(x => x.ID == id);
            return View(label);
        }

        //
        // POST: /Clients/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Label label = context.Labels.Single(x => x.ID == id);
            label.isDeleted = true;
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