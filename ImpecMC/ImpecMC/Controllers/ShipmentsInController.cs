using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ImpecMC.Models;
using System.IO;
using PagedList;

namespace ImpecMC.Controllers
{
    public class ShipmentsInController : Controller
    {
        private ImpecDBContext db = new ImpecDBContext();

        // GET: ShipmentsIn
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var shipmentsIn = db.ShipmentsIn.OrderByDescending(s => s.Id).Include(s => s.Division);

            if (!String.IsNullOrEmpty(searchString))
            {
                shipmentsIn = shipmentsIn.Where(s => s.FZInNum.Contains(searchString)
                                       || s.CommercialInvoiceNum.Contains(searchString)
                                       || s.AWBBOL.Contains(searchString)
                                       || s.ClaimsNum.Contains(searchString)
                                       || s.FOB.Contains(searchString)
                                       || (s.Division != null && s.Division.Name.Contains(searchString)));
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(shipmentsIn.ToPagedList(pageNumber, pageSize));
            //var shipmentsIn = db.ShipmentsIn.OrderByDescending(s=>s.Id).Include(s => s.Division);
            //return View(shipmentsIn.ToList());
        }

        // GET: ShipmentsIn/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShipmentIn shipmentIn = db.ShipmentsIn.Find(id);
            if (shipmentIn == null)
            {
                return HttpNotFound();
            }
            return View(shipmentIn);
        }

        // GET: ShipmentsIn/Create
        public ActionResult Create()
        {
            ViewBag.DivisionId = new SelectList(db.Divisions, "Id", "Name");
            return View();
        }

        // POST: ShipmentsIn/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FZInNum,CommercialInvoiceNum,AWBBOL,DocReceivedDate,ArrivedPortDate,ArrivedFZDate,FreightType,DivisionId,Status,Insurance,ShipmentType,ClaimsNum,FOB,TotalCost")] ShipmentIn shipmentIn)
        {
            if (ModelState.IsValid)
            {
                shipmentIn.DateCreated = DateTime.UtcNow.AddHours(2);
                db.ShipmentsIn.Add(shipmentIn);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DivisionId = new SelectList(db.Divisions, "Id", "Name", shipmentIn.DivisionId);
            return View(shipmentIn);
        }

        // GET: ShipmentsIn/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShipmentIn shipmentIn = db.ShipmentsIn.Find(id);
            if (shipmentIn == null)
            {
                return HttpNotFound();
            }
            ViewBag.DivisionId = new SelectList(db.Divisions, "Id", "Name", shipmentIn.DivisionId);
            return View(shipmentIn);
        }

        // POST: ShipmentsIn/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateCreated,FZInNum,CommercialInvoiceNum,AWBBOL,DocReceivedDate,ArrivedPortDate,ArrivedFZDate,FreightType,DivisionId,Status,Insurance,ShipmentType,ClaimsNum,FOB,TotalCost")] ShipmentIn shipmentIn)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shipmentIn).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DivisionId = new SelectList(db.Divisions, "Id", "Name", shipmentIn.DivisionId);
            return View(shipmentIn);
        }

        // GET: ShipmentsIn/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShipmentIn shipmentIn = db.ShipmentsIn.Find(id);
            if (shipmentIn == null)
            {
                return HttpNotFound();
            }
            return View(shipmentIn);
        }

        // POST: ShipmentsIn/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShipmentIn shipmentIn = db.ShipmentsIn.Find(id);
            db.ShipmentsIn.Remove(shipmentIn);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase file)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the fielname
                var fileName = Path.GetFileName(file.FileName);

                // then save on the server...
                var path = Path.Combine(Server.MapPath("~/uploads"), fileName);
          
                file.SaveAs(path);
                ExcelImport imp = new ExcelImport( path, "FZ IN");
                imp.ImportShipmentsIn();

            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ImportItems(HttpPostedFileBase file, int id)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the fielname
                var fileName = Path.GetFileName(file.FileName);

                // then save on the server...
                var path = Path.Combine(Server.MapPath("~/uploads"), fileName);
                file.SaveAs(path);
                ExcelImport imp = new ExcelImport(path, "ITEM");
                imp.ImportItemsIntoShipmentIn(id);

            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Edit", new { id = id });
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
