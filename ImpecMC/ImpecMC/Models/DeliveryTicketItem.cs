using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImpecMC.Models
{
    public class DeliveryTicketItem
    {
        public int Id { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        [Display(Name = "Delivery Ticket")]
        public int? DeliveryTicketId { get; set; }
        [Display(Name = "Delivery Ticket")]
        public virtual DeliveryTicket DeliveryTicket { get; set; }
        [Display(Name = "Item")]
        public int? ItemId { get; set; }
        [Display(Name = "Item")]
        public virtual Item Item { get; set; }
        public int? Quantity { get; set; }
        [Display(Name = "Unit Price")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C0}")]
        public double? UnitPrice { get; set; }
    }
}