using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImpecMC.Models.Excel
{
    public class ExcelDeliveryTicket
    {
        public string purchaseOrderNo { get; set; }
        public string salesDocument { get; set; }
        public string delivery { get; set; }
        public string partNumber { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public double unitprice { get; set; }
        public double total { get; set; }
    }
}