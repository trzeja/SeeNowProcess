namespace SeeNowProcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class parentTaskDeletion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Problem", "ParentProblem_ProblemID", "dbo.Problem");
            DropIndex("dbo.Problem", new[] { "ParentProblem_ProblemID" });
            DropColumn("dbo.Problem", "ParentProblem_ProblemID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Problem", "ParentProblem_ProblemID", c => c.Int());
            CreateIndex("dbo.Problem", "ParentProblem_ProblemID");
            AddForeignKey("dbo.Problem", "ParentProblem_ProblemID", "dbo.Problem", "ProblemID");
        }
    }
}
