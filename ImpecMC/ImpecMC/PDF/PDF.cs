using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using ImpecMC.Models;

/// <summary>
/// Summary description for PDF
/// </summary>
public class PDF
{
    public static String CreatePDF(ShipmentOut shipmentOut, string path , string logoPath)
    {
        

        String DTTitle = "Commercial Invoice";

        Document doc = new Document();    // instantiate a iTextSharp.text.pdf.Document
        MemoryStream mem = new MemoryStream(); // PDF data will be written here
        PdfWriter.GetInstance(doc, mem);  // tie a PdfWriter instance to the stream


        string filepath = path + shipmentOut.Id + ".pdf";
        PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

        doc.Open();

        Font titlefont = FontFactory.GetFont("Arial", 12, Font.BOLD);
        Font boldfont = FontFactory.GetFont("Arial", 12, Font.BOLD);
        Font font = FontFactory.GetFont("Arial", 8, Font.NORMAL);

        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(logoPath);
        img.ScalePercent(75f);

        img.Alignment = iTextSharp.text.Image.TEXTWRAP | iTextSharp.text.Image.ALIGN_LEFT;
        img.IndentationRight = 9f;
        img.SpacingAfter = 9f;
        doc.Add(img);

        Paragraph paragraph = new Paragraph(@"Impec Free Zone", boldfont);
        paragraph.Alignment = Element.ALIGN_RIGHT;
        paragraph.SetLeading(0.0f, 1.2f);
        doc.Add(paragraph);

        paragraph = new Paragraph(@"Part #8 47 Acer
                    Area East Of El - Robiky
                    Ind.Area - Badr City
                    Cairo Egypt", font);
        paragraph.Alignment = Element.ALIGN_RIGHT;
        paragraph.SetLeading(0.0f, 1.2f);
        doc.Add(paragraph);

        doc.Add(GetInvoiceHeader(DTTitle));

        doc.Add(GetInvoiceInfoTable(shipmentOut));

        doc.Add(GetInvoiceItemsTable(shipmentOut));

        paragraph = new Paragraph(new Phrase(@"I hereby certify that the information on this delivery ticket is true and correct to the best of my knowledge        
and that the contents of this shipment are as stated above.", FontFactory.GetFont("Arial", 8, Font.BOLD)));
        paragraph.SpacingBefore = 10f;
        paragraph.Alignment = Element.ALIGN_CENTER;

        doc.Add(paragraph);

        doc.Add(GetInvoiceFooterTable(shipmentOut));

        PdfPTable table = new PdfPTable(1);
        table.WidthPercentage = 100;
        table.SpacingBefore = 10f;
        PdfPCell cell = new PdfPCell(new Phrase("Notes:", FontFactory.GetFont("Arial", 8, Font.BOLD)));
        cell.BackgroundColor = new BaseColor(233, 234, 234);
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
        cell.FixedHeight = 40;
        table.AddCell(cell);
        doc.Add(table);

        paragraph = new Paragraph(new Phrase("Note: Print two (2) copies; one for customer and one Seaharvest signed by Customer\r\nThis " + DTTitle + " Not to be Invoiced", FontFactory.GetFont("Arial", 6, Font.ITALIC)));
        paragraph.SpacingBefore = 10f;
        paragraph.Alignment = Element.ALIGN_CENTER;

        doc.Add(paragraph);

        doc.Close();   // automatically closes the attached MemoryStream

       // byte[] docData = mem.GetBuffer(); // get the generated PDF as raw data

        // write the document data to response stream and set appropriate headers:
        //Response.AppendHeader("Content-Disposition", "attachment; filename=DT-" + dt.DTNumber + ".pdf");
       // Response.ContentType = "application/pdf";
       // Response.BinaryWrite(docData);
       // Response.End();

        //Response.Buffer = false; //transmitfile self buffers
        //Response.Clear();
        //Response.ClearContent();
        //Response.ClearHeaders();
        //Response.ContentType = "application/pdf";
        //Response.AppendHeader("Content-Disposition", "attachment; filename=DT-" + dt.Serial + ".pdf");
        ////Response.TransmitFile(filepath);
        //Response.End();

        //Response.Write("<script>");
        //Response.Write("window.open('" + filepath + "');");
        //Response.Write("</script>");

        //Response.Redirect("~/Archive/preview.pdf");
        return filepath;
    }

    public static PdfPTable GetInvoiceHeader(String Title)
    {
        PdfPTable table = new PdfPTable(7);
        table.WidthPercentage = 100;
        table.DefaultCell.BorderWidth = 0;

        PdfPTable nested = new PdfPTable(1);
        PdfPCell cell1 = new PdfPCell(new Phrase(" "));
        cell1.BorderWidth = 0;
        cell1.BorderWidthBottom = 1;
        cell1.FixedHeight = 20;
        nested.AddCell(cell1);
        PdfPCell cell2 = new PdfPCell(new Phrase(" "));
        cell2.BorderWidth = 0;
        cell2.BorderWidthTop = 1;
        nested.AddCell(cell2);
        PdfPCell nesthousing = new PdfPCell(nested);
        nesthousing.BorderWidth = 0;
        nesthousing.Padding = 0f;
        table.AddCell(nesthousing);

        nested = new PdfPTable(1);
        cell1 = new PdfPCell(new Phrase(" "));
        cell1.BorderWidth = 0;
        cell1.BorderWidthBottom = 1;
        cell1.FixedHeight = 20;
        nested.AddCell(cell1);
        cell2 = new PdfPCell(new Phrase(" "));
        cell2.BorderWidth = 0;
        cell2.BorderWidthTop = 1;
        nested.AddCell(cell2);
        nesthousing = new PdfPCell(nested);
        nesthousing.BorderWidth = 0;
        nesthousing.Padding = 0f;
        table.AddCell(nesthousing);

        nested = new PdfPTable(1);
        cell1 = new PdfPCell(new Phrase(" "));
        cell1.BorderWidth = 0;
        cell1.BorderWidthBottom = 1;
        cell1.FixedHeight = 20;
        nested.AddCell(cell1);
        cell2 = new PdfPCell(new Phrase(" "));
        cell2.BorderWidth = 0;
        cell2.BorderWidthTop = 1;
        nested.AddCell(cell2);
        nesthousing = new PdfPCell(nested);
        nesthousing.BorderWidth = 0;
        nesthousing.Padding = 0f;
        table.AddCell(nesthousing);

        nested = new PdfPTable(1);
        cell1 = new PdfPCell(new Phrase(" "));
        cell1.BorderWidth = 0;
        cell1.BorderWidthBottom = 1;
        cell1.FixedHeight = 20;
        nested.AddCell(cell1);
        cell2 = new PdfPCell(new Phrase(" "));
        cell2.BorderWidth = 0;
        cell2.BorderWidthTop = 1;
        nested.AddCell(cell2);
        nesthousing = new PdfPCell(nested);
        nesthousing.BorderWidth = 0;
        nesthousing.Padding = 0f;
        table.AddCell(nesthousing);

        nested = new PdfPTable(1);
        cell1 = new PdfPCell(new Phrase(" "));
        cell1.BorderWidth = 0;
        cell1.BorderWidthBottom = 1;
        cell1.FixedHeight = 20;
        nested.AddCell(cell1);
        cell2 = new PdfPCell(new Phrase(" "));
        cell2.BorderWidth = 0;
        cell2.BorderWidthTop = 1;
        nested.AddCell(cell2);
        nesthousing = new PdfPCell(nested);
        nesthousing.BorderWidth = 0;
        nesthousing.Padding = 0f;
        table.AddCell(nesthousing);

        Font invoicefont = FontFactory.GetFont("Arial", 10, Font.BOLDITALIC);

        PdfPCell invoice = new PdfPCell(new Phrase(Title.ToUpper(), invoicefont));
        invoice.HorizontalAlignment = Element.ALIGN_CENTER;
        invoice.BorderWidth = 0;
        invoice.VerticalAlignment = Element.ALIGN_MIDDLE;
        table.AddCell(invoice);

        nested = new PdfPTable(1);
        cell1 = new PdfPCell(new Phrase(" "));
        cell1.BorderWidth = 0;
        cell1.BorderWidthBottom = 1;
        cell1.FixedHeight = 20;
        nested.AddCell(cell1);
        cell2 = new PdfPCell(new Phrase(" "));
        cell2.BorderWidth = 0;
        cell2.BorderWidthTop = 1;
        nested.AddCell(cell2);
        nesthousing = new PdfPCell(nested);
        nesthousing.BorderWidth = 0;
        nesthousing.Padding = 0f;
        table.AddCell(nesthousing);

        return table;
    }

    public static PdfPTable GetInvoiceInfoTable(ShipmentOut shout)
    {
        PdfPTable maintable = new PdfPTable(3);
        maintable.WidthPercentage = 100;
        maintable.DefaultCell.BorderWidth = 0;
        float[] widths = new float[] { 110f, 100f, 120f };
        maintable.SetWidths(widths);

        PdfPTable table = new PdfPTable(2);
        widths = new float[] { 40, 110f };
        table.SetWidths(widths);
        table.WidthPercentage = 30;
        table.HorizontalAlignment = Element.ALIGN_LEFT;


        PdfPCell cell = new PdfPCell(new Phrase("Customer:", FontFactory.GetFont("Arial", 8, Font.BOLD)));
        cell.BackgroundColor = new BaseColor(233, 234, 234);
        cell.Colspan = 2;
        table.AddCell(cell);
        cell = new PdfPCell();
        cell.FixedHeight = 40;
        cell.Colspan = 2;
        cell.AddElement(new Phrase(shout.OwnerCompany.Name, FontFactory.GetFont("Arial", 9, Font.BOLD)));
       // cell.AddElement(new Phrase(dt.DTNumber + " " + dt.DTNumber + "\r\n" + dt.DTNumber, FontFactory.GetFont("Arial", 8, Font.NORMAL)));
        table.AddCell(cell);


        PdfPCell cell2 = new PdfPCell(new Phrase("Contractor:", FontFactory.GetFont("Arial", 8, Font.BOLD)));
        cell2.BackgroundColor = new BaseColor(233, 234, 234);
        cell2.Colspan = 2;
        table.AddCell(cell2);
        cell2 = new PdfPCell();
        cell2.FixedHeight = 40;
        cell2.Colspan = 2;
        cell2.AddElement(new Phrase(shout.ServiceCompany.Name, FontFactory.GetFont("Arial", 9, Font.BOLD)));
        //cell2.AddElement(new Phrase(dt.DTNumber + " " + dt.DTNumber + "\r\n" + dt.DTNumber, FontFactory.GetFont("Arial", 8, Font.NORMAL)));
      
        //dt.ShipmentOut.ServiceCompany.Name
       
        table.AddCell(cell2);
 

        maintable.AddCell(table);

        maintable.AddCell("");

        table = new PdfPTable(2);
        table.WidthPercentage = 30;
        table.HorizontalAlignment = Element.ALIGN_RIGHT;
        widths = new float[] { 80f, 116f };
        table.SetWidths(widths);

        cell = new PdfPCell(new Phrase("Invoice No:", FontFactory.GetFont("Arial", 8, Font.BOLD)));
        cell.BackgroundColor = new BaseColor(233, 234, 234);
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase(shout.InvoiceNumber, FontFactory.GetFont("Arial", 9, Font.BOLDITALIC, BaseColor.RED)));
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("Date:", FontFactory.GetFont("Arial", 8, Font.BOLD)));
        cell.BackgroundColor = new BaseColor(233, 234, 234);
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase(DateTime.UtcNow.AddHours(2).ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", 8, Font.NORMAL)));
        table.AddCell(cell);


        cell = new PdfPCell(new Phrase(""));
        cell.BorderWidth = 0;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase(""));
        cell.BorderWidth = 0;
        table.AddCell(cell);

        maintable.AddCell(table);

        return maintable;
    }

    public static PdfPTable GetInvoiceItemsTable(ShipmentOut shout)
    {
        PdfPTable mainTable = new PdfPTable(1);
        mainTable.SpacingBefore = 10f;
        mainTable.WidthPercentage = 100;
        mainTable.DefaultCell.BorderWidth = 0;
        mainTable.DefaultCell.FixedHeight = 300f;

        int cols = 6;
        float[] widths = new float[] { 10f, 30f, 100f, 50f, 15f, 15f };//220

     

        PdfPTable table = new PdfPTable(cols);
        table.WidthPercentage = 100;
        table.SetWidths(widths);

        PdfPCell cell = new PdfPCell(new Phrase("Item", FontFactory.GetFont("Arial", 8, Font.BOLD)));
        cell.BackgroundColor = new BaseColor(233, 234, 234);
        table.AddCell(cell);

     
        cell = new PdfPCell(new Phrase("Part No.", FontFactory.GetFont("Arial", 8, Font.BOLD)));
        cell.BackgroundColor = new BaseColor(233, 234, 234);
        table.AddCell(cell);
        

        cell = new PdfPCell(new Phrase("Description", FontFactory.GetFont("Arial", 8, Font.BOLD)));
        cell.BackgroundColor = new BaseColor(233, 234, 234);
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("FZ#", FontFactory.GetFont("Arial", 8, Font.BOLD)));
        cell.BackgroundColor = new BaseColor(233, 234, 234);
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("QTY", FontFactory.GetFont("Arial", 8, Font.BOLD)));
        cell.BackgroundColor = new BaseColor(233, 234, 234);
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("Unit Price", FontFactory.GetFont("Arial", 8, Font.BOLD)));
        cell.BackgroundColor = new BaseColor(233, 234, 234);
        table.AddCell(cell);

        ////////////////////
        ////////////////////
        ////////////////////

       // List<Item> itemList = shout.DeliveryTickets;

        List<DeliveryTicket> tickets = shout.DeliveryTickets;
        for (int j = 0; j < tickets.Count; j++)
        {
            List<DeliveryTicketItem> itemsList = tickets[j].Items;

            if (itemsList.Count > 0)
            {
                string val = "";
                string tPartnumber = itemsList[0].Item.PartNumber;


                for (int i = 0; i < itemsList.Count(); i++)
                {

                    if (itemsList[i].Item.PartNumber == tPartnumber)
                    {
                        if (i == 0)
                        {

                            string DT = "DT #: " + itemsList[i].DeliveryTicket.DTNumber; // itemsList[i].DeliveryTicket.DTNumber
                            string SO = "SO #: " + itemsList[i].DeliveryTicket.SONumber;//itemsList[i].DeliveryTicket.SONumber
                            string PO = "PO #: " + itemsList[i].DeliveryTicket.PONumber; //itemsList[i].DeliveryTicket.PONumber

                            string text = "";
                            if (itemsList[i].DeliveryTicket.DTNumber != null
                                && itemsList[i].DeliveryTicket.DTNumber != "")
                            {
                                text += DT + " | ";
                            }
                            if (itemsList[i].DeliveryTicket.SONumber != null
                                && itemsList[i].DeliveryTicket.SONumber != "")
                            {
                                text += SO + " | ";
                            }
                            if (itemsList[i].DeliveryTicket.PONumber != null
                                && itemsList[i].DeliveryTicket.PONumber != "")
                            {
                                text += PO;
                            }
                            text = text.TrimEnd(new char[] { ' ', '|', ' ' });

                            cell = new PdfPCell(new Phrase(text, FontFactory.GetFont("Arial", 8, Font.BOLD, new BaseColor(255, 255, 255))));
                            cell.BackgroundColor = new BaseColor(150, 150, 150);
                            cell.Colspan = 6;
                            cell.PaddingBottom = 3;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase((i + 1).ToString(), FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(itemsList[i].Item.PartNumber, FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                            table.AddCell(cell);


                            cell = new PdfPCell(new Phrase(itemsList[i].Item.Description, FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                            table.AddCell(cell);

                            val = val + itemsList[i].Item.ShipmentIn.FZInNum + "-" + itemsList[i].Item.Quantity.ToString();
                            cell = new PdfPCell(new Phrase(val, FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(itemsList[i].Item.Quantity.ToString(), FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(itemsList[i].UnitPrice.Value.ToString(), FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                            table.AddCell(cell);
                        }
                        else
                        {
                            val = val + itemsList[i].Item.ShipmentIn.FZInNum + " / " + itemsList[i].Item.Quantity.ToString() + " - ";
                        }

                    }
                    else
                    {
                        if (val == "")
                        {
                            val = val + itemsList[i].Item.ShipmentIn.FZInNum + " - " + itemsList[i].Item.Quantity.ToString();
                        }

                        cell = new PdfPCell(new Phrase((i + 1).ToString(), FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(itemsList[i].Item.PartNumber, FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                        table.AddCell(cell);


                        cell = new PdfPCell(new Phrase(itemsList[i].Item.Description, FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                        table.AddCell(cell);


                        cell = new PdfPCell(new Phrase(val, FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(itemsList[i].Item.Quantity.ToString(), FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(itemsList[i].UnitPrice.Value.ToString(), FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                        table.AddCell(cell);
                        val = "";
                        tPartnumber = itemsList[i].Item.PartNumber;
                    }



                }
            }

        }


      

        
        mainTable.AddCell(table);

        return mainTable;
    }

    public static PdfPTable GetInvoiceFooterTable(ShipmentOut shout)
    {
        PdfPTable maintable = new PdfPTable(3);
        maintable.SpacingBefore = 10f;
        maintable.WidthPercentage = 100;
        maintable.DefaultCell.BorderWidth = 0;
        float[] widths = new float[] { 120f, 116f, 110f };
        maintable.SetWidths(widths);

        PdfPTable table = new PdfPTable(2);
        table.WidthPercentage = 30;
        table.HorizontalAlignment = Element.ALIGN_RIGHT;
        widths = new float[] { 80f, 116f };
        table.SetWidths(widths);

        PdfPCell cell = new PdfPCell(new Phrase("Logistics:", FontFactory.GetFont("Arial", 8, Font.BOLD)));
        cell.BackgroundColor = new BaseColor(233, 234, 234);
        cell.Colspan = 2;
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("Truck Company:", FontFactory.GetFont("Arial", 8, Font.BOLDITALIC)));
        cell.BackgroundColor = new BaseColor(233, 234, 234);
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("Focal Point:", FontFactory.GetFont("Arial", 8, Font.BOLDITALIC)));
        cell.BackgroundColor = new BaseColor(233, 234, 234);
        table.AddCell(cell);
        cell = new PdfPCell();
        cell.AddElement(new Phrase("" + "\r\n" + "", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase(5, "Driver:", FontFactory.GetFont("Arial", 8, Font.BOLDITALIC)));
        cell.BackgroundColor = new BaseColor(233, 234, 234);
        table.AddCell(cell);
        cell = new PdfPCell();
        cell.AddElement(new Phrase("" + "\r\n" + "", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("Truck #:", FontFactory.GetFont("Arial", 8, Font.BOLDITALIC)));
        cell.BackgroundColor = new BaseColor(233, 234, 234);
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase(""));
        cell.BorderWidth = 0;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase(""));
        cell.BorderWidth = 0;
        table.AddCell(cell);

        maintable.AddCell(table);

        maintable.AddCell("");

        table = new PdfPTable(2);
        table.WidthPercentage = 30;
        table.DefaultCell.BorderWidth = 0;
        widths = new float[] { 80f, 116f };
        table.SetWidths(widths);

        cell = new PdfPCell(new Phrase("Issued by:", FontFactory.GetFont("Arial", 8, Font.BOLD)));
        cell.BorderWidth = 0;
        cell.BorderWidthBottom = 1;
        cell.FixedHeight = 20f;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase(""));
        cell.BorderWidth = 0;
        cell.BorderWidthBottom = 1;
        table.AddCell(cell);

        table.AddCell("");
        cell = new PdfPCell(new Phrase("", FontFactory.GetFont("Arial", 6, Font.ITALIC)));
        cell.BorderWidth = 0;
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("Received by:", FontFactory.GetFont("Arial", 8, Font.BOLD)));
        cell.BorderWidth = 0;
        cell.BorderWidthBottom = 1;
        cell.FixedHeight = 20f;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase(""));
        cell.BorderWidth = 0;
        cell.BorderWidthBottom = 1;
        table.AddCell(cell);

        table.AddCell("");
        cell = new PdfPCell(new Phrase("Customer Signature", FontFactory.GetFont("Arial", 6, Font.ITALIC)));
        cell.BorderWidth = 0;
        table.AddCell(cell);

        maintable.AddCell(table);

        return maintable;
    }
}