using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImpecMC.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using ImpecMC.Models.Excel;
using System.Text.RegularExpressions;

namespace ImpecMC.Models
{
    public class ExcelImport
    {
        ImpecDBContext db;
        string path;
        string startingText;

        public ExcelImport( string Path, string StartingText)
        {
            db = new ImpecDBContext();
            path = Path;
            startingText = StartingText;
        }

        //Start Modelizing functions

        public List<ExcelItem> ModelizeItems(List<List<string>> Input)
        {
            List<ExcelItem> output = new List<ExcelItem>();
            double ratio = (double)100 / (double)Input.Count;
            for (int i = 0; i < Input.Count ; i++ )
            {

                    ExcelItem n = new ExcelItem();
                    Boolean hasValue = false;
                    if (!isEmpty(Input[i][1].TrimStart('0')))
                    {
                        n.FZInNum = Input[i][1].TrimStart('0');
                        hasValue = true;
                    }
                    if (!isEmpty(Input[i][2].Trim()))
                    {
                        n.DivisionName = Input[i][2].Trim();
                        hasValue = true;
                    }
                    if (!isEmpty(Input[i][3]))
                    {
                        int quantity = 0;
                        if (int.TryParse(Input[i][3], out quantity))
                        {
                            n.InQuantity = quantity;
                            hasValue = true;
                        }
                    }

                    if (!isEmpty(Input[i][4]))
                    {
                        n.PartNumber = Input[i][4];
                        hasValue = true;
                    }

                    if (!isEmpty(Input[i][5]))
                    {
                        n.Description = Input[i][5];
                        hasValue = true;
                    }


                    if (!isEmpty(Input[i][6]))
                    {
                        int quantity = 0;
                        if (int.TryParse(Input[i][6], out quantity))
                        {
                            n.OutQuantity = quantity;
                            hasValue = true;
                        }
                    }

                    if(hasValue)
                    {
                        output.Add(n);
                    }
                    System.Diagnostics.Debug.WriteLine("Modelizing data :  " + i * ratio + " %");
                    
                
            }
                return output;
        }

        public List<ExcelShipmentIn> ModelizeShipmentsIn(List<List<string>> Input)
        {
            List<ExcelShipmentIn> output = new List<ExcelShipmentIn>();
            double ratio = (double)100 / (double)Input.Count;
            for (int i = 0; i < Input.Count; i++)
            {

                ExcelShipmentIn n = new ExcelShipmentIn();
                Boolean hasValue = false;
                if (!isEmpty(Input[i][1]))
                {
                    n.FZInNum = Input[i][1];
                    hasValue = true;
                }
                if (!isEmpty(Input[i][2].Trim()))
                {
                    n.CommercialInvoiceNum = Input[i][2].Trim();
                    hasValue = true;
                }
                if (!isEmpty(Input[i][4]))
                {
                    n.AWBBOL = Input[i][4];
                    hasValue = true;
                }
                if (Input[i][5] == "AIR")
                {
                    n.FreightType = ShipmentInFreightType.Air;
                    hasValue = true;
                }
                else if (Input[i][5] == "LAND")
                {
                    n.FreightType = ShipmentInFreightType.Land;
                    hasValue = true;
                }
                else if (Input[i][5] == "OCEAN")
                {
                    n.FreightType = ShipmentInFreightType.Ocean;
                    hasValue = true;
                }
                if (!isEmpty(Input[i][6]))
                {
                    DateTime d;
                    if (DateTime.TryParse(Input[i][6], out d))
                    {
                        n.DocReceivedDate = Convert.ToDateTime(Input[i][6]);
                        hasValue = true;
                    }
                  
                }
                if (!isEmpty(Input[i][7]))
                {
                    n.DivisionName = Input[i][7];
                    hasValue = true;
                }
                if (Input[i][8] == "COMPLETED")
                {
                    n.Status = ShipmentInStatus.Complete;
                    hasValue = true;
                }
                else if (Input[i][8] == "WAITING")
                {
                    n.Status = ShipmentInStatus.Waiting;
                    hasValue = true;
                }
                else if (Input[i][8] == "CANCELED")
                {
                    n.Status = ShipmentInStatus.Canceled;
                    hasValue = true;
                }
                if (!isEmpty(Input[i][10]))
                {
                    if (Input[i][10].Contains("$"))
                    {
                        string[] s = Input[i][10].Split('$');
                        n.TotalCost = ConvertToDoubleSafely(s[1]);
                        hasValue = true;
                    }
                    else
                    {
                        n.TotalCost = ConvertToDoubleSafely(Input[i][10]);
                        hasValue = true;
                    }
                }
                if (!isEmpty(Input[i][12]))
                {
                    if (Input[i][12].Contains("$"))
                    {
                        string[] s = Input[i][12].Split('$');
                        n.Insurance = ConvertToDoubleSafely(s[1]);
                        hasValue = true;
                    }
                    else
                    {
                        n.Insurance = ConvertToDoubleSafely(Input[i][12]);
                        hasValue = true;
                    }
                }
                if (hasValue)
                {
                    output.Add(n);
                }
                System.Diagnostics.Debug.WriteLine("Modelizing data :  " + i * ratio + " %");


            }
            return output;
        }

        public List<ExcelShipmentOut> ModelizeShipmentsOut(List<List<string>> Input)
        {
            List<ExcelShipmentOut> output = new List<ExcelShipmentOut>();
            double ratio = (double)100 / (double)Input.Count;
            for (int i = 0; i < Input.Count; i++)
            {

                ExcelShipmentOut n = new ExcelShipmentOut();
                Boolean hasValue = false;
                if (!isEmpty(Input[i][0]))
                {
                    n.invoice = Input[i][0];
                    hasValue = true;
                }
                if (!isEmpty(Input[i][1].Trim()))
                {
                    n.dt = Input[i][1].Trim();
                    hasValue = true;
                }
                if (!isEmpty(Input[i][2]))
                {
                    n.company = Input[i][2];
                    hasValue = true;
                }
                if (!isEmpty(Input[i][3]))
                {
                    n.owner = Input[i][3];
                    hasValue = true;
                }
                if (!isEmpty(Input[i][4]))
                {
                    n.cdn = Input[i][4];
                    hasValue = true;
                }
                if (!isEmpty(Input[i][5]))
                {
                    n.shahada = Input[i][5];
                    hasValue = true;
                }
                if (!isEmpty(Input[i][6]))
                {
                    n.kasima = Input[i][6];
                    hasValue = true;
                }
                if (!isEmpty(Input[i][7]))
                {
                    n.export = Input[i][7];
                    hasValue = true;
                }
                if (!isEmpty(Input[i][8]))
                {
                    n.totalINV = Input[i][8];
                    hasValue = true;
                }

                int sendDateNum = 0;
                if (int.TryParse(Input[i][9], out sendDateNum))
                {
                    n.sendDate = FromExcelSerialDate(sendDateNum);
                    hasValue = true;
                }
                int rcvdDateNum = 0;
                if (int.TryParse(Input[i][10], out rcvdDateNum))
                {
                    n.rcvd = FromExcelSerialDate(rcvdDateNum);
                    hasValue = true;
                }
            
                if (!isEmpty(Input[i][11]))
                {
                    n.status = Input[i][11];
                    hasValue = true;
                }
                 if (!isEmpty(Input[i][12]))
                {
                    n.notes = Input[i][12];
                    hasValue = true;
                }
                if (hasValue)
                {
                    output.Add(n);
                }
                System.Diagnostics.Debug.WriteLine("Modelizing data :  " + i * ratio + " %");


            }
            return output;
        }

        public List<ExcelDeliveryTicket> ModelizeDeliveryTickets(List<List<string>> Input)
        {
            List<ExcelDeliveryTicket> output = new List<ExcelDeliveryTicket>();
            double ratio = (double)100 / (double)Input.Count;
            for (int i = 0; i < Input.Count; i++)
            {

                ExcelDeliveryTicket n = new ExcelDeliveryTicket();
                Boolean hasValue = false;
                if (!isEmpty(Input[i][1]))
                {
                    n.purchaseOrderNo = Input[i][1];
                    hasValue = true;
                }
                if (!isEmpty(Input[i][2].Trim()))
                {
                    n.salesDocument = Input[i][2].Trim();
                    hasValue = true;
                }
                if (!isEmpty(Input[i][3].Trim()))
                {
                    n.delivery = Input[i][3].Trim();
                    hasValue = true;
                }
                if (!isEmpty(Input[i][4].Trim()))
                {
                    n.partNumber = Input[i][4].Trim();
                    hasValue = true;
                }
                if (!isEmpty(Input[i][5].Trim()))
                {
                    n.description = Input[i][5].Trim();
                    hasValue = true;
                }
                if (!isEmpty(Input[i][6].Trim()))
                {
                    int quantity = 0;
                    if (int.TryParse(Input[i][6].Trim(), out quantity))
                    {
                        n.quantity = quantity;
                        hasValue = true;
                    }
                  
                }
                if (!isEmpty(Input[i][7].Trim()))
                {
                   
                    int quantity = 0;
                    if (int.TryParse(Input[i][7].Trim(), out quantity))
                    {
                        n.unitprice = quantity;
                        hasValue = true;
                    }
                  
                }
                if (!isEmpty(Input[i][8].Trim()))
                {
                    n.total = ConvertToDoubleSafely(Input[i][8].Trim());
                    hasValue = true;
                }
                if (hasValue)
                {
                    output.Add(n);
                }
                System.Diagnostics.Debug.WriteLine("Modelizing data :  " + i * ratio + " %");


            }
            return output;
        }


        //End Modelizing functions

        //Start imports
        public void ImportItems()
        {
            List<List<string>> Output = readExcelFile(path);
            List<ExcelItem> ModelizedOutput = ModelizeItems(Output);

            var divisions = db.Divisions.ToList();
            var shipmentsIn = db.ShipmentsIn.ToList();
            var items = db.Items.ToList();
            int startingIndex = getStartingIndexM(ModelizedOutput, "FZInNum", "FREE ZONE");
            int endingIndex = ModelizedOutput.Count;
            double ratio = (double)100 / (double)endingIndex;

            for (int i = startingIndex; i < endingIndex; i++)//3
            {
                Item item = new Item();
                item.DateCreated = DateTime.Now;
                string freezone = ModelizedOutput[i].FZInNum;
                string div = ModelizedOutput[i].DivisionName;
                string partNumber = ModelizedOutput[i].PartNumber;
                string description = ModelizedOutput[i].Description;
                System.Diagnostics.Debug.WriteLine("Importing data :  " + i * ratio + " %");

                var sIns = shipmentsIn.Where(s => s.FZInNum == freezone).ToList();

                if (items.Where(s => s.ShipmentIn != null && s.ShipmentIn.FZInNum == freezone
                        && s.PartNumber == partNumber
                        && s.Description == description).ToList().Count == 0)
                {

                    if (sIns.Count == 0)
                    {
                        ShipmentIn shipin = new ShipmentIn();
                        shipin.DateCreated = DateTime.UtcNow.AddHours(2);
                        shipin.FZInNum = freezone;

                        List<Division> tmpdivisions = divisions.Where(s => s.Name == div).ToList();
                        if (tmpdivisions.Count > 0)
                        {
                            shipin.Division = tmpdivisions.First();
                        }
                        else
                        {
                            Division d = new Division();
                            d.DateCreated = DateTime.Now;
                            d.Name = div;
                            shipin.Division = d;

                            divisions.Add(d);
                        }
                        shipmentsIn.Add(shipin);
                        sIns = shipmentsIn.Where(s => s.FZInNum == freezone).ToList();
                    }


                    if (!isNull(ModelizedOutput[i].FZInNum))
                    {
                        item.ShipmentIn = sIns.First();
                    }

                    if (!isNull(ModelizedOutput[i].InQuantity))
                    {
                        item.Quantity = ModelizedOutput[i].InQuantity;
                    }
                    if (!isNull(ModelizedOutput[i].PartNumber))
                    {
                        item.PartNumber = ModelizedOutput[i].PartNumber;
                    }
                    if (!isNull(ModelizedOutput[i].Description))
                    {
                        item.Description = ModelizedOutput[i].Description;
                    }

                    db.Items.Add(item);
                }
                else if (items.Where(s => s.ShipmentIn != null && s.ShipmentIn.FZInNum == freezone
                        && s.PartNumber == partNumber
                        && s.Description == description).ToList().Count != 0)
                {

                    item = items.Where(s => s.ShipmentIn != null && s.ShipmentIn.FZInNum == freezone
                        && s.PartNumber == partNumber
                        && s.Description == description).ToList()[0];

                    if (!isNull(ModelizedOutput[i].InQuantity))
                    {
                        item.Quantity = ModelizedOutput[i].InQuantity;
                    }



                }



            }
            db.SaveChanges();
        }

        public void ImportShipmentsIn()
        {
            List<List<string>> Output = readExcelFile(path);// 15
            List<ExcelShipmentIn> ModelizedOutput = ModelizeShipmentsIn(Output);

            var divisions = db.Divisions.ToList();
            var shipmentsIn = db.ShipmentsIn.ToList();
            int startingIndex = getStartingIndexM(ModelizedOutput, "FZInNum", "FZ IN");
            int endingIndex = ModelizedOutput.Count;
            double ratio = (double)100 / (double)endingIndex;

            for (int i = startingIndex; i < endingIndex; i++)
            {
                if ( !isNull(ModelizedOutput[i].FZInNum) && !isNull(ModelizedOutput[i].CommercialInvoiceNum)  && shipmentsIn.Where(s => s.FZInNum == ModelizedOutput[i].FZInNum).Count() == 0)
                {
                    ShipmentIn ship = new ShipmentIn();
                    ship.DateCreated = DateTime.Now;
                    System.Diagnostics.Debug.WriteLine("Importing data :  " + i * ratio + " %");

                    if (!isNull(ModelizedOutput[i].FZInNum))
                    {
                        ship.FZInNum = ModelizedOutput[i].FZInNum;
                    }
                    if (!isNull(ModelizedOutput[i].CommercialInvoiceNum))
                    {
                        ship.CommercialInvoiceNum = ModelizedOutput[i].CommercialInvoiceNum;
                    }
                    if (!isNull(ModelizedOutput[i].AWBBOL))
                    {
                        ship.AWBBOL = ModelizedOutput[i].AWBBOL;
                    }
                    if (!isNull(ModelizedOutput[i].FreightType))
                    {
                        ship.FreightType = ModelizedOutput[i].FreightType;
                    }
                    if (!isNull(ModelizedOutput[i].DocReceivedDate))
                    {
                        ship.DocReceivedDate = ModelizedOutput[i].DocReceivedDate;
                    }

                    if (!isNull(ModelizedOutput[i].DivisionName))
                    {
                        string div = ModelizedOutput[i].DivisionName;
                        List<Division> tmpdivisions = divisions.Where(s => s.Name == div).ToList();
                        if (tmpdivisions.Count > 0)
                        {
                            ship.Division = tmpdivisions.First();//needs checkup
                        }
                        else
                        {
                            Division d = new Division();
                            d.DateCreated = DateTime.Now;
                            d.Name = div;
                            ship.Division = d;

                            divisions.Add(d);
                        }
                    }

                    if (!isNull(ModelizedOutput[i].Status))
                    {
                        ship.Status = ModelizedOutput[i].Status;
                    }

                    if (!isNull(ModelizedOutput[i].TotalCost))
                    {
                        ship.TotalCost = ModelizedOutput[i].TotalCost;
                    }
                    if (!isNull(ModelizedOutput[i].Insurance))
                    {
                        ship.Insurance = ModelizedOutput[i].Insurance;
                    }

                    db.ShipmentsIn.Add(ship);

                }
            }
            db.SaveChanges();
        }

        public void ImportItemsIntoShipmentIn(int id)
        {

            List<List<string>> Output = readExcelFile(path);// 10
            List<ExcelItem> ModelizedOutput = ModelizeItems(Output);

            var items = db.Items.ToList();
            int startingIndex = getStartingIndexM(ModelizedOutput, "FZInNum", "FREE ZONE");
            int endingIndex = ModelizedOutput.Count;
            double ratio = (double)100 / (double)endingIndex;

            var shipmentsIn = db.ShipmentsIn.Where(s => s.Id == id).ToList();
            bool itemsInserted = false;
            if (shipmentsIn.Count > 0)
            {
                for (int i = startingIndex; i < endingIndex; i++)//3
                {
                    Item item = new Item();
                    item.DateCreated = DateTime.Now;
                    string freezone = ModelizedOutput[i].FZInNum;
                    string partNumber = ModelizedOutput[i].PartNumber;
                    string description = ModelizedOutput[i].Description;
                    System.Diagnostics.Debug.WriteLine("Importing data :  " + i * ratio + " %");


                    //var shipmentsIn = db.ShipmentsIn.Where(s => s.FZInNum == freezone).ToList();
                    var itemsIdentical = items.Where(s => (s.ShipmentIn == null || (s.ShipmentIn != null && s.ShipmentIn.FZInNum == freezone))
                            && s.PartNumber == partNumber
                            && s.Description == description).ToList();

                    if (shipmentsIn.First().FZInNum == freezone
                        && itemsIdentical.Count == 0)
                    {
                        item.ShipmentIn = shipmentsIn.First();
                        if (!isNull(ModelizedOutput[i].InQuantity))
                        {
                            item.Quantity = ModelizedOutput[i].InQuantity;
                        }
                        if (!isNull(ModelizedOutput[i].PartNumber))
                        {
                            item.PartNumber = ModelizedOutput[i].PartNumber;
                        }
                        if (!isNull(ModelizedOutput[i].Description))
                        {
                            item.Description = ModelizedOutput[i].Description;
                        }

                        db.Items.Add(item);
                        itemsInserted = true;
                    }



                }
            }

            if (itemsInserted)
            {
                shipmentsIn.First().Status = ShipmentInStatus.Complete;
            }

            db.SaveChanges();
        }

        public void ImportShipmentsOut()
        {

            List<List<string>> Output = readExcelFile(path);// 13
            List<ExcelShipmentOut> ModelizedOutput = ModelizeShipmentsOut(Output);

            int startingIndex = getStartingIndexM(ModelizedOutput, "invoice", "INVOICE#");//ITEM
            int endingIndex = ModelizedOutput.Count;
            double ratio = (double)100 / (double)endingIndex;

            var currentShipmentOutID = 0;
            var items = db.Items.ToList();
            for (int i = startingIndex; i < endingIndex; i++)//3
            {
                string invoice = ModelizedOutput[i].invoice;
                string dt = ModelizedOutput[i].dt;
                string company = ModelizedOutput[i].company;
                string owner = ModelizedOutput[i].owner;
                string cdn = ModelizedOutput[i].cdn;
                string shahada = ModelizedOutput[i].shahada;
                string kasima = ModelizedOutput[i].kasima;
                string export = ModelizedOutput[i].export;
                string totalINV = ModelizedOutput[i].totalINV;
                DateTime? sendDate = ModelizedOutput[i].sendDate;
                DateTime? rcvd = ModelizedOutput[i].rcvd;
                string status = ModelizedOutput[i].status;
                string notes = ModelizedOutput[i].notes;
                System.Diagnostics.Debug.WriteLine("Importing data :  " + i * ratio + " %");

                if ( isNull(invoice)  && !isNull(dt)  && isNull(company) && isNull(owner)
                    && isNull(cdn)  && isNull(shahada) && isNull(kasima)  && isNull(export)
                    && isNull(totalINV)  && isNull(sendDate) && isNull(rcvd)  && isNull(status))
                {

                    var shipmentsout = db.ShipmentsOut.ToList();
                    shipmentsout = shipmentsout.Where(s => s.Id == currentShipmentOutID).ToList(); // delivery

                    if (shipmentsout.Count > 0)
                    {
                        var shipmentOut = shipmentsout.First();

                        if (DTNumberExists(dt))
                        {
                            var tickets = db.DeliveryTickets.ToList();
                            tickets = tickets.Where(s => s.DTNumber == dt).ToList(); // delivery
                            var ticket = tickets.First();
                            ticket.ShipmentOutId = shipmentOut.Id;

                        }
                        else
                        {
                            DeliveryTicket ticket = new DeliveryTicket();
                            ticket.DateCreated = DateTime.Now;
                            ticket.DTNumber = dt;
                            ticket.ShipmentOutId = shipmentOut.Id;
                            db.DeliveryTickets.Add(ticket);

                        }
                    }




                }
                else
                {

                    if (!shipmentsExists(invoice, dt))
                    {
                        if (!isNull(invoice))
                        {

                            ShipmentOut shipmentOut = new ShipmentOut();
                            shipmentOut.DateCreated = DateTime.Now;

                            shipmentOut.InvoiceNumber = invoice;
                            if (DTNumberExists(dt))
                            {
                                var tickets = db.DeliveryTickets.ToList();
                                tickets = tickets.Where(s => s.DTNumber == dt).ToList(); // delivery
                                var ticket = tickets.First();
                                ticket.ShipmentOutId = shipmentOut.Id;
                                //shipmentOut.DeliveryTickets.Add(ticket);
                            }
                            else
                            {
                                DeliveryTicket ticket = new DeliveryTicket();
                                ticket.DateCreated = DateTime.Now;
                                ticket.DTNumber = dt;
                                ticket.ShipmentOutId = shipmentOut.Id;
                                db.DeliveryTickets.Add(ticket);
                                //shipmentOut.DeliveryTickets.Add(ticket);
                            }

                            if (ownerCompanyExists(owner))
                            {

                                shipmentOut.OwnerCompany = db.OwnerCompanies.Single(a => a.Name == owner);
                            }
                            else
                            {
                                OwnerCompany ownerC = new OwnerCompany();
                                ownerC.DateCreated = DateTime.Now;
                                ownerC.Name = owner;
                                shipmentOut.OwnerCompany = ownerC;
                            }

                            if (serviceCompanyExists(company))
                            {
                                shipmentOut.ServiceCompany = db.ServiceCompanies.Single(a => a.Name == company);
                            }
                            else
                            {
                                ServiceCompany serviceC = new ServiceCompany();
                                serviceC.DateCreated = DateTime.Now;
                                serviceC.Name = company;
                                shipmentOut.ServiceCompany = serviceC;
                            }
                            shipmentOut.CDNumber = cdn;
                            shipmentOut.ShahadaNumber = shahada;
                            shipmentOut.KasimaNumber = kasima;
                            shipmentOut.ExportNumber = export;
                            //total

                            shipmentOut.SendDate = sendDate;
                            shipmentOut.ReceivedDate = rcvd;
                            shipmentOut.Status = status;
                            shipmentOut.Notes = notes;
                            db.ShipmentsOut.Add(shipmentOut);
                            db.SaveChanges();
                            currentShipmentOutID = shipmentOut.Id;
                        }
                    }


                }

            }
            db.SaveChanges();
        }

        public void ImportDeliveryTickets()
        {

            List<List<string>> Output = readExcelFile(path);// 9
            List<ExcelDeliveryTicket> ModelizedOutput = ModelizeDeliveryTickets(Output);

            string prePurchaseOrderNo = "";
            string preSalesDocument = "";
            string preDelivery = "";

            int startingIndex = getStartingIndexM(ModelizedOutput, "purchaseOrderNo", "Purchase order no.");//ITEM
            int endingIndex = ModelizedOutput.Count;
            double ratio = (double)100 / (double)endingIndex;

            var items = db.Items.ToList();

            for (int i = startingIndex; i < endingIndex; i++)//3
            {
                string purchaseOrderNo = ModelizedOutput[i].purchaseOrderNo;//
                string salesDocument = ModelizedOutput[i].salesDocument;//
                string delivery = ModelizedOutput[i].delivery;//
                string partNumber = ModelizedOutput[i].partNumber;
                string description = ModelizedOutput[i].description;
                int quantity = ModelizedOutput[i].quantity;
                double unitprice = ModelizedOutput[i].unitprice;
                double total = ModelizedOutput[i].total;
                System.Diagnostics.Debug.WriteLine("Importing data :  " + i * ratio + " %");

                if (isNull(purchaseOrderNo)  && isNull(salesDocument)  &&
                    isNull(delivery) && !isNull(partNumber)  && !isNull(description) &&
                    !isNull(quantity)  && isNull(unitprice)  && !isNull(total))
                {
                    purchaseOrderNo = prePurchaseOrderNo;
                    salesDocument = preSalesDocument;
                    delivery = preDelivery;
                }
                else
                {
                    prePurchaseOrderNo = purchaseOrderNo;
                    preSalesDocument = salesDocument;
                    preDelivery = delivery;
                }


                if (!DTNumberExists(delivery)) //dts.Where(s=>s.DTNumber == delivery).Count() == 0
                {
                    DeliveryTicket ticket = new DeliveryTicket();
                    ticket.DateCreated = DateTime.Now;
                    ticket.DTNumber = delivery;
                    ticket.PONumber = purchaseOrderNo;
                    ticket.SONumber = salesDocument;


                    var availableitems = items.Where(s => s.PartNumber == partNumber && s.Quantity > s.DeliveryTicketItems.Sum(d => d.Quantity)).ToList();

                    if (availableitems.Count > 0)
                    {
                        var selectedItem = availableitems.First();
                        var filteredItemsList = availableitems;

                        if (selectedItem.Quantity >= quantity)
                        {
                            DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                            ticketItem.DateCreated = DateTime.Now;
                            ticketItem.Item = selectedItem;
                            ticketItem.DeliveryTicket = ticket;
                            ticketItem.Quantity = quantity;
                            ticketItem.UnitPrice = unitprice;
                            db.DeliveryTicketItems.Add(ticketItem);
                        }
                        else
                        {
                            int target = quantity;
                            int current = 0;
                            int targetindex = 0;
                            for (int j = 0; j < filteredItemsList.Count; j++)
                            {
                                if (filteredItemsList[j].Quantity.HasValue)
                                {
                                    current = current + filteredItemsList[j].Quantity.Value;
                                }
                                if (current >= target)
                                {
                                    targetindex = j;
                                    break;
                                }
                            }


                            if (current == target)
                            {
                                for (int x = 0; x < targetindex + 1; x++)
                                {
                                    DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                                    ticketItem.DateCreated = DateTime.Now;
                                    ticketItem.Item = filteredItemsList[x];
                                    ticketItem.DeliveryTicket = ticket;
                                    ticketItem.Quantity = filteredItemsList[x].Quantity;
                                    ticketItem.UnitPrice = unitprice;
                                    db.DeliveryTicketItems.Add(ticketItem);
                                }
                            }
                            else
                            {
                                int tempCurrent = 0;
                                for (int x = 0; x < targetindex; x++)
                                {
                                    if (filteredItemsList[x].Quantity.HasValue)
                                    {
                                        tempCurrent = tempCurrent + filteredItemsList[x].Quantity.Value;
                                    }
                                    DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                                    ticketItem.DateCreated = DateTime.Now;
                                    ticketItem.Item = filteredItemsList[x];
                                    ticketItem.DeliveryTicket = ticket;
                                    ticketItem.Quantity = filteredItemsList[x].Quantity;
                                    ticketItem.UnitPrice = unitprice;
                                    db.DeliveryTicketItems.Add(ticketItem);
                                }

                                DeliveryTicketItem ticketItemH = new DeliveryTicketItem();
                                ticketItemH.DateCreated = DateTime.Now;
                                ticketItemH.Item = filteredItemsList[targetindex];
                                ticketItemH.DeliveryTicket = ticket;
                                ticketItemH.Quantity = quantity - tempCurrent;
                                ticketItemH.UnitPrice = unitprice;
                                db.DeliveryTicketItems.Add(ticketItemH);

                            }
                        }
                        db.DeliveryTickets.Add(ticket);
                    }
                    db.SaveChanges();
                }
                else
                {
                    var tickets = db.DeliveryTickets.ToList();
                    tickets = tickets.Where(s => s.DTNumber == delivery).ToList(); // delivery
                    var ticket = tickets.First();

                    var availableitems = items.Where(s => s.PartNumber == partNumber && s.Quantity > s.DeliveryTicketItems.Sum(d => d.Quantity)).ToList();

                    if (availableitems.Count > 0)
                    {
                        var selectedItem = availableitems.ToList().First();
                        var filteredItemsList = availableitems.ToList();


                        if (selectedItem.Quantity >= quantity)
                        {
                            var dtis = db.DeliveryTicketItems.Where(s => s.DeliveryTicketId == ticket.Id && s.ItemId == selectedItem.Id).ToList();

                            if (dtis.Count() == 0)
                            {
                                DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                                ticketItem.DateCreated = DateTime.Now;
                                ticketItem.Item = selectedItem;
                                ticketItem.DeliveryTicket = ticket;
                                ticketItem.Quantity = quantity;
                                ticketItem.UnitPrice = unitprice;
                                db.DeliveryTicketItems.Add(ticketItem);
                            }
                        }
                        else
                        {
                            int target = quantity;
                            int current = 0;
                            int targetindex = 0;
                            for (int j = 0; j < filteredItemsList.Count; j++)
                            {
                                if (filteredItemsList[j].Quantity.HasValue)
                                {
                                    current = current + filteredItemsList[j].Quantity.Value;
                                }
                                if (current >= target)
                                {
                                    targetindex = j;
                                    break;
                                }
                            }


                            if (current == target)
                            {
                                for (int x = 0; x < targetindex + 1; x++)
                                {
                                    Item tmpitem = filteredItemsList[x];
                                    var dtis = db.DeliveryTicketItems.Where(s => s.DeliveryTicketId == ticket.Id && s.ItemId == tmpitem.Id).ToList();

                                    if (dtis.Count() == 0)
                                    {
                                        DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                                        ticketItem.DateCreated = DateTime.Now;
                                        ticketItem.Item = filteredItemsList[x];
                                        ticketItem.DeliveryTicket = ticket;
                                        ticketItem.Quantity = filteredItemsList[x].Quantity;
                                        ticketItem.UnitPrice = unitprice;
                                        db.DeliveryTicketItems.Add(ticketItem);
                                    }
                                }
                            }
                            else
                            {
                                int tempCurrent = 0;
                                for (int x = 0; x < targetindex; x++)
                                {
                                    Item tmpitem = filteredItemsList[x];
                                    var dtis = db.DeliveryTicketItems.Where(s => s.DeliveryTicketId == ticket.Id && s.ItemId == tmpitem.Id).ToList();

                                    if (dtis.Count() == 0)
                                    {
                                        if (filteredItemsList[x].Quantity.HasValue)
                                        {
                                            tempCurrent = tempCurrent + filteredItemsList[x].Quantity.Value;
                                        }
                                        DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                                        ticketItem.DateCreated = DateTime.Now;
                                        ticketItem.Item = filteredItemsList[x];
                                        ticketItem.DeliveryTicket = ticket;
                                        ticketItem.Quantity = filteredItemsList[x].Quantity;
                                        ticketItem.UnitPrice = unitprice;
                                        db.DeliveryTicketItems.Add(ticketItem);
                                    }
                                }

                                Item tmpitemm = filteredItemsList[targetindex];
                                var dtiss = db.DeliveryTicketItems.Where(s => s.DeliveryTicketId == ticket.Id && s.ItemId == tmpitemm.Id).ToList();

                                if (dtiss.Count() == 0)
                                {
                                    DeliveryTicketItem ticketItemH = new DeliveryTicketItem();
                                    ticketItemH.DateCreated = DateTime.Now;
                                    ticketItemH.Item = filteredItemsList[targetindex];
                                    ticketItemH.DeliveryTicket = ticket;
                                    ticketItemH.Quantity = quantity - tempCurrent;
                                    ticketItemH.UnitPrice = unitprice;
                                    db.DeliveryTicketItems.Add(ticketItemH);
                                }

                            }
                            //db.SaveChanges();
                        }
                        //db.DeliveryTickets.Add(ticket);
                    }
                    db.SaveChanges();
                }

            }
            //db.SaveChanges();
        }

        public void ImportDeliveryTicketsIntoShipmentOut(int id)
        {

            List<List<string>> Output = readExcelFile(path);// 9
            List<ExcelDeliveryTicket> ModelizedOutput = ModelizeDeliveryTickets(Output);

            string prePurchaseOrderNo = "";
            string preSalesDocument = "";
            string preDelivery = "";

            int startingIndex = getStartingIndexM(ModelizedOutput, "purchaseOrderNo", "Purchase order no.");//ITEM
            int endingIndex = ModelizedOutput.Count;
            double ratio = (double)100 / (double)endingIndex;

            var items = db.Items.ToList();
            for (int i = startingIndex; i < endingIndex; i++)//3
            {
                string purchaseOrderNo = ModelizedOutput[i].purchaseOrderNo;//
                string salesDocument = ModelizedOutput[i].salesDocument;//
                string delivery = ModelizedOutput[i].delivery;//
                string partNumber = ModelizedOutput[i].partNumber;
                string description = ModelizedOutput[i].description;
                int quantity = ModelizedOutput[i].quantity;
                double unitprice = ModelizedOutput[i].unitprice;
                double total = ModelizedOutput[i].total;
                System.Diagnostics.Debug.WriteLine("Importing data :  " + i * ratio + " %");

                if (isNull(purchaseOrderNo) && isNull(salesDocument) &&
                    isNull(delivery) && !isNull(partNumber) && !isNull(description) &&
                    !isNull(quantity) && isNull(unitprice) && !isNull(total))
                {
                    purchaseOrderNo = prePurchaseOrderNo;
                    salesDocument = preSalesDocument;
                    delivery = preDelivery;
                }
                else
                {
                    prePurchaseOrderNo = purchaseOrderNo;
                    preSalesDocument = salesDocument;
                    preDelivery = delivery;
                }


                if (!DTNumberExists(delivery))
                {
                    DeliveryTicket ticket = new DeliveryTicket();
                    ticket.DateCreated = DateTime.Now;
                    ticket.DTNumber = delivery;
                    ticket.PONumber = purchaseOrderNo;
                    ticket.SONumber = salesDocument;
                    ticket.ShipmentOutId = id;

                    var availableitems = items.Where(s => s.PartNumber == partNumber && s.Quantity > s.DeliveryTicketItems.Sum(d => d.Quantity)).ToList();

                    if (availableitems.Count > 0)
                    {
                        var selectedItem = availableitems.First();
                        var filteredItemsList = availableitems;

                        if (selectedItem.Quantity >= Convert.ToInt32(quantity))
                        {
                            DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                            ticketItem.DateCreated = DateTime.Now;
                            ticketItem.Item = selectedItem;
                            ticketItem.DeliveryTicket = ticket;
                            ticketItem.Quantity = quantity;
                            ticketItem.UnitPrice = unitprice;
                            db.DeliveryTicketItems.Add(ticketItem);
                        }
                        else
                        {
                            int target = quantity;
                            int current = 0;
                            int targetindex = 0;
                            for (int j = 0; j < filteredItemsList.Count; j++)
                            {
                                if (filteredItemsList[j].Quantity.HasValue)
                                {
                                    current = current + filteredItemsList[j].Quantity.Value;
                                }
                                if (current >= target)
                                {
                                    targetindex = j;
                                    break;
                                }
                            }


                            if (current == target)
                            {
                                for (int x = 0; x < targetindex + 1; x++)
                                {
                                    DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                                    ticketItem.DateCreated = DateTime.Now;
                                    ticketItem.Item = filteredItemsList[x];
                                    ticketItem.DeliveryTicket = ticket;
                                    ticketItem.Quantity = filteredItemsList[x].Quantity;
                                    ticketItem.UnitPrice = unitprice;
                                    db.DeliveryTicketItems.Add(ticketItem);
                                }
                            }
                            else
                            {
                                int tempCurrent = 0;
                                for (int x = 0; x < targetindex; x++)
                                {
                                    if (filteredItemsList[x].Quantity.HasValue)
                                    {
                                        tempCurrent = tempCurrent + filteredItemsList[x].Quantity.Value;
                                    }
                                    DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                                    ticketItem.DateCreated = DateTime.Now;
                                    ticketItem.Item = filteredItemsList[x];
                                    ticketItem.DeliveryTicket = ticket;
                                    ticketItem.Quantity = filteredItemsList[x].Quantity;
                                    ticketItem.UnitPrice = unitprice;
                                    db.DeliveryTicketItems.Add(ticketItem);
                                }

                                DeliveryTicketItem ticketItemH = new DeliveryTicketItem();
                                ticketItemH.DateCreated = DateTime.Now;
                                ticketItemH.Item = filteredItemsList[targetindex];
                                ticketItemH.DeliveryTicket = ticket;
                                ticketItemH.Quantity = quantity - tempCurrent;
                                ticketItemH.UnitPrice = unitprice;
                                db.DeliveryTicketItems.Add(ticketItemH);

                            }
                        }

                    }
                    if (!isNull(delivery) && !isNull(purchaseOrderNo) && !isNull(salesDocument) )
                    {
                        db.DeliveryTickets.Add(ticket);// moved out of the available items condition
                        db.SaveChanges();
                    }
                }
                else
                {
                    var tickets = db.DeliveryTickets.ToList();
                    tickets = tickets.Where(s => s.DTNumber == delivery).ToList(); // delivery
                    var ticket = tickets.First();

                    var availableitems = items.Where(s => s.PartNumber == partNumber && s.Quantity > s.DeliveryTicketItems.Sum(d => d.Quantity)).ToList();

                    if (availableitems.Count > 0)
                    {
                        var selectedItem = availableitems.ToList().First();
                        var filteredItemsList = availableitems.ToList();


                        if (selectedItem.Quantity >= quantity)
                        {
                            var dtis = db.DeliveryTicketItems.Where(s => s.DeliveryTicketId == ticket.Id && s.ItemId == selectedItem.Id).ToList();

                            if (dtis.Count() == 0)
                            {
                                DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                                ticketItem.DateCreated = DateTime.Now;
                                ticketItem.Item = selectedItem;
                                ticketItem.DeliveryTicket = ticket;
                                ticketItem.Quantity = quantity;
                                ticketItem.UnitPrice = unitprice;
                                db.DeliveryTicketItems.Add(ticketItem);
                            }
                        }
                        else
                        {
                            int target = quantity;
                            int current = 0;
                            int targetindex = 0;
                            for (int j = 0; j < filteredItemsList.Count; j++)
                            {
                                if (filteredItemsList[j].Quantity.HasValue)
                                {
                                    current = current + filteredItemsList[j].Quantity.Value;
                                }
                                if (current >= target)
                                {
                                    targetindex = j;
                                    break;
                                }
                            }


                            if (current == target)
                            {
                                for (int x = 0; x < targetindex + 1; x++)
                                {
                                    Item tmpitem = filteredItemsList[x];
                                    var dtis = db.DeliveryTicketItems.Where(s => s.DeliveryTicketId == ticket.Id && s.ItemId == tmpitem.Id).ToList();

                                    if (dtis.Count() == 0)
                                    {
                                        DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                                        ticketItem.DateCreated = DateTime.Now;
                                        ticketItem.Item = filteredItemsList[x];
                                        ticketItem.DeliveryTicket = ticket;
                                        ticketItem.Quantity = filteredItemsList[x].Quantity;
                                        ticketItem.UnitPrice = unitprice;
                                        db.DeliveryTicketItems.Add(ticketItem);
                                    }
                                }
                            }
                            else
                            {
                                int tempCurrent = 0;
                                for (int x = 0; x < targetindex; x++)
                                {
                                    Item tmpitem = filteredItemsList[x];
                                    var dtis = db.DeliveryTicketItems.Where(s => s.DeliveryTicketId == ticket.Id && s.ItemId == tmpitem.Id).ToList();

                                    if (dtis.Count() == 0)
                                    {
                                        if (filteredItemsList[x].Quantity.HasValue)
                                        {
                                            tempCurrent = tempCurrent + filteredItemsList[x].Quantity.Value;
                                        }
                                        DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                                        ticketItem.DateCreated = DateTime.Now;
                                        ticketItem.Item = filteredItemsList[x];
                                        ticketItem.DeliveryTicket = ticket;
                                        ticketItem.Quantity = filteredItemsList[x].Quantity;
                                        ticketItem.UnitPrice = unitprice;
                                        db.DeliveryTicketItems.Add(ticketItem);
                                    }
                                }

                                Item tmpitemm = filteredItemsList[targetindex];
                                var dtiss = db.DeliveryTicketItems.Where(s => s.DeliveryTicketId == ticket.Id && s.ItemId == tmpitemm.Id).ToList();

                                if (dtiss.Count() == 0)
                                {
                                    DeliveryTicketItem ticketItemH = new DeliveryTicketItem();
                                    ticketItemH.DateCreated = DateTime.Now;
                                    ticketItemH.Item = filteredItemsList[targetindex];
                                    ticketItemH.DeliveryTicket = ticket;
                                    ticketItemH.Quantity = quantity - tempCurrent;
                                    ticketItemH.UnitPrice = unitprice;
                                    db.DeliveryTicketItems.Add(ticketItemH);
                                }

                            }
                        }
                        //db.DeliveryTickets.Add(ticket);
                    }

                }
                db.SaveChanges();

            }
            //db.SaveChanges();

        }


        public void ImportItemsOld()
        {
            List<List<string>> Output = readExcelFile(path);
   

            var divisions = db.Divisions.ToList();
            var shipmentsIn = db.ShipmentsIn.ToList();
            var items = db.Items.ToList();
            int startingIndex = getStartingIndex(Output, startingText);//ITEM
            int endingIndex = getEndingIndex(Output, startingIndex, Output[0].Count, 3);
            double ratio = (double)100 / (double)endingIndex;

            for (int i = startingIndex; i < endingIndex; i++)//3
            {
                Item item = new Item();
                item.DateCreated = DateTime.Now;
                string freezone = Output[i][1].TrimStart('0');
                string div = Output[i][2].Trim();
                string partNumber = Output[i][4];
                string description = Output[i][5];
                System.Diagnostics.Debug.WriteLine("Importing data :  " + i * ratio + " %");

                var sIns = shipmentsIn.Where(s => s.FZInNum == freezone).ToList();

                if (items.Where(s => s.ShipmentIn != null && s.ShipmentIn.FZInNum == freezone
                        && s.PartNumber == partNumber
                        && s.Description == description).ToList().Count == 0)
                {

                    if (sIns.Count == 0)
                    {
                        ShipmentIn shipin = new ShipmentIn();
                        shipin.DateCreated = DateTime.UtcNow.AddHours(2);
                        shipin.FZInNum = freezone;

                        List<Division> tmpdivisions = divisions.Where(s => s.Name == div).ToList();
                        if (tmpdivisions.Count > 0)
                        {
                            shipin.Division = tmpdivisions.First();
                        }
                        else
                        {
                            Division d = new Division();
                            d.DateCreated = DateTime.Now;
                            d.Name = div;
                            shipin.Division = d;

                            divisions.Add(d);
                        }
                        shipmentsIn.Add(shipin);
                        sIns = shipmentsIn.Where(s => s.FZInNum == freezone).ToList();
                    }


                    if (Output[i][1] != "")
                    {
                        item.ShipmentIn = sIns.First();
                    }
                    int quantity = 0;
                    if (int.TryParse(Output[i][3], out quantity))
                    {
                        item.Quantity = quantity;
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
                else if (items.Where(s => s.ShipmentIn != null && s.ShipmentIn.FZInNum == freezone
                        && s.PartNumber == partNumber
                        && s.Description == description).ToList().Count != 0)
                {

                    item = items.Where(s => s.ShipmentIn != null && s.ShipmentIn.FZInNum == freezone
                        && s.PartNumber == partNumber
                        && s.Description == description).ToList()[0];

                    int quantity = 0;
                    if (int.TryParse(Output[i][3], out quantity))
                    {
                        item.Quantity = quantity;
                    }



                }



            }
            db.SaveChanges();
        }

        public void ImportShipmentsInOld()
        {
            List<List<string>> Output = readExcelFile(path);// 15

            var divisions = db.Divisions.ToList();
            var shipmentsIn = db.ShipmentsIn.ToList();
            int startingIndex = getStartingIndex(Output, startingText);//FZ IN
            int endingIndex = getEndingIndex(Output, startingIndex, Output[0].Count, 3);
            double ratio = (double)100 / (double)endingIndex;

            for (int i = startingIndex; i < endingIndex; i++)
            {
                if (Output[i][1] != "" && Output[i][2] != "" && shipmentsIn.Where(s => s.FZInNum == Output[i][1]).Count() == 0)
                {
                    ShipmentIn ship = new ShipmentIn();
                    ship.DateCreated = DateTime.Now;
                    System.Diagnostics.Debug.WriteLine("Importing data :  " + i * ratio + " %");

                    if (Output[i][1] != "")
                    {
                        ship.FZInNum = Output[i][1];
                    }
                    if (Output[i][2] != "")
                    {
                        ship.CommercialInvoiceNum = Output[i][2];
                    }
                    if (Output[i][3] != "")
                    {
                        ship.AWBBOL = Output[i][3];
                    }



                    if (Output[i][4] == "AIR")
                    {
                        ship.FreightType = ShipmentInFreightType.Air;
                    }
                    else if (Output[i][4] == "LAND")
                    {
                        ship.FreightType = ShipmentInFreightType.Land;
                    }
                    else if (Output[i][4] == "OCEAN")
                    {
                        ship.FreightType = ShipmentInFreightType.Ocean;
                    }
                    if (Output[i][5] != "")
                    {
                        ship.DocReceivedDate = Convert.ToDateTime(Output[i][5]);
                    }

                    if (Output[i][6] != "")
                    {
                        string div = Output[i][6];
                        List<Division> tmpdivisions = divisions.Where(s => s.Name == div).ToList();
                        if (tmpdivisions.Count > 0)
                        {
                            ship.Division = tmpdivisions.First();//needs checkup
                        }
                        else
                        {
                            Division d = new Division();
                            d.DateCreated = DateTime.Now;
                            d.Name = div;
                            ship.Division = d;

                            divisions.Add(d);
                        }
                    }



                    if (Output[i][7] == "COMPLETED")
                    {
                        ship.Status = ShipmentInStatus.Complete;
                    }
                    else if (Output[i][7] == "WAITING")
                    {
                        ship.Status = ShipmentInStatus.Waiting;
                    }
                    else if (Output[i][7] == "CANCELED")
                    {
                        ship.Status = ShipmentInStatus.Canceled;
                    }

                    if (Output[i][9] != "")
                    {
                        if (Output[i][9].Contains("$"))
                        {
                            string[] s = Output[i][9].Split('$');
                            ship.TotalCost = Convert.ToDouble(s[1]);
                        }
                        else
                        {
                            ship.TotalCost = Convert.ToDouble(Output[i][9]);
                        }
                    }
                    if (Output[i][11] != "")
                    {
                        if (Output[i][11].Contains("$"))
                        {
                            string[] s = Output[i][11].Split('$');
                            ship.Insurance = Convert.ToDouble(s[1]);
                        }
                        else
                        {
                            ship.Insurance = Convert.ToDouble(Output[i][11]);
                        }
                    }

                    db.ShipmentsIn.Add(ship);

                }
            }
            db.SaveChanges();
        }

        public void ImportItemsIntoShipmentInOld(int id)
        {

            List<List<string>> Output = readExcelFile(path);// 10


            var items = db.Items.ToList();
            int startingIndex = getStartingIndex(Output, startingText);//ITEM
            int endingIndex = getEndingIndex(Output, startingIndex, Output[0].Count, 3);
            double ratio = (double)100 / (double)endingIndex;

            var shipmentsIn = db.ShipmentsIn.Where(s => s.Id == id).ToList();
            bool itemsInserted = false;
            if (shipmentsIn.Count > 0)
            {
                for (int i = startingIndex; i < endingIndex; i++)//3
                {
                    Item item = new Item();
                    item.DateCreated = DateTime.Now;
                    string freezone = Output[i][1].TrimStart('0');
                    string partNumber = Output[i][4];
                    string description = Output[i][5];
                    System.Diagnostics.Debug.WriteLine("Importing data :  " + i * ratio + " %");


                    //var shipmentsIn = db.ShipmentsIn.Where(s => s.FZInNum == freezone).ToList();
                    var itemsIdentical = items.Where(s => (s.ShipmentIn == null || (s.ShipmentIn != null && s.ShipmentIn.FZInNum == freezone))
                            && s.PartNumber == partNumber
                            && s.Description == description).ToList();

                    if (shipmentsIn.First().FZInNum == freezone
                        && itemsIdentical.Count == 0)
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
                        itemsInserted = true;
                    }



                }
            }

            if (itemsInserted)
            {
                shipmentsIn.First().Status = ShipmentInStatus.Complete;
            }

            db.SaveChanges();
        }

        public void ImportShipmentsOutOld()
        {

            List<List<string>> Output = readExcelFile(path);// 13

            int startingIndex = getStartingIndex(Output, startingText);//ITEM
            int endingIndex = getEndingIndex(Output, startingIndex, Output[0].Count, 3);
            double ratio = (double)100 / (double)endingIndex;

            var currentShipmentOutID = 0;
            var items = db.Items.ToList();
            for (int i = startingIndex; i < endingIndex; i++)//3
            {
                string invoice = Output[i][0];
                string dt = Output[i][1];
                string company = Output[i][2];
                string owner = Output[i][3];
                string cdn = Output[i][4];
                string shahada = Output[i][5];
                string kasima = Output[i][6];
                string export = Output[i][7];
                string totalINV = Output[i][8];
                string sendDate = Output[i][9];
                string rcvd = Output[i][10];
                string status = Output[i][11];
                string notes = Output[i][12];
                System.Diagnostics.Debug.WriteLine("Importing data :  " + i * ratio + " %");

                if (invoice == "" && dt != "" && company == "" && owner == ""
                    && cdn == "" && shahada == "" && kasima == "" && export == ""
                    && totalINV == "" && sendDate == "" && rcvd == "" && status == "")
                {

                    var shipmentsout = db.ShipmentsOut.ToList();
                    shipmentsout = shipmentsout.Where(s => s.Id == currentShipmentOutID).ToList(); // delivery

                    if (shipmentsout.Count > 0)
                    {
                        var shipmentOut = shipmentsout.First();

                        if (DTNumberExists(dt))
                        {
                            var tickets = db.DeliveryTickets.ToList();
                            tickets = tickets.Where(s => s.DTNumber == dt).ToList(); // delivery
                            var ticket = tickets.First();
                            ticket.ShipmentOutId = shipmentOut.Id;

                        }
                        else
                        {
                            DeliveryTicket ticket = new DeliveryTicket();
                            ticket.DateCreated = DateTime.Now;
                            ticket.DTNumber = dt;
                            ticket.ShipmentOutId = shipmentOut.Id;
                            db.DeliveryTickets.Add(ticket);

                        }
                    }




                }
                else
                {

                    if (!shipmentsExists(invoice, dt))
                    {
                        if (invoice != "")
                        {

                            ShipmentOut shipmentOut = new ShipmentOut();
                            shipmentOut.DateCreated = DateTime.Now;

                            shipmentOut.InvoiceNumber = invoice;
                            if (DTNumberExists(dt))
                            {
                                var tickets = db.DeliveryTickets.ToList();
                                tickets = tickets.Where(s => s.DTNumber == dt).ToList(); // delivery
                                var ticket = tickets.First();
                                ticket.ShipmentOutId = shipmentOut.Id;
                                //shipmentOut.DeliveryTickets.Add(ticket);
                            }
                            else
                            {
                                DeliveryTicket ticket = new DeliveryTicket();
                                ticket.DateCreated = DateTime.Now;
                                ticket.DTNumber = dt;
                                ticket.ShipmentOutId = shipmentOut.Id;
                                db.DeliveryTickets.Add(ticket);
                                //shipmentOut.DeliveryTickets.Add(ticket);
                            }

                            if (ownerCompanyExists(owner))
                            {

                                shipmentOut.OwnerCompany = db.OwnerCompanies.Single(a => a.Name == owner);
                            }
                            else
                            {
                                OwnerCompany ownerC = new OwnerCompany();
                                ownerC.DateCreated = DateTime.Now;
                                ownerC.Name = owner;
                                shipmentOut.OwnerCompany = ownerC;
                            }

                            if (serviceCompanyExists(company))
                            {
                                shipmentOut.ServiceCompany = db.ServiceCompanies.Single(a => a.Name == company);
                            }
                            else
                            {
                                ServiceCompany serviceC = new ServiceCompany();
                                serviceC.DateCreated = DateTime.Now;
                                serviceC.Name = company;
                                shipmentOut.ServiceCompany = serviceC;
                            }
                            shipmentOut.CDNumber = cdn;
                            shipmentOut.ShahadaNumber = shahada;
                            shipmentOut.KasimaNumber = kasima;
                            shipmentOut.ExportNumber = export;
                            //total
                            int sendDateNum = 0;
                            if (int.TryParse(sendDate, out sendDateNum))
                            {
                                shipmentOut.SendDate = FromExcelSerialDate(sendDateNum);
                            }
                            int rcvdDateNum = 0;
                            if (int.TryParse(rcvd, out rcvdDateNum))
                            {
                                shipmentOut.ReceivedDate = FromExcelSerialDate(rcvdDateNum);
                            }
                            shipmentOut.Status = status;
                            shipmentOut.Notes = notes;
                            db.ShipmentsOut.Add(shipmentOut);
                            db.SaveChanges();
                            currentShipmentOutID = shipmentOut.Id;
                        }
                    }


                }

            }
            db.SaveChanges();
        }

        public void ImportDeliveryTicketsOld()
        {

            List<List<string>> Output = readExcelFile(path);// 9

            string prePurchaseOrderNo = "";
            string preSalesDocument = "";
            string preDelivery = "";

            int startingIndex = getStartingIndex(Output, startingText);//ITEM
            int endingIndex = getEndingIndex(Output, startingIndex, Output[0].Count, 3);
            double ratio = (double)100 / (double)endingIndex;

            var items = db.Items.ToList();

            for (int i = startingIndex; i < endingIndex; i++)//3
            {
                string purchaseOrderNo = Output[i][1];//
                string salesDocument = Output[i][2];//
                string delivery = Output[i][3];//
                string partNumber = Output[i][4];
                string description = Output[i][5];
                string quantity = Output[i][6];
                string unitprice = Output[i][7];
                string total = Output[i][8];
                System.Diagnostics.Debug.WriteLine("Importing data :  " + i * ratio + " %");

                if (purchaseOrderNo == "" && salesDocument == "" &&
                    delivery == "" && partNumber != "" && description != "" &&
                    quantity != "" && unitprice != "" && total != "")
                {
                    purchaseOrderNo = prePurchaseOrderNo;
                    salesDocument = preSalesDocument;
                    delivery = preDelivery;
                }
                else
                {
                    prePurchaseOrderNo = purchaseOrderNo;
                    preSalesDocument = salesDocument;
                    preDelivery = delivery;
                }


                if (!DTNumberExists(delivery)) //dts.Where(s=>s.DTNumber == delivery).Count() == 0
                {
                    DeliveryTicket ticket = new DeliveryTicket();
                    ticket.DateCreated = DateTime.Now;
                    ticket.DTNumber = delivery;
                    ticket.PONumber = purchaseOrderNo;
                    ticket.SONumber = salesDocument;


                    var availableitems = items.Where(s => s.PartNumber == partNumber && s.Quantity > s.DeliveryTicketItems.Sum(d => d.Quantity)).ToList();

                    if (availableitems.Count > 0)
                    {
                        var selectedItem = availableitems.First();
                        var filteredItemsList = availableitems;

                        if (selectedItem.Quantity >= Convert.ToInt32(quantity))
                        {
                            DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                            ticketItem.DateCreated = DateTime.Now;
                            ticketItem.Item = selectedItem;
                            ticketItem.DeliveryTicket = ticket;
                            ticketItem.Quantity = Convert.ToInt32(quantity);
                            ticketItem.UnitPrice = Convert.ToDouble(unitprice);
                            db.DeliveryTicketItems.Add(ticketItem);
                        }
                        else
                        {
                            int target = Convert.ToInt32(quantity);
                            int current = 0;
                            int targetindex = 0;
                            for (int j = 0; j < filteredItemsList.Count; j++)
                            {
                                if (filteredItemsList[j].Quantity.HasValue)
                                {
                                    current = current + filteredItemsList[j].Quantity.Value;
                                }
                                if (current >= target)
                                {
                                    targetindex = j;
                                    break;
                                }
                            }


                            if (current == target)
                            {
                                for (int x = 0; x < targetindex + 1; x++)
                                {
                                    DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                                    ticketItem.DateCreated = DateTime.Now;
                                    ticketItem.Item = filteredItemsList[x];
                                    ticketItem.DeliveryTicket = ticket;
                                    ticketItem.Quantity = filteredItemsList[x].Quantity;
                                    ticketItem.UnitPrice = Convert.ToDouble(unitprice);
                                    db.DeliveryTicketItems.Add(ticketItem);
                                }
                            }
                            else
                            {
                                int tempCurrent = 0;
                                for (int x = 0; x < targetindex; x++)
                                {
                                    if (filteredItemsList[x].Quantity.HasValue)
                                    {
                                        tempCurrent = tempCurrent + filteredItemsList[x].Quantity.Value;
                                    }
                                    DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                                    ticketItem.DateCreated = DateTime.Now;
                                    ticketItem.Item = filteredItemsList[x];
                                    ticketItem.DeliveryTicket = ticket;
                                    ticketItem.Quantity = filteredItemsList[x].Quantity;
                                    ticketItem.UnitPrice = Convert.ToDouble(unitprice);
                                    db.DeliveryTicketItems.Add(ticketItem);
                                }

                                DeliveryTicketItem ticketItemH = new DeliveryTicketItem();
                                ticketItemH.DateCreated = DateTime.Now;
                                ticketItemH.Item = filteredItemsList[targetindex];
                                ticketItemH.DeliveryTicket = ticket;
                                ticketItemH.Quantity = Convert.ToInt32(quantity) - tempCurrent;
                                ticketItemH.UnitPrice = Convert.ToDouble(unitprice);
                                db.DeliveryTicketItems.Add(ticketItemH);

                            }
                        }
                        db.DeliveryTickets.Add(ticket);
                    }
                    db.SaveChanges();
                }
                else
                {
                    var tickets = db.DeliveryTickets.ToList();
                    tickets = tickets.Where(s => s.DTNumber == delivery).ToList(); // delivery
                    var ticket = tickets.First();

                    var availableitems = items.Where(s => s.PartNumber == partNumber && s.Quantity > s.DeliveryTicketItems.Sum(d => d.Quantity)).ToList();

                    if (availableitems.Count > 0)
                    {
                        var selectedItem = availableitems.ToList().First();
                        var filteredItemsList = availableitems.ToList();


                        if (selectedItem.Quantity >= Convert.ToInt32(quantity))
                        {
                            var dtis = db.DeliveryTicketItems.Where(s => s.DeliveryTicketId == ticket.Id && s.ItemId == selectedItem.Id).ToList();

                            if (dtis.Count() == 0)
                            {
                                DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                                ticketItem.DateCreated = DateTime.Now;
                                ticketItem.Item = selectedItem;
                                ticketItem.DeliveryTicket = ticket;
                                ticketItem.Quantity = Convert.ToInt32(quantity);
                                ticketItem.UnitPrice = Convert.ToDouble(unitprice);
                                db.DeliveryTicketItems.Add(ticketItem);
                            }
                        }
                        else
                        {
                            int target = Convert.ToInt32(quantity);
                            int current = 0;
                            int targetindex = 0;
                            for (int j = 0; j < filteredItemsList.Count; j++)
                            {
                                if (filteredItemsList[j].Quantity.HasValue)
                                {
                                    current = current + filteredItemsList[j].Quantity.Value;
                                }
                                if (current >= target)
                                {
                                    targetindex = j;
                                    break;
                                }
                            }


                            if (current == target)
                            {
                                for (int x = 0; x < targetindex + 1; x++)
                                {
                                    Item tmpitem = filteredItemsList[x];
                                    var dtis = db.DeliveryTicketItems.Where(s => s.DeliveryTicketId == ticket.Id && s.ItemId == tmpitem.Id).ToList();

                                    if (dtis.Count() == 0)
                                    {
                                        DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                                        ticketItem.DateCreated = DateTime.Now;
                                        ticketItem.Item = filteredItemsList[x];
                                        ticketItem.DeliveryTicket = ticket;
                                        ticketItem.Quantity = filteredItemsList[x].Quantity;
                                        ticketItem.UnitPrice = Convert.ToDouble(unitprice);
                                        db.DeliveryTicketItems.Add(ticketItem);
                                    }
                                }
                            }
                            else
                            {
                                int tempCurrent = 0;
                                for (int x = 0; x < targetindex; x++)
                                {
                                    Item tmpitem = filteredItemsList[x];
                                    var dtis = db.DeliveryTicketItems.Where(s => s.DeliveryTicketId == ticket.Id && s.ItemId == tmpitem.Id).ToList();

                                    if (dtis.Count() == 0)
                                    {
                                        if (filteredItemsList[x].Quantity.HasValue)
                                        {
                                            tempCurrent = tempCurrent + filteredItemsList[x].Quantity.Value;
                                        }
                                        DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                                        ticketItem.DateCreated = DateTime.Now;
                                        ticketItem.Item = filteredItemsList[x];
                                        ticketItem.DeliveryTicket = ticket;
                                        ticketItem.Quantity = filteredItemsList[x].Quantity;
                                        ticketItem.UnitPrice = Convert.ToDouble(unitprice);
                                        db.DeliveryTicketItems.Add(ticketItem);
                                    }
                                }

                                Item tmpitemm = filteredItemsList[targetindex];
                                var dtiss = db.DeliveryTicketItems.Where(s => s.DeliveryTicketId == ticket.Id && s.ItemId == tmpitemm.Id).ToList();

                                if (dtiss.Count() == 0)
                                {
                                    DeliveryTicketItem ticketItemH = new DeliveryTicketItem();
                                    ticketItemH.DateCreated = DateTime.Now;
                                    ticketItemH.Item = filteredItemsList[targetindex];
                                    ticketItemH.DeliveryTicket = ticket;
                                    ticketItemH.Quantity = Convert.ToInt32(quantity) - tempCurrent;
                                    ticketItemH.UnitPrice = Convert.ToDouble(unitprice);
                                    db.DeliveryTicketItems.Add(ticketItemH);
                                }

                            }
                            //db.SaveChanges();
                        }
                        //db.DeliveryTickets.Add(ticket);
                    }
                    db.SaveChanges();
                }

            }
            //db.SaveChanges();
        }

        public void ImportDeliveryTicketsIntoShipmentOutOld(int id)
        {

            List<List<string>> Output = readExcelFile(path);// 9

            string prePurchaseOrderNo = "";
            string preSalesDocument = "";
            string preDelivery = "";

            int startingIndex = getStartingIndex(Output, startingText);//ITEM
            int endingIndex = getEndingIndex(Output, startingIndex, Output[0].Count, 3);
            double ratio = (double)100 / (double)endingIndex;

            var items = db.Items.ToList();
            for (int i = startingIndex; i < endingIndex; i++)//3
            {
                string purchaseOrderNo = Output[i][1];
                string salesDocument = Output[i][2];
                string delivery = Output[i][3];
                string partNumber = Output[i][4];
                string description = Output[i][5];
                string quantity = Output[i][6];
                string unitprice = Output[i][7];
                string total = Output[i][8];
                System.Diagnostics.Debug.WriteLine("Importing data :  " + i * ratio + " %");

                if (purchaseOrderNo == "" && salesDocument == "" &&
                    delivery == "" && partNumber != "" && description != "" &&
                    quantity != "" && unitprice != "" && total != "")
                {
                    purchaseOrderNo = prePurchaseOrderNo;
                    salesDocument = preSalesDocument;
                    delivery = preDelivery;
                }
                else
                {
                    prePurchaseOrderNo = purchaseOrderNo;
                    preSalesDocument = salesDocument;
                    preDelivery = delivery;
                }


                if (!DTNumberExists(delivery))
                {
                    DeliveryTicket ticket = new DeliveryTicket();
                    ticket.DateCreated = DateTime.Now;
                    ticket.DTNumber = delivery;
                    ticket.PONumber = purchaseOrderNo;
                    ticket.SONumber = salesDocument;
                    ticket.ShipmentOutId = id;

                    var availableitems = items.Where(s => s.PartNumber == partNumber && s.Quantity > s.DeliveryTicketItems.Sum(d => d.Quantity)).ToList();

                    if (availableitems.Count > 0)
                    {
                        var selectedItem = availableitems.First();
                        var filteredItemsList = availableitems;

                        if (selectedItem.Quantity >= Convert.ToInt32(quantity))
                        {
                            DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                            ticketItem.DateCreated = DateTime.Now;
                            ticketItem.Item = selectedItem;
                            ticketItem.DeliveryTicket = ticket;
                            ticketItem.Quantity = Convert.ToInt32(quantity);
                            ticketItem.UnitPrice = Convert.ToDouble(unitprice);
                            db.DeliveryTicketItems.Add(ticketItem);
                        }
                        else
                        {
                            int target = Convert.ToInt32(quantity);
                            int current = 0;
                            int targetindex = 0;
                            for (int j = 0; j < filteredItemsList.Count; j++)
                            {
                                if (filteredItemsList[j].Quantity.HasValue)
                                {
                                    current = current + filteredItemsList[j].Quantity.Value;
                                }
                                if (current >= target)
                                {
                                    targetindex = j;
                                    break;
                                }
                            }


                            if (current == target)
                            {
                                for (int x = 0; x < targetindex + 1; x++)
                                {
                                    DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                                    ticketItem.DateCreated = DateTime.Now;
                                    ticketItem.Item = filteredItemsList[x];
                                    ticketItem.DeliveryTicket = ticket;
                                    ticketItem.Quantity = filteredItemsList[x].Quantity;
                                    ticketItem.UnitPrice = Convert.ToDouble(unitprice);
                                    db.DeliveryTicketItems.Add(ticketItem);
                                }
                            }
                            else
                            {
                                int tempCurrent = 0;
                                for (int x = 0; x < targetindex; x++)
                                {
                                    if (filteredItemsList[x].Quantity.HasValue)
                                    {
                                        tempCurrent = tempCurrent + filteredItemsList[x].Quantity.Value;
                                    }
                                    DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                                    ticketItem.DateCreated = DateTime.Now;
                                    ticketItem.Item = filteredItemsList[x];
                                    ticketItem.DeliveryTicket = ticket;
                                    ticketItem.Quantity = filteredItemsList[x].Quantity;
                                    ticketItem.UnitPrice = Convert.ToDouble(unitprice);
                                    db.DeliveryTicketItems.Add(ticketItem);
                                }

                                DeliveryTicketItem ticketItemH = new DeliveryTicketItem();
                                ticketItemH.DateCreated = DateTime.Now;
                                ticketItemH.Item = filteredItemsList[targetindex];
                                ticketItemH.DeliveryTicket = ticket;
                                ticketItemH.Quantity = Convert.ToInt32(quantity) - tempCurrent;
                                ticketItemH.UnitPrice = Convert.ToDouble(unitprice);
                                db.DeliveryTicketItems.Add(ticketItemH);

                            }
                        }

                    }
                    if (delivery != "" && purchaseOrderNo != "" && salesDocument != "")
                    {
                        db.DeliveryTickets.Add(ticket);// moved out of the available items condition
                        db.SaveChanges();
                    }
                }
                else
                {
                    var tickets = db.DeliveryTickets.ToList();
                    tickets = tickets.Where(s => s.DTNumber == delivery).ToList(); // delivery
                    var ticket = tickets.First();

                    var availableitems = items.Where(s => s.PartNumber == partNumber && s.Quantity > s.DeliveryTicketItems.Sum(d => d.Quantity)).ToList();

                    if (availableitems.Count > 0)
                    {
                        var selectedItem = availableitems.ToList().First();
                        var filteredItemsList = availableitems.ToList();


                        if (selectedItem.Quantity >= Convert.ToInt32(quantity))
                        {
                            var dtis = db.DeliveryTicketItems.Where(s => s.DeliveryTicketId == ticket.Id && s.ItemId == selectedItem.Id).ToList();

                            if (dtis.Count() == 0)
                            {
                                DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                                ticketItem.DateCreated = DateTime.Now;
                                ticketItem.Item = selectedItem;
                                ticketItem.DeliveryTicket = ticket;
                                ticketItem.Quantity = Convert.ToInt32(quantity);
                                ticketItem.UnitPrice = Convert.ToDouble(unitprice);
                                db.DeliveryTicketItems.Add(ticketItem);
                            }
                        }
                        else
                        {
                            int target = Convert.ToInt32(quantity);
                            int current = 0;
                            int targetindex = 0;
                            for (int j = 0; j < filteredItemsList.Count; j++)
                            {
                                if (filteredItemsList[j].Quantity.HasValue)
                                {
                                    current = current + filteredItemsList[j].Quantity.Value;
                                }
                                if (current >= target)
                                {
                                    targetindex = j;
                                    break;
                                }
                            }


                            if (current == target)
                            {
                                for (int x = 0; x < targetindex + 1; x++)
                                {
                                    Item tmpitem = filteredItemsList[x];
                                    var dtis = db.DeliveryTicketItems.Where(s => s.DeliveryTicketId == ticket.Id && s.ItemId == tmpitem.Id).ToList();

                                    if (dtis.Count() == 0)
                                    {
                                        DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                                        ticketItem.DateCreated = DateTime.Now;
                                        ticketItem.Item = filteredItemsList[x];
                                        ticketItem.DeliveryTicket = ticket;
                                        ticketItem.Quantity = filteredItemsList[x].Quantity;
                                        ticketItem.UnitPrice = Convert.ToDouble(unitprice);
                                        db.DeliveryTicketItems.Add(ticketItem);
                                    }
                                }
                            }
                            else
                            {
                                int tempCurrent = 0;
                                for (int x = 0; x < targetindex; x++)
                                {
                                    Item tmpitem = filteredItemsList[x];
                                    var dtis = db.DeliveryTicketItems.Where(s => s.DeliveryTicketId == ticket.Id && s.ItemId == tmpitem.Id).ToList();

                                    if (dtis.Count() == 0)
                                    {
                                        if (filteredItemsList[x].Quantity.HasValue)
                                        {
                                            tempCurrent = tempCurrent + filteredItemsList[x].Quantity.Value;
                                        }
                                        DeliveryTicketItem ticketItem = new DeliveryTicketItem();
                                        ticketItem.DateCreated = DateTime.Now;
                                        ticketItem.Item = filteredItemsList[x];
                                        ticketItem.DeliveryTicket = ticket;
                                        ticketItem.Quantity = filteredItemsList[x].Quantity;
                                        ticketItem.UnitPrice = Convert.ToDouble(unitprice);
                                        db.DeliveryTicketItems.Add(ticketItem);
                                    }
                                }

                                Item tmpitemm = filteredItemsList[targetindex];
                                var dtiss = db.DeliveryTicketItems.Where(s => s.DeliveryTicketId == ticket.Id && s.ItemId == tmpitemm.Id).ToList();

                                if (dtiss.Count() == 0)
                                {
                                    DeliveryTicketItem ticketItemH = new DeliveryTicketItem();
                                    ticketItemH.DateCreated = DateTime.Now;
                                    ticketItemH.Item = filteredItemsList[targetindex];
                                    ticketItemH.DeliveryTicket = ticket;
                                    ticketItemH.Quantity = Convert.ToInt32(quantity) - tempCurrent;
                                    ticketItemH.UnitPrice = Convert.ToDouble(unitprice);
                                    db.DeliveryTicketItems.Add(ticketItemH);
                                }

                            }
                        }
                        //db.DeliveryTickets.Add(ticket);
                    }

                }
                db.SaveChanges();

            }
            //db.SaveChanges();

        }


        //End imports

        public List<List<string>> readExcelFile(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(fs, false))
                {
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    SharedStringTablePart sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                    SharedStringTable sst = sstpart.SharedStringTable;

                    int numnum = workbookPart.WorksheetParts.Count() - 1;
                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.ElementAt(numnum);
                    Worksheet sheet = worksheetPart.Worksheet;

                   // var cells = sheet.Descendants<Cell>();
                    var rows = sheet.Descendants<Row>();

                    int numberOfrows = rows.Count();
                    int numberOfcolumns = getColumnsCount(rows);

                    List<List<string>> Output = new List<List<string>>();
                    initLists(Output, numberOfrows, numberOfcolumns);
                    double ratio = (double)100 / (double)rows.LongCount();

                 

                    // One way: go through each cell in the sheet
                    //foreach (Cell cell in cells)
                    //{
                    //    string val = "";
                    //    if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
                    //    {
                    //        int ssid = int.Parse(cell.CellValue.Text);
                    //        string str = sst.ChildElements[ssid].InnerText;
                    //        //System.Diagnostics.Debug.WriteLine("Shared string {0}: {1}", ssid, str);
                    //        System.Diagnostics.Debug.WriteLine(str);
                    //        val = str;
                    //    }
                    //    else if (cell.CellValue != null)
                    //    {
                    //        //System.Diagnostics.Debug.WriteLine("Cell contents: {0}", cell.CellValue.Text);
                    //        System.Diagnostics.Debug.WriteLine(cell.CellValue.Text);
                    //        val = cell.CellValue.Text;
                    //    }
                    //    if(val != "")
                    //    {
                    //        colsL.Add(val);
                    //        Output.Add(colsL);
                    //        colsL = new List<string>();
                    //    }
                    //}
                    //return Output;

                    int currentRow = 0;
                    // Or... via each row
                    foreach (Row row in rows)
                    {

                        foreach (Cell c in row.Elements<Cell>())
                        {
                            string val = "";
                            string columnName = GetColumnName(c.CellReference);
                            int currentColumnIndex = ConvertColumnNameToNumber(columnName);

                            if ((c.DataType != null) && (c.DataType == CellValues.SharedString))
                            {
                                int ssid = int.Parse(c.CellValue.Text);
                                string str = sst.ChildElements[ssid].InnerText;
                                val = str;
                          
                            }
                            else if (c.CellValue != null)
                            {
                                val = c.CellValue.Text;
                          
                            }
                            Output[currentRow][currentColumnIndex] = val;
                        }
                    
                        currentRow++;
                        System.Diagnostics.Debug.WriteLine("Reading file :  " + currentRow * ratio + " %");
                       
                    
                    }
                    return Output;
                }
            }
        }


        #region helper functions
        // Start helper functions

        private void initLists(List<List<string>> Output,int numberOfrows,int numberOfColumns)
        {
            for(int i=0;i<numberOfrows;i++)
            {
                List<string> colsL = new List<string>();
                for (int j = 0; j < numberOfColumns; j++)
                {
                    colsL.Add("");
                }
                Output.Add(colsL);
            }
           
        }

        public Boolean isEmpty(string inp)
        {
            Boolean output = false;
            if (inp == "")
            {
                output = true;
            }
            return output;

        }
        public Boolean isNull(object inp)
        {
            Boolean output = false;
            if(inp == null)
            {
                output = true;
            }
            return output;
        }
        private int getColumnsCount(IEnumerable<Row> rows)
        {
            int output = 0;
            foreach (Row row in rows)
            {
                if(row.Count() > output)
                {
                    output = row.Count();
                }
            }

            return output;
        }

        private int getStartingIndex(List<List<string>> Input, string checkFor)
        {
            int output = 0;
            for (int i = 0; i < Input.Count; i++)
            {
                if (Input[i][0] == checkFor)
                {
                    return i + 1;
                }
            }
            return output;

        }
        private int getEndingIndex(List<List<string>> Input, int startingIndex, int numberOfColumns, int range)
        {
            int output = Input.Count;
            int tcount = 0;
            for (int i = startingIndex; i < Input.Count; i++)
            {
                if (i == 1465)
                {

                }
                for (int j = 0; j < numberOfColumns; j++)
                {
                    if (Input[i][j] != "")
                    {
                        break;
                    }
                    if (j == numberOfColumns - 1)
                    {
                        if (tcount < range)
                        {
                            tcount++;
                        }
                        else
                        {
                            return i;
                        }
                    }
                }
            }
            return output;
        }

        private int getStartingIndexM(IEnumerable<object> Input, string compareWith, string checkFor)
        {
           
            string type = Input.GetType().ToString().Split('.')[6].Split(']')[0];
            int output = 0;
            switch(type)
            {
                case "ExcelItem": 
                    List<ExcelItem> items = Input.Cast<ExcelItem>().ToList();
                    
                    for (int i = 0; i < items.Count; i++)
                    {
                        switch(compareWith)
                        {
                            case "FZInNum":
                            if (items[i].FZInNum == checkFor)
                            {
                                return i + 1;
                            }
                            break;
                            case "DivisionName":
                            if (items[i].DivisionName == checkFor)
                            {
                                return i + 1;
                            }
                            break;
                            case "InQuantity":
                            if (items[i].InQuantity.ToString() == checkFor)
                            {
                                return i + 1;
                            }
                            break;
                            case "PartNumber":
                            if (items[i].PartNumber == checkFor)
                            {
                                return i + 1;
                            }
                            break;
                            case "Description":
                            if (items[i].Description == checkFor)
                            {
                                return i + 1;
                            }
                            break;
                            case "OutQuantity":
                            if (items[i].OutQuantity.ToString() == checkFor)
                            {
                                return i + 1;
                            }
                            break;
                        }
                       
                    }
                    break;
                case "ExcelShipmentIn": 
                    List<ExcelShipmentIn> shipmentsIn = Input.Cast<ExcelShipmentIn>().ToList();
                    for (int i = 0; i < shipmentsIn.Count; i++)
                    {
                        switch (compareWith)
                        {
                            case "FZInNum":
                                if (shipmentsIn[i].FZInNum == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "CommercialInvoiceNum":
                                if (shipmentsIn[i].CommercialInvoiceNum == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "AWBBOL":
                                if (shipmentsIn[i].AWBBOL == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "FreightType":
                                if (shipmentsIn[i].FreightType.ToString() == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "DocReceivedDate":
                                if (shipmentsIn[i].DocReceivedDate.ToString() == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "DivisionName":
                                if (shipmentsIn[i].DivisionName == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "Status":
                                if (shipmentsIn[i].Status.ToString() == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "TotalCost":
                                if (shipmentsIn[i].TotalCost.ToString() == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "Insurance":
                                if (shipmentsIn[i].Insurance.ToString() == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                        }
                    }
                    break;
                case "ExcelShipmentOut": 
                    List<ExcelShipmentOut> shipmentsOut = Input.Cast<ExcelShipmentOut>().ToList();
                    for (int i = 0; i < shipmentsOut.Count; i++)
                    {
                        switch (compareWith)
                        {
                            case "invoice":
                                if (shipmentsOut[i].invoice == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "dt":
                                if (shipmentsOut[i].dt == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "company":
                                if (shipmentsOut[i].company == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "owner":
                                if (shipmentsOut[i].owner == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "cdn":
                                if (shipmentsOut[i].cdn == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "shahada":
                                if (shipmentsOut[i].shahada == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "kasima":
                                if (shipmentsOut[i].kasima == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "export":
                                if (shipmentsOut[i].export == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "totalINV":
                                if (shipmentsOut[i].totalINV == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "sendDate":
                                if (shipmentsOut[i].sendDate.ToString() == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "rcvd":
                                if (shipmentsOut[i].rcvd.ToString() == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "status":
                                if (shipmentsOut[i].status == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "notes":
                                if (shipmentsOut[i].notes == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                        }
                    }
                    break;
                case "ExcelDeliveryTicket": 
                    List<ExcelDeliveryTicket> deliveryTickets = Input.Cast<ExcelDeliveryTicket>().ToList();
                    for (int i = 0; i < deliveryTickets.Count; i++)
                    {
                        switch (compareWith)
                        {
                            case "purchaseOrderNo":
                                if (deliveryTickets[i].purchaseOrderNo == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "salesDocument":
                                if (deliveryTickets[i].salesDocument == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "delivery":
                                if (deliveryTickets[i].delivery == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "partNumber":
                                if (deliveryTickets[i].partNumber == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "description":
                                if (deliveryTickets[i].description == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "quantity":
                                if (deliveryTickets[i].quantity.ToString() == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "unitprice":
                                if (deliveryTickets[i].unitprice.ToString() == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                            case "total":
                                if (deliveryTickets[i].total.ToString() == checkFor)
                                {
                                    return i + 1;
                                }
                                break;
                        }
                    }
                    break;
            }
     
       
            return output;

        }
        private Boolean serviceCompanyExists(string serviceCompanyName)
        {

            var ServiceCompanies = db.ServiceCompanies.ToList();
            Boolean output = false;
            if (ServiceCompanies.Where(s => s.Name == serviceCompanyName).ToList().Count > 0)
            {
                output = true;
            }
            return output;
        }
        private Boolean shipmentsExists(string invoice, string dt)
        {
            var shipments = db.ShipmentsOut.ToList();
            Boolean found = false;
            if (shipments.Where(s => s.InvoiceNumber == invoice && s.DeliveryTickets.Where(a => a.DTNumber == dt).Count() > 0).ToList().Count > 0)
            {
                found = true;
            }
            return found;
        }
        private Boolean ownerCompanyExists(string ownerCompanyName)
        {

            var OwnerCompanies = db.OwnerCompanies.ToList();
            Boolean output = false;
            if (OwnerCompanies.Where(s => s.Name == ownerCompanyName).ToList().Count > 0)
            {
                output = true;
            }
            return output;
        }
        private Boolean DTNumberExists(string ticket)
        {
            var ticks = db.DeliveryTickets.ToList();
            Boolean output = false;
            if (ticks.Where(s => s.DTNumber == ticket).ToList().Count > 0)
            {
                output = true;
            }
            return output;
        }
        private static DateTime FromExcelSerialDate(int SerialDate)
        {
            if (SerialDate > 59) SerialDate -= 1; //Excel/Lotus 2/29/1900 bug   
            return new DateTime(1899, 12, 31).AddDays(SerialDate);
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

            //xlApp = new Excel.ApplicationClass();
            xlWorkBook = xlApp.Workbooks.Open(filename, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            int maxTabNum = xlWorkBook.Worksheets.Count;
            //if (tabNumber > maxTabNum)
            //{
            //   // System.Windows.MessageBox.Show(tabNumber + " is not a valid sheet number, please enter a number between 1 and " + maxTabNum);
            //    return;
            //}
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(tabNumber);
            range = xlWorkSheet.UsedRange;
            if (rowsNumber == 0)
            {
                rowsNumber = range.Rows.Count;
            }

            double ratio = (double)100 / (double)rowsNumber;
            for (rCnt = 1; rCnt <= rowsNumber; rCnt++)
            {
                int attCounter = 0;
                for (cCnt = 1; cCnt <= columnsNumber; cCnt++)
                {
                    str = (range.Cells[rCnt, cCnt] as Microsoft.Office.Interop.Excel.Range).Value2 + "";
                    //str = str.Trim();

                    colsL.Add(str);


                    attCounter++;
                }
                Output.Add(colsL);
                colsL = new List<string>();
                epcount++;
                System.Diagnostics.Debug.WriteLine("Reading file :  " + rCnt * ratio + " %");

            }

            // xlWorkBook.Close(true, null, null);
            xlApp.Quit();

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


        private double ConvertToDoubleSafely(string input)
        {
            double d;
            if (double.TryParse(input, out d))
            {
                return d;
            }
            return 0;
        }

   

        /// <summary>
        /// Given a cell name, parses the specified cell to get the column name.
        /// </summary>
        /// <param name="cellReference">Address of the cell (ie. B2)</param>
        /// <returns>Column Name (ie. B)</returns>
        public static string GetColumnName(string cellReference)
        {
            // Match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);

            return match.Value;
        }

        /// <summary>
        /// Given just the column name (no row index),
        /// it will return the zero based column index.
        /// </summary>
        /// <param name="columnName">Column Name (ie. A or AB)</param>
        /// <returns>Zero based index if the conversion was successful</returns>
        /// <exception cref="ArgumentException">thrown if the given string
        /// contains characters other than uppercase letters</exception>
        public static int ConvertColumnNameToNumber(string columnName)
        {
            Regex alpha = new Regex("^[A-Z]+$");
            if (!alpha.IsMatch(columnName)) throw new ArgumentException();

            char[] colLetters = columnName.ToCharArray();
            Array.Reverse(colLetters);

            int convertedValue = 0;
            for (int i = 0; i < colLetters.Length; i++)
            {
                char letter = colLetters[i];
                int current = i == 0 ? letter - 65 : letter - 64; // ASCII 'A' = 65
                convertedValue += current * (int)Math.Pow(26, i);
            }

            return convertedValue;
        }



        //End helper functions
        #endregion
    }
}