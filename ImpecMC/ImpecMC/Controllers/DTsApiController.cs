using ImpecMC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace ImpecMC.Controllers
{
    public class DTsApiController : ApiController
    {
        private ImpecDBContext db = new ImpecDBContext();
        public object GetDTs()
        {
            var deliveryTickets = db.DeliveryTickets.Select(deliveryTicket => new
            {
                deliveryTicket.Id,
                deliveryTicket.DateCreated,
                InvoiceNumber = deliveryTicket.ShipmentOut.InvoiceNumber,
                deliveryTicket.DTNumber,
                deliveryTicket.SONumber,
                deliveryTicket.PONumber,
                ItemsCount = deliveryTicket.Items != null ? deliveryTicket.Items.Count : 0,
                deliveryTicket.ShipmentOutId
            }).OrderByDescending(s => s.DateCreated).ToList();

            return deliveryTickets;
        }

        public object GetDTs(int id)
        {
            ShipmentOut shipmentOut = db.ShipmentsOut.Find(id);
            if (shipmentOut == null)
            {
                return new List<DeliveryTicket> { };
            }

            var deliveryTickets = shipmentOut.DeliveryTickets.Select(deliveryTicket => new
            {
                deliveryTicket.Id,
                deliveryTicket.DateCreated,
                InvoiceNumber = deliveryTicket.ShipmentOut.InvoiceNumber,
                deliveryTicket.DTNumber,
                deliveryTicket.SONumber,
                deliveryTicket.PONumber,
                ItemsCount = deliveryTicket.Items != null ? deliveryTicket.Items.Count : 0,
                deliveryTicket.ShipmentOutId
            }).OrderByDescending(s => s.DateCreated).ToList();

            return deliveryTickets;
        }



        [ResponseType(typeof(DeliveryTicket))]
        public IHttpActionResult CreateDT(DeliveryTicket deliveryTicket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            deliveryTicket.DateCreated = DateTime.UtcNow.AddHours(2);

            db.DeliveryTickets.Add(deliveryTicket);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = deliveryTicket.Id }, deliveryTicket);
        }

        [ResponseType(typeof(DeliveryTicket))]
        public IHttpActionResult EditDT(DeliveryTicket deliveryTicket)
        {
            db.Entry(deliveryTicket).State = EntityState.Modified;
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = deliveryTicket.Id }, deliveryTicket);
        }
        
        [ResponseType(typeof(DeliveryTicket))]
        public IHttpActionResult RemoveDT(int id)
        {
            DeliveryTicket deliveryTicket = db.DeliveryTickets.Find(id);
            if (deliveryTicket == null)
            {
                return NotFound();
            }

            db.DeliveryTickets.Remove(deliveryTicket);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = deliveryTicket.Id }, deliveryTicket);
        }
    }
}   
