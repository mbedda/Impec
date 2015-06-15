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
    public class DeliveryTicketItemsController : Controller
    {
        private ImpecDBContext db = new ImpecDBContext();

        // GET: DeliveryTicketItems
        public ActionResult Index()
        {
            var deliveryTicketItems = db.DeliveryTicketItems.Include(d => d.DeliveryTicket).Include(d => d.Item);
            return View(deliveryTicketItems.ToList());
        }

        // GET: DeliveryTicketItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryTicketItem deliveryTicketItem = db.DeliveryTicketItems.Find(id);
            if (deliveryTicketItem == null)
            {
                return HttpNotFound();
            }
            return View(deliveryTicketItem);
        }

        // GET: DeliveryTicketItems/Create
        public ActionResult Create()
        {
            ViewBag.DeliveryTicketId = new SelectList(db.DeliveryTickets, "Id", "Id");
            ViewBag.ItemId = new SelectList(db.Items, "Id", "PartNumber");
            return View();
        }

        // POST: DeliveryTicketItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DeliveryTicketId,ItemId,Quantity,UnitPrice")] DeliveryTicketItem deliveryTicketItem)
        {
            if (ModelState.IsValid)
            {
                deliveryTicketItem.DateCreated = DateTime.UtcNow.AddHours(2);
                db.DeliveryTicketItems.Add(deliveryTicketItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DeliveryTicketId = new SelectList(db.DeliveryTickets, "Id", "Id", deliveryTicketItem.DeliveryTicketId);
            ViewBag.ItemId = new SelectList(db.Items, "Id", "PartNumber", deliveryTicketItem.ItemId);
            return View(deliveryTicketItem);
        }

        // GET: DeliveryTicketItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryTicketItem deliveryTicketItem = db.DeliveryTicketItems.Find(id);
            if (deliveryTicketItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeliveryTicketId = new SelectList(db.DeliveryTickets, "Id", "Id", deliveryTicketItem.DeliveryTicketId);
            ViewBag.ItemId = new SelectList(db.Items, "Id", "PartNumber", deliveryTicketItem.ItemId);
            return View(deliveryTicketItem);
        }

        // POST: DeliveryTicketItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateCreated,DeliveryTicketId,ItemId,Quantity,UnitPrice")] DeliveryTicketItem deliveryTicketItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deliveryTicketItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DeliveryTicketId = new SelectList(db.DeliveryTickets, "Id", "Id", deliveryTicketItem.DeliveryTicketId);
            ViewBag.ItemId = new SelectList(db.Items, "Id", "PartNumber", deliveryTicketItem.ItemId);
            return View(deliveryTicketItem);
        }

        // GET: DeliveryTicketItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryTicketItem deliveryTicketItem = db.DeliveryTicketItems.Find(id);
            if (deliveryTicketItem == null)
            {
                return HttpNotFound();
            }
            return View(deliveryTicketItem);
        }

        // POST: DeliveryTicketItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeliveryTicketItem deliveryTicketItem = db.DeliveryTicketItems.Find(id);
            db.DeliveryTicketItems.Remove(deliveryTicketItem);
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
