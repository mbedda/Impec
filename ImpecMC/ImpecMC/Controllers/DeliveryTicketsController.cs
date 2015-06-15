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
    public class DeliveryTicketsController : Controller
    {
        private ImpecDBContext db = new ImpecDBContext();

        // GET: /DeliveryTickets/
        public ActionResult Index()
        {
            var deliveryticket = db.DeliveryTickets.Include(d => d.ShipmentOut);
            return View(deliveryticket.ToList());
        }

        // GET: /DeliveryTickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryTicket deliveryticket = db.DeliveryTickets.Find(id);
            if (deliveryticket == null)
            {
                return HttpNotFound();
            }
            return View(deliveryticket);
        }

        // GET: /DeliveryTickets/Create
        public ActionResult Create()
        {
            ViewBag.ShipmentOutId = new SelectList(db.ShipmentsOut, "Id", "InvoiceNumber");
            return View();
        }

        // POST: /DeliveryTickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,DateCreated,ShipmentOutId,DTNumber,SONumber,PONumber")] DeliveryTicket deliveryticket)
        {
            if (ModelState.IsValid)
            {
                deliveryticket.DateCreated = DateTime.UtcNow.AddHours(2);
                db.DeliveryTickets.Add(deliveryticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ShipmentOutId = new SelectList(db.ShipmentsOut, "Id", "InvoiceNumber", deliveryticket.ShipmentOutId);
            return View(deliveryticket);
        }

        // GET: /DeliveryTickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryTicket deliveryticket = db.DeliveryTickets.Find(id);
            if (deliveryticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.ShipmentOutId = new SelectList(db.ShipmentsOut, "Id", "InvoiceNumber", deliveryticket.ShipmentOutId);
            return View(deliveryticket);
        }

        // POST: /DeliveryTickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,DateCreated,ShipmentOutId,DTNumber,SONumber,PONumber")] DeliveryTicket deliveryticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deliveryticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ShipmentOutId = new SelectList(db.ShipmentsOut, "Id", "InvoiceNumber", deliveryticket.ShipmentOutId);
            return View(deliveryticket);
        }

        // GET: /DeliveryTickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryTicket deliveryticket = db.DeliveryTickets.Find(id);
            if (deliveryticket == null)
            {
                return HttpNotFound();
            }
            return View(deliveryticket);
        }

        // POST: /DeliveryTickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeliveryTicket deliveryticket = db.DeliveryTickets.Find(id);
            db.DeliveryTickets.Remove(deliveryticket);
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
