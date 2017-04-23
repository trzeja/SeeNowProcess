namespace SeeNowProcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewPasswordStoring : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Salt", c => c.Binary());
            AddColumn("dbo.User", "Hash", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "Hash");
            DropColumn("dbo.User", "Salt");
        }
    }
}
