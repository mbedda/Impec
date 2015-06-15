using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImpecMC.Models
{
    public class ShipmentIn
    {
        public int Id { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        [Display(Name = "FZ In #")]
        public string FZInNum { get; set; }
        [Display(Name = "Commercial Invoice #")]
        public string CommercialInvoiceNum { get; set; }
        [Display(Name = "AWB/BOL")]
        public string AWBBOL { get; set; }
        [Display(Name = "Document Received Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d/M/yyyy}")]
        public DateTime? DocReceivedDate { get; set; }
        [Display(Name = "Arrived Port Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d/M/yyyy}")]
        public DateTime? ArrivedPortDate { get; set; }
        [Display(Name = "Arrived FZ Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d/M/yyyy}")]
        public DateTime? ArrivedFZDate { get; set; }
        [Display(Name = "Freight Type")]
        public ShipmentInFreightType? FreightType { get; set; }
        [Display(Name = "Division")]
        public int? DivisionId { get; set; }
        [Display(Name = "Division")]
        public virtual Division Division { get; set; }
        [Display(Name = "Status")]
        public ShipmentInStatus? Status { get; set; }
        [Display(Name = "Insurance")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public double? Insurance { get; set; }
        [Display(Name = "Shipment Type")]
        public ShipmentInType? ShipmentType { get; set; }
        [Display(Name = "Claims #")]
        public string ClaimsNum { get; set; }
        public string FOB { get; set; }
        [Display(Name = "Total Cost")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public double? TotalCost { get; set; }
        public virtual List<Item> Items { get; set; }

        public ShipmentIn()
        {
            Items = new List<Item>();
        }
    }

    public enum ShipmentInFreightType
    {
        Air, Land, Ocean
    }

    public enum ShipmentInStatus
    {
        Complete, Waiting, Canceled
    }

    public enum ShipmentInType
    {
        Stock, Assets, Consumables
    }
}