using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ImpecMC.Models;

namespace ImpecMC.Controllers
{
    public class ServiceCompaniesController : Controller
    {
        private ImpecDBContext db = new ImpecDBContext();

        // GET: ServiceCompanies
        public ActionResult Index()
        {
            return View(db.ServiceCompanies.ToList());
        }

        // GET: ServiceCompanies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceCompany serviceCompany = db.ServiceCompanies.Find(id);
            if (serviceCompany == null)
            {
                return HttpNotFound();
            }
            return View(serviceCompany);
        }

        // GET: ServiceCompanies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServiceCompanies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] ServiceCompany serviceCompany)
        {
            if (ModelState.IsValid)
            {
                serviceCompany.DateCreated = DateTime.UtcNow.AddHours(2);
                db.ServiceCompanies.Add(serviceCompany);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(serviceCompany);
        }

        // GET: ServiceCompanies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceCompany serviceCompany = db.ServiceCompanies.Find(id);
            if (serviceCompany == null)
            {
                return HttpNotFound();
            }
            return View(serviceCompany);
        }

        // POST: ServiceCompanies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateCreated,Name")] ServiceCompany serviceCompany)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceCompany).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(serviceCompany);
        }

        // GET: ServiceCompanies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceCompany serviceCompany = db.ServiceCompanies.Find(id);
            if (serviceCompany == null)
            {
                return HttpNotFound();
            }
            return View(serviceCompany);
        }

        // POST: ServiceCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServiceCompany serviceCompany = db.ServiceCompanies.Find(id);
            db.ServiceCompanies.Remove(serviceCompany);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
