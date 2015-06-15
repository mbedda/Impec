namespace ImpecMC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dtchanges : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DeliveryTickets", "DeliveryTicketNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeliveryTickets", "DeliveryTicketNumber", c => c.String());
        }
    }
}
