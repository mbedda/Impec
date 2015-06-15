namespace ImpecMC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class otheroptionals1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Items", "Quantity", c => c.Int());
            AlterColumn("dbo.Items", "UnitPrice", c => c.Double());
            AlterColumn("dbo.DeliveryTicketItems", "Quantity", c => c.Int());
            AlterColumn("dbo.DeliveryTicketItems", "UnitPrice", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DeliveryTicketItems", "UnitPrice", c => c.Double(nullable: false));
            AlterColumn("dbo.DeliveryTicketItems", "Quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.Items", "UnitPrice", c => c.Double(nullable: false));
            AlterColumn("dbo.Items", "Quantity", c => c.Int(nullable: false));
        }
    }
}
