namespace SeeNowProcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class supervisorAndNiggersRemoved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.User", "Supervisor_UserID", "dbo.User");
            DropIndex("dbo.User", new[] { "Supervisor_UserID" });
            DropColumn("dbo.User", "Supervisor_UserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "Supervisor_UserID", c => c.Int());
            CreateIndex("dbo.User", "Supervisor_UserID");
            AddForeignKey("dbo.User", "Supervisor_UserID", "dbo.User", "UserID");
        }
    }
}
