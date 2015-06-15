namespace ImpecMC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wgfag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeliveryTickets", "DTNumber", c => c.String());
            AddColumn("dbo.DeliveryTickets", "SONumber", c => c.String());
            AddColumn("dbo.DeliveryTickets", "PONumber", c => c.String());
            DropColumn("dbo.Divisions", "DTNumber");
            DropColumn("dbo.Divisions", "SONumber");
            DropColumn("dbo.Divisions", "PONumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Divisions", "PONumber", c => c.String());
            AddColumn("dbo.Divisions", "SONumber", c => c.String());
            AddColumn("dbo.Divisions", "DTNumber", c => c.String());
            DropColumn("dbo.DeliveryTickets", "PONumber");
            DropColumn("dbo.DeliveryTickets", "SONumber");
            DropColumn("dbo.DeliveryTickets", "DTNumber");
        }
    }
}
