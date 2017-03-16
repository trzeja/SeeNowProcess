namespace SeeNowProcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Comments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SimpleTasks", "Comments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SimpleTasks", "Comments");
        }
    }
}
