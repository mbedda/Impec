namespace ImpecMC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wegzmig2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeliveryTickets", "DeliveryTicketNumber", c => c.String());
            DropColumn("dbo.DeliveryTickets", "DeliveryNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeliveryTickets", "DeliveryNumber", c => c.String());
            DropColumn("dbo.DeliveryTickets", "DeliveryTicketNumber");
        }
    }
}
