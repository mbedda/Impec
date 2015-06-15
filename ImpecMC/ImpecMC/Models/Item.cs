using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImpecMC.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        public int? Quantity { get; set; }
        [Display(Name = "Part #")]
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string UOM { get; set; }
        [Display(Name = "HS Code")]
        public string HSCode { get; set; }
        [Display(Name = "Unit Price")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public double? UnitPrice { get; set; }
        [Display(Name = "Shipment In")]
        public int? ShipmentInId { get; set; }
        [Display(Name = "Shipment In")]
        public virtual ShipmentIn ShipmentIn { get; set; }
        public virtual List<DeliveryTicketItem> DeliveryTicketItems { get; set; }

        public Item()
        {
            DeliveryTicketItems = new List<DeliveryTicketItem>();
        }
    }
}