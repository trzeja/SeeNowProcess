namespace SeeNowProcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class itertionChangeRestriction : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Iteration", "EndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Iteration", "EndDate", c => c.DateTime(nullable: false));
        }
    }
}
