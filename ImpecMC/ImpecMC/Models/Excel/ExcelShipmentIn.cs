using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImpecMC.Models.Excel
{
    public class ExcelShipmentIn
    {
        public string FZInNum { get; set; }
        public string CommercialInvoiceNum { get; set; }
        public string AWBBOL { get; set; }
        public ShipmentInFreightType FreightType { get; set; }
        public DateTime? DocReceivedDate { get; set; }
        public string DivisionName { get; set; }
        public ShipmentInStatus Status { get; set; }
        public double TotalCost { get; set; }
        public double Insurance { get; set; }










    }
}