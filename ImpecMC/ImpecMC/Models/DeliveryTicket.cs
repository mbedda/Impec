using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImpecMC.Models
{
    public class DeliveryTicket
    {
        public int Id { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        public virtual List<DeliveryTicketItem> Items { get; set; }
        [Display(Name = "Shipment Out")]
        public int? ShipmentOutId { get; set; }
        [Display(Name = "Shipment Out")]
        public virtual ShipmentOut ShipmentOut { get; set; }
        [Display(Name = "DT #")]
        public string DTNumber { get; set; }
        [Display(Name = "SO #")]
        public string SONumber { get; set; }
        [Display(Name = "PO #")]
        public string PONumber { get; set; }

        public DeliveryTicket()
        {
            Items = new List<DeliveryTicketItem>();
        }
    }
}