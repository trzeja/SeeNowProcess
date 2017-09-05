namespace SeeNowProcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class problemRestrictions : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Problem", "Title", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.Problem", "Description", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Problem", "Importance", c => c.Int(nullable: false));
            AlterColumn("dbo.Problem", "CreationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Problem", "CreationDate", c => c.DateTime());
            AlterColumn("dbo.Problem", "Importance", c => c.Int());
            AlterColumn("dbo.Problem", "Description", c => c.String());
            AlterColumn("dbo.Problem", "Title", c => c.String(maxLength: 60));
        }
    }
}
