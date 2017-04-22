namespace SeeNowProcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BoxOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Box", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Box", "Order");
        }
    }
}
