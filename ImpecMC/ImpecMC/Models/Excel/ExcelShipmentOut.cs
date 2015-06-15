using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImpecMC.Models.Excel
{
    public class ExcelShipmentOut
    {
        public string invoice { get; set; }
        public string dt { get; set; }
        public string company { get; set; }
        public string owner { get; set; }
        public string cdn { get; set; }
        public string shahada { get; set; }
        public string kasima { get; set; }
        public string export { get; set; }
        public string totalINV { get; set; }
        public DateTime? sendDate { get; set; }
        public DateTime? rcvd { get; set; }
        public string status { get; set; }
        public string notes { get; set; }

    }
}