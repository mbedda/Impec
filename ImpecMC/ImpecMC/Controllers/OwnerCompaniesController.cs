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
    public class OwnerCompaniesController : Controller
    {
        private ImpecDBContext db = new ImpecDBContext();

        // GET: OwnerCompanies
        public ActionResult Index()
        {
            return View(db.OwnerCompanies.ToList());
        }

        // GET: OwnerCompanies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerCompany ownerCompany = db.OwnerCompanies.Find(id);
            if (ownerCompany == null)
            {
                return HttpNotFound();
            }
            return View(ownerCompany);
        }

        // GET: OwnerCompanies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OwnerCompanies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] OwnerCompany ownerCompany)
        {
            if (ModelState.IsValid)
            {
                ownerCompany.DateCreated = DateTime.UtcNow.AddHours(2);
                db.OwnerCompanies.Add(ownerCompany);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ownerCompany);
        }

        // GET: OwnerCompanies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerCompany ownerCompany = db.OwnerCompanies.Find(id);
            if (ownerCompany == null)
            {
                return HttpNotFound();
            }
            return View(ownerCompany);
        }

        // POST: OwnerCompanies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateCreated,Name")] OwnerCompany ownerCompany)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ownerCompany).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ownerCompany);
        }

        // GET: OwnerCompanies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerCompany ownerCompany = db.OwnerCompanies.Find(id);
            if (ownerCompany == null)
            {
                return HttpNotFound();
            }
            return View(ownerCompany);
        }

        // POST: OwnerCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OwnerCompany ownerCompany = db.OwnerCompanies.Find(id);
            db.OwnerCompanies.Remove(ownerCompany);
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
