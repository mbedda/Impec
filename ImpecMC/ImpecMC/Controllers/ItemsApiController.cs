using ImpecMC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace ImpecMC.Controllers
{
    public class ItemsApiController : ApiController
    {
        private ImpecDBContext db = new ImpecDBContext();
        public object GetItems()
        {
            var items = db.Items.Select(item => new
            {
                item.Id,
                item.DateCreated,
                FZInNum = item.ShipmentIn.FZInNum,
                item.PartNumber,
                item.Description,
                item.HSCode,
                item.UOM,
                item.Quantity,
                item.UnitPrice,
                item.ShipmentInId
            }).OrderByDescending(s => s.DateCreated).ToList();

            return items;
        }

        public object GetItems(int id)
        {
            ShipmentIn shipmentIn = db.ShipmentsIn.Find(id);
            if (shipmentIn == null)
            {
                return new List<Item> { };
            }

            var items = shipmentIn.Items.Select(item => new
            {
                item.Id,
                item.DateCreated,
                FZInNum = item.ShipmentIn.FZInNum,
                item.PartNumber,
                item.Description,
                item.HSCode,
                item.UOM,
                item.Quantity,
                item.UnitPrice,
                item.ShipmentInId
            }).OrderByDescending(s => s.DateCreated).ToList();

            return items;
        }



        [ResponseType(typeof(Item))]
        public IHttpActionResult CreateItem(Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            item.DateCreated = DateTime.UtcNow.AddHours(2);

            db.Items.Add(item);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = item.Id }, item);
        }

        [ResponseType(typeof(Item))]
        public IHttpActionResult EditItem(Item item)
        {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = item.Id }, item);
        }
        
        [ResponseType(typeof(Item))]
        public IHttpActionResult RemoveItem(int id)
        {
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            db.Items.Remove(item);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = item.Id }, item);
        }

        [HttpPost]
        public IHttpActionResult Import(HttpPostedFileBase file)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the fielname
                var fileName = Path.GetFileName(file.FileName);

                // then save on the server...
                var path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/uploads"), fileName);
                file.SaveAs(path);
                List<List<string>> Output = readExcel(path, 1, 10, 0);// for production, set the rows number to 0

                var items = db.Items.ToList();
                for (int i = 9; i < Output.Count; i++)//3
                {
                    Item item = new Item();
                    item.DateCreated = DateTime.Now;
                    string freezone = Output[i][1].TrimStart('0');
                    string partNumber = Output[i][4];
                    string description = Output[i][5];


                    var shipmentsIn = db.ShipmentsIn.Where(s => s.FZInNum == freezone).ToList();

                    if (shipmentsIn.Count > 0
                        && items.Where(s => s.ShipmentIn != null && s.ShipmentIn.FZInNum == freezone
                            && s.PartNumber == partNumber
                            && s.Description == description).ToList().Count == 0)
                    {


                        if (Output[i][1] != "")
                        {
                            item.ShipmentIn = shipmentsIn.First();
                        }
                        if (Output[i][3] != "")
                        {
                            item.Quantity = Convert.ToInt32(Output[i][3]);
                        }
                        if (Output[i][4] != "")
                        {
                            item.PartNumber = Output[i][4];
                        }
                        if (Output[i][5] != "")
                        {
                            item.Description = Output[i][5];
                        }

                        db.Items.Add(item);
                    }



                }
                db.SaveChanges();

            }
            // redirect back to the index action to show the form once again
            return Ok();
        }

        private List<List<string>> readExcel(String filename, int tabNumber, int columnsNumber, int rowsNumber)
        {
            Microsoft.Office.Interop.Excel._Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            Microsoft.Office.Interop.Excel.Range range;

            List<List<string>> Output = new List<List<string>>();
            List<string> colsL = new List<string>();
            string str;

            int rCnt = 0;
            int cCnt = 0;
            int epcount = 0;

            xlWorkBook = xlApp.Workbooks.Open(filename, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            int maxTabNum = xlWorkBook.Worksheets.Count;

            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(tabNumber);
            range = xlWorkSheet.UsedRange;
            if (rowsNumber == 0)
            {
                rowsNumber = range.Rows.Count;
            }


            for (rCnt = 1; rCnt <= rowsNumber; rCnt++)
            {
                int attCounter = 0;
                for (cCnt = 1; cCnt <= columnsNumber; cCnt++)
                {
                    str = (range.Cells[rCnt, cCnt] as Microsoft.Office.Interop.Excel.Range).Value2 + "";
                    str = str.Trim();
                    colsL.Add(str);
                    attCounter++;
                }
                Output.Add(colsL);
                colsL = new List<string>();
                epcount++;

            }
            FileInfo f = new FileInfo(filename);
            long s1 = f.Length;
            if (s1 > 2195056)
            {
                xlApp.Quit();
            }
            else
            {
                // xlWorkBook.Close(true, null, null);
                xlApp.Quit();
            }


            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
            return Output;

        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                //MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}   
