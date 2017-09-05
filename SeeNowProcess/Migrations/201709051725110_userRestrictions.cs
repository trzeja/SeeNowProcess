namespace SeeNowProcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userRestrictions : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "Login", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.User", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.User", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.User", "role", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "role", c => c.Int());
            AlterColumn("dbo.User", "Email", c => c.String());
            AlterColumn("dbo.User", "Name", c => c.String());
            AlterColumn("dbo.User", "Login", c => c.String(maxLength: 20));
        }
    }
}
