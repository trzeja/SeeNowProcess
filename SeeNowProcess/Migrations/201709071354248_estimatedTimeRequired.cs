namespace SeeNowProcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class estimatedTimeRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Problem", "EstimatedTime", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Problem", "EstimatedTime", c => c.Time(precision: 7));
        }
    }
}
