namespace SeeNowProcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class iterationBoxtoprojectBox : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Box", "Iteration_IterationId", "dbo.Iteration");
            DropIndex("dbo.Box", new[] { "Iteration_IterationId" });
            AddColumn("dbo.Problem", "Iteration_IterationId", c => c.Int());
            AddColumn("dbo.Box", "Project_ProjectID", c => c.Int());
            CreateIndex("dbo.Problem", "Iteration_IterationId");
            CreateIndex("dbo.Box", "Project_ProjectID");
            AddForeignKey("dbo.Box", "Project_ProjectID", "dbo.Project", "ProjectID");
            AddForeignKey("dbo.Problem", "Iteration_IterationId", "dbo.Iteration", "IterationId");
            DropColumn("dbo.Box", "Iteration_IterationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Box", "Iteration_IterationId", c => c.Int());
            DropForeignKey("dbo.Problem", "Iteration_IterationId", "dbo.Iteration");
            DropForeignKey("dbo.Box", "Project_ProjectID", "dbo.Project");
            DropIndex("dbo.Box", new[] { "Project_ProjectID" });
            DropIndex("dbo.Problem", new[] { "Iteration_IterationId" });
            DropColumn("dbo.Box", "Project_ProjectID");
            DropColumn("dbo.Problem", "Iteration_IterationId");
            CreateIndex("dbo.Box", "Iteration_IterationId");
            AddForeignKey("dbo.Box", "Iteration_IterationId", "dbo.Iteration", "IterationId");
        }
    }
}
