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
using System.Text.RegularExpressions;
using PagedList;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace ImpecMC.Controllers
{
    public class ItemsController : Controller
    {
        private const int defaultPageSize = 10;
        private ImpecDBContext db = new ImpecDBContext();

        // GET: Items
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

            var items = (IQueryable<Item>)db.Items.OrderByDescending(s => s.Id);

            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Description.Contains(searchString)
                                       || s.HSCode.Contains(searchString)
                                       || s.PartNumber.Contains(searchString)
                                       || s.UOM.Contains(searchString));
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));

            //var items = db.Items.Take(10).Include(i => i.ShipmentIn);//db.Items.Include(i => i.ShipmentIn);

            //return View(items.ToList());
        }

        public ActionResult Inventory(string currentFilter, string searchString, int? page)
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




            //var inventoryitems = db.Items.GroupBy(s => s.ShipmentIn.FZInNum)
            //    .Select(d => d).Take(20).ToList();

            //List<InventoryViewModel> ivms = new List<InventoryViewModel>();
            List<InventoryViewModel> inventoryitems = new List<InventoryViewModel>();
            

            string queryString =
            @"SELECT PartNumber, Description, SUM(item.Quantity), SUM(dtitem.Quantity)
            FROM dbo.Items item
                LEFT OUTER JOIN DeliveryTicketItems dtitem
                    ON dtitem.ItemId = item.Id
            GROUP BY PartNumber, Description;";
             
            String connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(
               connectionString))
            {
                SqlCommand command = new SqlCommand(
                    queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        InventoryViewModel ivm = new InventoryViewModel();
                        ivm.PartNumber = reader[0].ToString();
                        ivm.Description = reader[1].ToString();
                        int quantityinTmp = 0;
                        if (int.TryParse(reader[2].ToString(), out quantityinTmp))
                        {
                            ivm.QuantityIn = quantityinTmp;
                        }
                        int quantityOutTmp = 0;
                        if (int.TryParse(reader[3].ToString(), out quantityOutTmp))
                        {
                            ivm.QuantityOut = quantityOutTmp;
                        }

                        ivm.QuantityOnHand = ivm.QuantityIn - ivm.QuantityOut;

                        //Console.WriteLine(String.Format("{0}",
                        //    reader[0]));
                        inventoryitems.Add(ivm);
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
            }

            //foreach(var its in inventoryitems){
            //    InventoryViewModel ivm = new InventoryViewModel();
            //    ivm.PartNumber = its.First().PartNumber;
            //    ivm.Description = its.First().Description;
            //    ivm.QuantityIn = its.Sum(d=>d.Quantity).Value;
            //    foreach (var it in its)
            //    {
            //        foreach (var dtit in it.DeliveryTicketItems)
            //        {
            //            if (dtit.Quantity.HasValue && dtit.Quantity > 0)
            //            {
            //                ivm.QuantityOut += dtit.Quantity.Value;
            //            }
            //        }
            //    }
            //    //ivm.QuantityOut = its.Sum(d => d.DeliveryTicketItems.Sum(f => f.Quantity).Value);
            //    ivm.QuantityOnHand = ivm.QuantityIn - ivm.QuantityOut;

            //    ivms.Add(ivm);
            //}

            if (!String.IsNullOrEmpty(searchString))
            {
                inventoryitems = inventoryitems.Where(s => s.Description.Contains(searchString)
                                       || s.PartNumber.Contains(searchString)).ToList();
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(inventoryitems.ToPagedList(pageNumber, pageSize));
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            ViewBag.ShipmentInId = new SelectList(db.ShipmentsIn, "Id", "FZInNum");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Quantity,PartNumber,Description,UOM,HSCode,UnitPrice,ShipmentInId")] Item item)
        {
            if (ModelState.IsValid)
            {
                item.DateCreated = DateTime.UtcNow.AddHours(2);
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ShipmentInId = new SelectList(db.ShipmentsIn, "Id", "FZInNum", item.ShipmentInId);
            return View(item);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.ShipmentInId = new SelectList(db.ShipmentsIn, "Id", "FZInNum", item.ShipmentInId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateCreated,Quantity,PartNumber,Description,UOM,HSCode,UnitPrice,ShipmentInId")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ShipmentInId = new SelectList(db.ShipmentsIn, "Id", "FZInNum", item.ShipmentInId);
            return View(item);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
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

                ExcelImport imp = new ExcelImport(path, "ITEM P/N");//8378
                imp.ImportItems();
                //imp.readExcelFile(path);
             
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Index");
        }


        



        public string[] parseCSV(string line)
        {
            List<string> datalist = new List<string>();

            /*
             * Define a regular expression for csv.
             * This Pattern will match on either quoted text or text between commas, including
             * whitespace, and accounting for beginning and end of line.
             */

            Regex rx = new Regex("\"([^\"]*)\"|(?<=,|^)([^,]*)(?:,|$)",
              RegexOptions.Compiled | RegexOptions.IgnoreCase);

            // Find matches.
            MatchCollection matches = rx.Matches(line);

            // Report the number of matches found.
            Console.WriteLine("{0} matches found.", matches.Count);

            // Report on each match.
            foreach (Match match in matches)
            {
                if (match.Groups[1].Value.Length > 0)
                    datalist.Add(match.Groups[1].Value); // match csv values inside commas
                else
                    datalist.Add(match.Groups[2].Value); // match csv values outside commas
            }
            return datalist.ToArray();
        }

    }
}
