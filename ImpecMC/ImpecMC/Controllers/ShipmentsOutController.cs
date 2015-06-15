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
    public class ShipmentsOutController : Controller
    {
        private ImpecDBContext db = new ImpecDBContext();

        // GET: ShipmentsOut
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

            var shipmentsOut = db.ShipmentsOut.OrderByDescending(s => s.DateCreated).Include(s => s.ServiceCompany).Include(s => s.OwnerCompany);

            if (!String.IsNullOrEmpty(searchString))
            {
                shipmentsOut = shipmentsOut.Where(s => s.CDNumber.Contains(searchString)
                                       || s.ExportNumber.Contains(searchString)
                                       || s.InvoiceNumber.Contains(searchString)
                                       || s.KasimaNumber.Contains(searchString)
                                       || s.Notes.Contains(searchString)
                                       || s.ShahadaNumber.Contains(searchString)
                                       || s.Status.Contains(searchString)
                                       || (s.OwnerCompany != null && s.OwnerCompany.Name.Contains(searchString))
                                       || (s.ServiceCompany != null && s.ServiceCompany.Name.Contains(searchString)));
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(shipmentsOut.ToPagedList(pageNumber, pageSize));
            //var shipmentsOut = db.ShipmentsOut.OrderByDescending(s => s.DateCreated).Include(s => s.ServiceCompany).Include(s => s.OwnerCompany);
            //return View(shipmentsOut.ToList());
        }

        // GET: ShipmentsOut/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ShipmentOut shipmentOut = db.ShipmentsOut.Find(id);
            if (shipmentOut == null)
            {
                return HttpNotFound();
            }
            return View(shipmentOut);
        }

        // GET: ShipmentsOut/Create
        public ActionResult Create()
        {
            ViewBag.ServiceCompanyId = new SelectList(db.ServiceCompanies, "Id", "Name");
            ViewBag.OwnerCompanyId = new SelectList(db.OwnerCompanies, "Id", "Name");
            return View();
        }

        // POST: ShipmentsOut/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,InvoiceNumber,ServiceCompanyId,OwnerCompanyId,CDNumber,ShahadaNumber,KasimaNumber,ExportNumber,SendDate,ReceivedDate,Status,Type,Notes")] ShipmentOut shipmentOut)
        {
            if (ModelState.IsValid)
            {
                shipmentOut.DateCreated = DateTime.UtcNow.AddHours(2);
                db.ShipmentsOut.Add(shipmentOut);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ServiceCompanyId = new SelectList(db.ServiceCompanies, "Id", "Name", shipmentOut.ServiceCompanyId);
            ViewBag.OwnerCompanyId = new SelectList(db.OwnerCompanies, "Id", "Name", shipmentOut.OwnerCompanyId);
            return View(shipmentOut);
        }

        // GET: ShipmentsOut/Edit/5
        public ActionResult Edit(int? id)   
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShipmentOut shipmentOut = db.ShipmentsOut.Find(id);
            if (shipmentOut == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServiceCompanyId = new SelectList(db.ServiceCompanies, "Id", "Name", shipmentOut.ServiceCompanyId);
            ViewBag.OwnerCompanyId = new SelectList(db.OwnerCompanies, "Id", "Name", shipmentOut.OwnerCompanyId);
            return View(shipmentOut);
        }

        // POST: ShipmentsOut/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateCreated,InvoiceNumber,ServiceCompanyId,OwnerCompanyId,CDNumber,ShahadaNumber,KasimaNumber,ExportNumber,SendDate,ReceivedDate,Status,Type,Notes")] ShipmentOut shipmentOut)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shipmentOut).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ServiceCompanyId = new SelectList(db.ServiceCompanies, "Id", "Name", shipmentOut.ServiceCompanyId);
            ViewBag.OwnerCompanyId = new SelectList(db.OwnerCompanies, "Id", "Name", shipmentOut.OwnerCompanyId);
            return View(shipmentOut);
        }

        // GET: ShipmentsOut/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShipmentOut shipmentOut = db.ShipmentsOut.Find(id);
            if (shipmentOut == null)
            {
                return HttpNotFound();
            }
            return View(shipmentOut);
        }

        // POST: ShipmentsOut/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShipmentOut shipmentOut = db.ShipmentsOut.Find(id);
            db.ShipmentsOut.Remove(shipmentOut);
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


        public FilePathResult Download(int id)
        {
            ShipmentOut shipmentOut = db.ShipmentsOut.Find(id);
            var path = Server.MapPath("~/uploads");
            var logoPath = Path.Combine(Server.MapPath("~/Images/Final Impec logo m.png"));

            string output = PDF.CreatePDF(shipmentOut, path, logoPath);
            string filename = Path.GetFileName(output);
            return File(output, "pdf/plain", "Commercial Invoice - " + MakeValidFileName(shipmentOut.InvoiceNumber) + ".pdf");
        }

        private static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        }

        [HttpPost]
        public ActionResult ImportDT(HttpPostedFileBase file)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the fielname
                var fileName = Path.GetFileName(file.FileName) + DateTime.Now.Ticks;

                // then save on the server...
                var path = Path.Combine(Server.MapPath("~/uploads"), fileName);

                file.SaveAs(path);
                ExcelImport imp = new ExcelImport(path, "ITEM");
                imp.ImportDeliveryTickets();

            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ImportDTDTs(HttpPostedFileBase file, int id)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the fielname
                var fileName = Path.GetFileName(file.FileName) + DateTime.Now.Ticks;

                // then save on the server...
                var path = Path.Combine(Server.MapPath("~/uploads"), fileName);

                file.SaveAs(path);
                ExcelImport imp = new ExcelImport(path, "ITEM");
                imp.ImportDeliveryTicketsIntoShipmentOut(id);
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Edit", new { id = id });
        }

        [HttpPost]
        public ActionResult ImportSO(HttpPostedFileBase file)
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
                imp.ImportShipmentsOut();
               

            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Index");
        }

        


        public string updateIfnotEmpty(string old, string neW)
        {
            if (neW == "")
            {
                return old;
            }
            return neW;
        }
     
    }
}
