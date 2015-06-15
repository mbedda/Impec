namespace ImpecMC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class otheroptionals : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ShipmentOuts", "SendDate", c => c.DateTime());
            AlterColumn("dbo.ShipmentOuts", "ReceivedDate", c => c.DateTime());
            AlterColumn("dbo.ShipmentOuts", "Type", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ShipmentOuts", "Type", c => c.Int(nullable: false));
            AlterColumn("dbo.ShipmentOuts", "ReceivedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ShipmentOuts", "SendDate", c => c.DateTime(nullable: false));
        }
    }
}
