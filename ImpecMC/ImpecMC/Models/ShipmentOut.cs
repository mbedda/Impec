using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImpecMC.Models
{
    public class ShipmentOut
    {
        public int Id { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        [Display(Name = "Invoice #")]
        public string InvoiceNumber { get; set; }
        [Display(Name = "Service Company")]
        public int? ServiceCompanyId { get; set; }
        [Display(Name = "Service Company")]
        public virtual ServiceCompany ServiceCompany { get; set; }
        [Display(Name = "Owner Company")]
        public int? OwnerCompanyId { get; set; }
        [Display(Name = "Owner Company")]
        public virtual OwnerCompany OwnerCompany { get; set; }
        [Display(Name = "CD #")]
        public string CDNumber { get; set; }
        [Display(Name = "Shahada #")]
        public string ShahadaNumber { get; set; }
        [Display(Name = "Kasima #")]
        public string KasimaNumber { get; set; }
        [Display(Name = "Export #")]
        public string ExportNumber { get; set; }
        [Display(Name = "Send Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d/M/yyyy}")]
        public DateTime? SendDate { get; set; }
        [Display(Name = "Received Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d/M/yyyy}")]
        public DateTime? ReceivedDate { get; set; }
        public string Status { get; set; }
        public ShipmentOutType? Type { get; set; }
        public string Notes { get; set; }
        public virtual List<DeliveryTicket> DeliveryTickets { get; set; }


        public ShipmentOut()
        {
            DeliveryTickets = new List<DeliveryTicket>();
        }
    }
    public enum ShipmentOutType
    {
        Local, Abroad, Transit
    }
}