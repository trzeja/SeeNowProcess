namespace SeeNowProcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class projectRestrictions : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Project", "Name", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.Project", "Description", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Project", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Project", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Project", "Status", c => c.Int());
            AlterColumn("dbo.Project", "StartDate", c => c.DateTime());
            AlterColumn("dbo.Project", "Description", c => c.String());
            AlterColumn("dbo.Project", "Name", c => c.String(maxLength: 60));
        }
    }
}
