using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ImpecMC.Models;

namespace ImpecMC.Models
{
    public class ImpecDBContext : DbContext
    {
        public ImpecDBContext()
            : base("name=DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<ShipmentIn> ShipmentsIn { get; set; }
        public DbSet<ShipmentOut> ShipmentsOut { get; set; }
        public DbSet<DeliveryTicket> DeliveryTickets { get; set; }
        public DbSet<DeliveryTicketItem> DeliveryTicketItems { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<OwnerCompany> OwnerCompanies { get; set; }
        public DbSet<ServiceCompany> ServiceCompanies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //one-to-many 
            modelBuilder.Entity<DeliveryTicketItem>()
                        .HasOptional<DeliveryTicket>(s => s.DeliveryTicket)
                        .WithMany(s => s.Items)
                        .HasForeignKey(s => s.DeliveryTicketId);

            modelBuilder.Entity<DeliveryTicket>()
                        .HasOptional<ShipmentOut>(s => s.ShipmentOut)
                        .WithMany(s => s.DeliveryTickets)
                        .HasForeignKey(s => s.ShipmentOutId);

            modelBuilder.Entity<DeliveryTicket>()
                        .HasOptional<ShipmentOut>(s => s.ShipmentOut)
                        .WithMany(s => s.DeliveryTickets)
                        .HasForeignKey(s => s.ShipmentOutId);

            modelBuilder.Entity<Item>()
                        .HasOptional<ShipmentIn>(s => s.ShipmentIn)
                        .WithMany(s => s.Items)
                        .HasForeignKey(s => s.ShipmentInId);

            modelBuilder.Entity<DeliveryTicketItem>()
                        .HasOptional<Item>(s => s.Item)
                        .WithMany(s => s.DeliveryTicketItems)
                        .HasForeignKey(s => s.ItemId);

            //modelBuilder.Entity<Order>()
            //            .HasMany<Rider>(s => s.PotentialRiders)
            //            .WithMany(s => s.PotentialOrders)
            //            .Map(c =>
            //            {
            //                c.MapLeftKey("OrderId");
            //                c.MapRightKey("PotentialRiderId");
            //                c.ToTable("OrderPotentialRider");
            //            });



            //modelBuilder.Entity<Order>()
            //            .HasMany<Rider>(s => s.RejectedRiders)
            //            .WithMany(s => s.RejectedOrders)
            //            .Map(c =>
            //            {
            //                c.MapLeftKey("OrderId");
            //                c.MapRightKey("RejectedRiderId");
            //                c.ToTable("OrderRejectedRider");
            //            });
        }
    }
}