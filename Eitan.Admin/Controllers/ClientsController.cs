using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eitan.Models;
using Eitan.Admin.Models;
using Eitan.Data;
using System.Web.Helpers;

namespace Eitan.Admin.Controllers
{   
    public class ClientsController : Controller
    {
        private EitanDbContext context = new EitanDbContext();

        public ClientsController()
        {
            ViewBag.ClientsActive = "active";
        }

        //
        // GET: /Clients/

        public ViewResult Index()
        {
            return View(context.Clients.ToList());
        }

        //
        // GET: /Clients/Details/5

        public ViewResult Details(int id)
        {
            Client client = context.Clients.Single(x => x.ID == id);
            return View(client);
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
        public ActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                var image = WebImage.GetImageFromRequest("UploadedImage");

                if (image != null)
                    client.MainImage = Server.MapPath("/Images/Clients/").SaveImage(image);

                client.Date_Creation = DateTime.Now;

                context.Clients.Add(client);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(client);
        }
        
        //
        // GET: /Clients/Edit/5
 
        public ActionResult Edit(int id)
        {
            Client client = context.Clients.Single(x => x.ID == id);
            return View(client);
        }

        //
        // POST: /Clients/Edit/5

        [HttpPost]
        public ActionResult Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                var image = WebImage.GetImageFromRequest("UploadedImage");

                if (image != null)
                    client.MainImage = Server.MapPath("/Images/Clients/").SaveImage(image);

                context.Entry(client).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        //
        // GET: /Clients/Delete/5
 
        public ActionResult Delete(int id)
        {
            Client client = context.Clients.Single(x => x.ID == id);
            return View(client);
        }

        //
        // POST: /Clients/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = context.Clients.Single(x => x.ID == id);
            context.Clients.Remove(client);
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