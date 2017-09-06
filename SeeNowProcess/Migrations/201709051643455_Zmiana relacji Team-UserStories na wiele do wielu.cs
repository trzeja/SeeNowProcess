namespace SeeNowProcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianarelacjiTeamUserStoriesnawieledowielu : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Team", "UserStory_UserStoryID", "dbo.UserStory");
            DropIndex("dbo.Team", new[] { "UserStory_UserStoryID" });
            CreateTable(
                "dbo.UserStoryTeam",
                c => new
                    {
                        UserStory_UserStoryID = c.Int(nullable: false),
                        Team_TeamID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserStory_UserStoryID, t.Team_TeamID })
                .ForeignKey("dbo.UserStory", t => t.UserStory_UserStoryID, cascadeDelete: true)
                .ForeignKey("dbo.Team", t => t.Team_TeamID, cascadeDelete: true)
                .Index(t => t.UserStory_UserStoryID)
                .Index(t => t.Team_TeamID);
            
            DropColumn("dbo.Team", "UserStory_UserStoryID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Team", "UserStory_UserStoryID", c => c.Int());
            DropForeignKey("dbo.UserStoryTeam", "Team_TeamID", "dbo.Team");
            DropForeignKey("dbo.UserStoryTeam", "UserStory_UserStoryID", "dbo.UserStory");
            DropIndex("dbo.UserStoryTeam", new[] { "Team_TeamID" });
            DropIndex("dbo.UserStoryTeam", new[] { "UserStory_UserStoryID" });
            DropTable("dbo.UserStoryTeam");
            CreateIndex("dbo.Team", "UserStory_UserStoryID");
            AddForeignKey("dbo.Team", "UserStory_UserStoryID", "dbo.UserStory", "UserStoryID");
        }
    }
}
