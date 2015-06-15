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
    public class DTItemsApiController : ApiController
    {
        private ImpecDBContext db = new ImpecDBContext();

        public object GetItems()
        {
            var items = db.Items.Select(item => new
            {
                item.Id,
                item.PartNumber,
                item.DateCreated
            }).OrderByDescending(s => s.DateCreated).ToList();

            return items;
        }

        public object GetDTItems()
        {
            var deliveryTicketItems = db.DeliveryTicketItems.Select(deliveryTicketItem => new
            {
                deliveryTicketItem.Id,
                deliveryTicketItem.DateCreated,
                DTNumber = deliveryTicketItem.DeliveryTicket != null ? deliveryTicketItem.DeliveryTicket.DTNumber : "",
                PartNumber = deliveryTicketItem.Item != null ? deliveryTicketItem.Item.PartNumber : "",
                deliveryTicketItem.Quantity,
                deliveryTicketItem.UnitPrice,
                deliveryTicketItem.DeliveryTicketId
            }).OrderByDescending(s => s.DateCreated).ToList();

            return deliveryTicketItems;
        }

        public object GetDTItems(int id)
        {
            DeliveryTicket deliveryTicket = db.DeliveryTickets.Find(id);
            if (deliveryTicket == null)
            {
                return new List<DeliveryTicketItem> { };
            }

            var deliveryTicketItems = deliveryTicket.Items.Select(deliveryTicketItem => new
            {
                deliveryTicketItem.Id,
                deliveryTicketItem.DateCreated,
                DTNumber = deliveryTicketItem.DeliveryTicket != null ? deliveryTicketItem.DeliveryTicket.DTNumber : "",
                PartNumber = deliveryTicketItem.Item != null ? deliveryTicketItem.Item.PartNumber : "",
                deliveryTicketItem.Quantity,
                deliveryTicketItem.UnitPrice,
                deliveryTicketItem.DeliveryTicketId
            }).OrderByDescending(s => s.DateCreated).ToList();

            return deliveryTicketItems;
        }



        [ResponseType(typeof(DeliveryTicketItem))]
        public IHttpActionResult CreateDTItem(DeliveryTicketItem deliveryTicketItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            deliveryTicketItem.DateCreated = DateTime.UtcNow.AddHours(2);

            db.DeliveryTicketItems.Add(deliveryTicketItem);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = deliveryTicketItem.Id }, deliveryTicketItem);
        }

        [ResponseType(typeof(DeliveryTicketItem))]
        public IHttpActionResult EditDTItem(DeliveryTicketItem deliveryTicketItem)
        {
            db.Entry(deliveryTicketItem).State = EntityState.Modified;
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = deliveryTicketItem.Id }, deliveryTicketItem);
        }
        
        [ResponseType(typeof(DeliveryTicketItem))]
        public IHttpActionResult RemoveDTItem(int id)
        {
            DeliveryTicketItem deliveryTicketItem = db.DeliveryTicketItems.Find(id);
            if (deliveryTicketItem == null)
            {
                return NotFound();
            }

            db.DeliveryTicketItems.Remove(deliveryTicketItem);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = deliveryTicketItem.Id }, deliveryTicketItem);
        }
    }
}   
