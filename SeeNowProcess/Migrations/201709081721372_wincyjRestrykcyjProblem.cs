namespace SeeNowProcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wincyjRestrykcyjProblem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Problem", "Box_BoxID", "dbo.Box");
            DropForeignKey("dbo.Problem", "Iteration_IterationId", "dbo.Iteration");
            DropForeignKey("dbo.Problem", "Story_UserStoryID", "dbo.UserStory");
            DropIndex("dbo.Problem", new[] { "Box_BoxID" });
            DropIndex("dbo.Problem", new[] { "Iteration_IterationId" });
            DropIndex("dbo.Problem", new[] { "Story_UserStoryID" });
            AlterColumn("dbo.Problem", "Progress", c => c.Int(nullable: false));
            AlterColumn("dbo.Problem", "Box_BoxID", c => c.Int(nullable: false));
            AlterColumn("dbo.Problem", "Iteration_IterationId", c => c.Int(nullable: false));
            AlterColumn("dbo.Problem", "Story_UserStoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.Problem", "Box_BoxID");
            CreateIndex("dbo.Problem", "Iteration_IterationId");
            CreateIndex("dbo.Problem", "Story_UserStoryID");
            AddForeignKey("dbo.Problem", "Box_BoxID", "dbo.Box", "BoxID", cascadeDelete: true);
            AddForeignKey("dbo.Problem", "Iteration_IterationId", "dbo.Iteration", "IterationId", cascadeDelete: true);
            AddForeignKey("dbo.Problem", "Story_UserStoryID", "dbo.UserStory", "UserStoryID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Problem", "Story_UserStoryID", "dbo.UserStory");
            DropForeignKey("dbo.Problem", "Iteration_IterationId", "dbo.Iteration");
            DropForeignKey("dbo.Problem", "Box_BoxID", "dbo.Box");
            DropIndex("dbo.Problem", new[] { "Story_UserStoryID" });
            DropIndex("dbo.Problem", new[] { "Iteration_IterationId" });
            DropIndex("dbo.Problem", new[] { "Box_BoxID" });
            AlterColumn("dbo.Problem", "Story_UserStoryID", c => c.Int());
            AlterColumn("dbo.Problem", "Iteration_IterationId", c => c.Int());
            AlterColumn("dbo.Problem", "Box_BoxID", c => c.Int());
            AlterColumn("dbo.Problem", "Progress", c => c.Int());
            CreateIndex("dbo.Problem", "Story_UserStoryID");
            CreateIndex("dbo.Problem", "Iteration_IterationId");
            CreateIndex("dbo.Problem", "Box_BoxID");
            AddForeignKey("dbo.Problem", "Story_UserStoryID", "dbo.UserStory", "UserStoryID");
            AddForeignKey("dbo.Problem", "Iteration_IterationId", "dbo.Iteration", "IterationId");
            AddForeignKey("dbo.Problem", "Box_BoxID", "dbo.Box", "BoxID");
        }
    }
}
