namespace SeeNowProcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teamRestrictions : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Team", "Name", c => c.String(nullable: false, maxLength: 60));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Team", "Name", c => c.String());
        }
    }
}
