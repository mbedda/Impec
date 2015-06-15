using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImpecMC.Models.Excel
{
    public class ExcelItem
    {
        public string FZInNum { get; set; }
        public string DivisionName { get; set; }
        public int InQuantity { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public int OutQuantity { get; set; }
    }
}