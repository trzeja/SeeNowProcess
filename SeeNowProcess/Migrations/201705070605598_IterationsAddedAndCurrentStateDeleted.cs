namespace SeeNowProcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IterationsAddedAndCurrentStateDeleted : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Box", "Project_ProjectID", "dbo.Project");
            DropIndex("dbo.Box", new[] { "Project_ProjectID" });
            CreateTable(
                "dbo.Iteration",
                c => new
                    {
                        IterationId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 60),
                        Description = c.String(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Project_ProjectID = c.Int(),
                    })
                .PrimaryKey(t => t.IterationId)
                .ForeignKey("dbo.Project", t => t.Project_ProjectID)
                .Index(t => t.Project_ProjectID);
            
            AddColumn("dbo.Box", "Iteration_IterationId", c => c.Int());
            CreateIndex("dbo.Box", "Iteration_IterationId");
            AddForeignKey("dbo.Box", "Iteration_IterationId", "dbo.Iteration", "IterationId");
            DropColumn("dbo.Problem", "CurrentState");
            DropColumn("dbo.Box", "Project_ProjectID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Box", "Project_ProjectID", c => c.Int());
            AddColumn("dbo.Problem", "CurrentState", c => c.Int());
            DropForeignKey("dbo.Box", "Iteration_IterationId", "dbo.Iteration");
            DropForeignKey("dbo.Iteration", "Project_ProjectID", "dbo.Project");
            DropIndex("dbo.Iteration", new[] { "Project_ProjectID" });
            DropIndex("dbo.Box", new[] { "Iteration_IterationId" });
            DropColumn("dbo.Box", "Iteration_IterationId");
            DropTable("dbo.Iteration");
            CreateIndex("dbo.Box", "Project_ProjectID");
            AddForeignKey("dbo.Box", "Project_ProjectID", "dbo.Project", "ProjectID");
        }
    }
}
