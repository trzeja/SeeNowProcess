namespace SeeNowProcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assignment",
                c => new
                    {
                        AssignmentID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        TeamID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AssignmentID)
                .ForeignKey("dbo.Team", t => t.TeamID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.TeamID);
            
            CreateTable(
                "dbo.Team",
                c => new
                    {
                        TeamID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UserStory_UserStoryID = c.Int(),
                        TeamLeader_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.TeamID)
                .ForeignKey("dbo.UserStory", t => t.UserStory_UserStoryID)
                .ForeignKey("dbo.User", t => t.TeamLeader_UserID)
                .Index(t => t.UserStory_UserStoryID)
                .Index(t => t.TeamLeader_UserID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Login = c.String(maxLength: 20),
                        Password = c.String(maxLength: 100),
                        Name = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        role = c.Int(),
                        Supervisor_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.User", t => t.Supervisor_UserID)
                .Index(t => t.Supervisor_UserID);
            
            CreateTable(
                "dbo.UserStory",
                c => new
                    {
                        UserStoryID = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 60),
                        Description = c.String(),
                        Size = c.Int(),
                        Unit = c.String(),
                        Notes = c.String(),
                        Criteria = c.String(),
                        Owner_UserID = c.Int(),
                        Project_ProjectID = c.Int(),
                    })
                .PrimaryKey(t => t.UserStoryID)
                .ForeignKey("dbo.User", t => t.Owner_UserID)
                .ForeignKey("dbo.Project", t => t.Project_ProjectID)
                .Index(t => t.Owner_UserID)
                .Index(t => t.Project_ProjectID);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        ProjectID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 60),
                        Description = c.String(),
                        StartDate = c.DateTime(),
                        CompletionDate = c.DateTime(),
                        Status = c.Int(),
                    })
                .PrimaryKey(t => t.ProjectID);
            
            CreateTable(
                "dbo.Box",
                c => new
                    {
                        BoxID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Project_ProjectID = c.Int(),
                    })
                .PrimaryKey(t => t.BoxID)
                .ForeignKey("dbo.Project", t => t.Project_ProjectID)
                .Index(t => t.Project_ProjectID);
            
            CreateTable(
                "dbo.Problem",
                c => new
                    {
                        ProblemID = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 60),
                        Description = c.String(),
                        CurrentState = c.Int(),
                        Importance = c.Int(),
                        Progress = c.Int(),
                        CreationDate = c.DateTime(),
                        EstimatedTime = c.Time(precision: 7),
                        FinalTime = c.Time(precision: 7),
                        Box_BoxID = c.Int(),
                        ParentProblem_ProblemID = c.Int(),
                        Story_UserStoryID = c.Int(),
                    })
                .PrimaryKey(t => t.ProblemID)
                .ForeignKey("dbo.Box", t => t.Box_BoxID)
                .ForeignKey("dbo.Problem", t => t.ParentProblem_ProblemID)
                .ForeignKey("dbo.UserStory", t => t.Story_UserStoryID)
                .Index(t => t.Box_BoxID)
                .Index(t => t.ParentProblem_ProblemID)
                .Index(t => t.Story_UserStoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Team", "TeamLeader_UserID", "dbo.User");
            DropForeignKey("dbo.User", "Supervisor_UserID", "dbo.User");
            DropForeignKey("dbo.Team", "UserStory_UserStoryID", "dbo.UserStory");
            DropForeignKey("dbo.UserStory", "Project_ProjectID", "dbo.Project");
            DropForeignKey("dbo.Box", "Project_ProjectID", "dbo.Project");
            DropForeignKey("dbo.Problem", "Story_UserStoryID", "dbo.UserStory");
            DropForeignKey("dbo.Problem", "ParentProblem_ProblemID", "dbo.Problem");
            DropForeignKey("dbo.Problem", "Box_BoxID", "dbo.Box");
            DropForeignKey("dbo.UserStory", "Owner_UserID", "dbo.User");
            DropForeignKey("dbo.Assignment", "UserID", "dbo.User");
            DropForeignKey("dbo.Assignment", "TeamID", "dbo.Team");
            DropIndex("dbo.Problem", new[] { "Story_UserStoryID" });
            DropIndex("dbo.Problem", new[] { "ParentProblem_ProblemID" });
            DropIndex("dbo.Problem", new[] { "Box_BoxID" });
            DropIndex("dbo.Box", new[] { "Project_ProjectID" });
            DropIndex("dbo.UserStory", new[] { "Project_ProjectID" });
            DropIndex("dbo.UserStory", new[] { "Owner_UserID" });
            DropIndex("dbo.User", new[] { "Supervisor_UserID" });
            DropIndex("dbo.Team", new[] { "TeamLeader_UserID" });
            DropIndex("dbo.Team", new[] { "UserStory_UserStoryID" });
            DropIndex("dbo.Assignment", new[] { "TeamID" });
            DropIndex("dbo.Assignment", new[] { "UserID" });
            DropTable("dbo.Problem");
            DropTable("dbo.Box");
            DropTable("dbo.Project");
            DropTable("dbo.UserStory");
            DropTable("dbo.User");
            DropTable("dbo.Team");
            DropTable("dbo.Assignment");
        }
    }
}
