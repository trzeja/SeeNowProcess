namespace SeeNowProcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userStoryRestrictions : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserStory", "Title", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.UserStory", "Description", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.UserStory", "Size", c => c.Int(nullable: false));
            AlterColumn("dbo.UserStory", "Unit", c => c.String(nullable: false));
            AlterColumn("dbo.UserStory", "Notes", c => c.String(maxLength: 200));
            AlterColumn("dbo.UserStory", "Criteria", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserStory", "Criteria", c => c.String());
            AlterColumn("dbo.UserStory", "Notes", c => c.String());
            AlterColumn("dbo.UserStory", "Unit", c => c.String());
            AlterColumn("dbo.UserStory", "Size", c => c.Int());
            AlterColumn("dbo.UserStory", "Description", c => c.String());
            AlterColumn("dbo.UserStory", "Title", c => c.String(maxLength: 60));
        }
    }
}
