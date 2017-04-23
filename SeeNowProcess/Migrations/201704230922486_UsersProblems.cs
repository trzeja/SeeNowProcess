namespace SeeNowProcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersProblems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProblemUser",
                c => new
                    {
                        Problem_ProblemID = c.Int(nullable: false),
                        User_UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Problem_ProblemID, t.User_UserID })
                .ForeignKey("dbo.Problem", t => t.Problem_ProblemID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_UserID, cascadeDelete: true)
                .Index(t => t.Problem_ProblemID)
                .Index(t => t.User_UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProblemUser", "User_UserID", "dbo.User");
            DropForeignKey("dbo.ProblemUser", "Problem_ProblemID", "dbo.Problem");
            DropIndex("dbo.ProblemUser", new[] { "User_UserID" });
            DropIndex("dbo.ProblemUser", new[] { "Problem_ProblemID" });
            DropTable("dbo.ProblemUser");
        }
    }
}
