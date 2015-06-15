namespace ImpecMC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wegzmig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeliveryTickets", "DeliveryNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeliveryTickets", "DeliveryNumber");
        }
    }
}
