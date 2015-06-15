using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImpecMC.Models
{
    public class InventoryViewModel
    {
        [Display(Name = "Part #")]
        public string PartNumber { get; set; }
        public string Description { get; set; }
        [Display(Name = "Quantity In")]
        public int QuantityIn { get; set; }
        [Display(Name = "Quantity Out")]
        public int QuantityOut { get; set; }
        [Display(Name = "Quantity On Hand")]
        public int QuantityOnHand { get; set; }
    }
}