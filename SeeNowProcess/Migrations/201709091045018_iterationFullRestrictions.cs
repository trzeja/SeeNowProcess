namespace SeeNowProcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class iterationFullRestrictions : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Iteration", "Project_ProjectID", "dbo.Project");
            DropIndex("dbo.Iteration", new[] { "Project_ProjectID" });
            AlterColumn("dbo.Iteration", "Name", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.Iteration", "Description", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Iteration", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Iteration", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Iteration", "Project_ProjectID", c => c.Int(nullable: false));
            CreateIndex("dbo.Iteration", "Project_ProjectID");
            AddForeignKey("dbo.Iteration", "Project_ProjectID", "dbo.Project", "ProjectID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Iteration", "Project_ProjectID", "dbo.Project");
            DropIndex("dbo.Iteration", new[] { "Project_ProjectID" });
            AlterColumn("dbo.Iteration", "Project_ProjectID", c => c.Int());
            AlterColumn("dbo.Iteration", "EndDate", c => c.DateTime());
            AlterColumn("dbo.Iteration", "StartDate", c => c.DateTime());
            AlterColumn("dbo.Iteration", "Description", c => c.String());
            AlterColumn("dbo.Iteration", "Name", c => c.String(maxLength: 60));
            CreateIndex("dbo.Iteration", "Project_ProjectID");
            AddForeignKey("dbo.Iteration", "Project_ProjectID", "dbo.Project", "ProjectID");
        }
    }
}
