namespace SeeNowProcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SimpleTasks", "Title", c => c.String(maxLength: 60));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SimpleTasks", "Title", c => c.String());
        }
    }
}
