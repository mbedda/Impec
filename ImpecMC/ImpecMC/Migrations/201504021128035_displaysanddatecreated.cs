namespace ImpecMC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class displaysanddatecreated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeliveryTicketItems", "DateCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeliveryTicketItems", "DateCreated");
        }
    }
}
