namespace ImpecMC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.ShipmentIns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        FZInNum = c.String(),
                        CommercialInvoiceNum = c.String(),
                        AWBBOL = c.String(),
                        DocReceivedDate = c.DateTime(),
                        ArrivedPortDate = c.DateTime(),
                        ArrivedFZDate = c.DateTime(),
                        FreightType = c.Int(),
                        DivisionId = c.Int(),
                        Status = c.Int(),
                        Insurance = c.Double(),
                        ShipmentType = c.Int(),
                        ClaimsNum = c.String(),
                        FOB = c.String(),
                        TotalCost = c.Double(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "dbo.Divisions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        Name = c.String(),
                        DTNumber = c.String(),
                        SONumber = c.String(),
                        PONumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        Quantity = c.Int(nullable: false),
                        PartNumber = c.String(),
                        Description = c.String(),
                        UOM = c.String(),
                        HSCode = c.String(),
                        UnitPrice = c.Double(nullable: false),
                        ShipmentInId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ShipmentIns", t => t.ShipmentInId)
                .Index(t => t.ShipmentInId);
            
            CreateTable(
                "dbo.DeliveryTicketItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeliveryTicketId = c.Int(),
                        ItemId = c.Int(),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DeliveryTickets", t => t.DeliveryTicketId)
                .ForeignKey("dbo.Items", t => t.ItemId)
                .Index(t => t.DeliveryTicketId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.DeliveryTickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        ShipmentOutId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ShipmentOuts", t => t.ShipmentOutId)
                .Index(t => t.ShipmentOutId);
            
            CreateTable(
                "dbo.ShipmentOuts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        InvoiceNumber = c.String(),
                        ServiceCompanyId = c.Int(),
                        OwnerCompanyId = c.Int(),
                        CDNumber = c.String(),
                        ShahadaNumber = c.String(),
                        KasimaNumber = c.String(),
                        ExportNumber = c.String(),
                        SendDate = c.DateTime(nullable: false),
                        ReceivedDate = c.DateTime(nullable: false),
                        Status = c.String(),
                        Type = c.Int(nullable: false),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceCompanies", t => t.ServiceCompanyId)
                .ForeignKey("dbo.OwnerCompanies", t => t.OwnerCompanyId)
                .Index(t => t.ServiceCompanyId)
                .Index(t => t.OwnerCompanyId);
            
            CreateTable(
                "dbo.ServiceCompanies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OwnerCompanies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ShipmentOuts", new[] { "OwnerCompanyId" });
            DropIndex("dbo.ShipmentOuts", new[] { "ServiceCompanyId" });
            DropIndex("dbo.DeliveryTickets", new[] { "ShipmentOutId" });
            DropIndex("dbo.DeliveryTicketItems", new[] { "ItemId" });
            DropIndex("dbo.DeliveryTicketItems", new[] { "DeliveryTicketId" });
            DropIndex("dbo.Items", new[] { "ShipmentInId" });
            DropIndex("dbo.ShipmentIns", new[] { "DivisionId" });
            DropForeignKey("dbo.ShipmentOuts", "OwnerCompanyId", "dbo.OwnerCompanies");
            DropForeignKey("dbo.ShipmentOuts", "ServiceCompanyId", "dbo.ServiceCompanies");
            DropForeignKey("dbo.DeliveryTickets", "ShipmentOutId", "dbo.ShipmentOuts");
            DropForeignKey("dbo.DeliveryTicketItems", "ItemId", "dbo.Items");
            DropForeignKey("dbo.DeliveryTicketItems", "DeliveryTicketId", "dbo.DeliveryTickets");
            DropForeignKey("dbo.Items", "ShipmentInId", "dbo.ShipmentIns");
            DropForeignKey("dbo.ShipmentIns", "DivisionId", "dbo.Divisions");
            DropTable("dbo.OwnerCompanies");
            DropTable("dbo.ServiceCompanies");
            DropTable("dbo.ShipmentOuts");
            DropTable("dbo.DeliveryTickets");
            DropTable("dbo.DeliveryTicketItems");
            DropTable("dbo.Items");
            DropTable("dbo.Divisions");
            DropTable("dbo.ShipmentIns");
            DropTable("dbo.UserProfile");
        }
    }
}
