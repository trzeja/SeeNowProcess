namespace SeeNowProcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SimpleTasks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Importance = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        CurrentState = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        CompletionDate = c.DateTime(nullable: false),
                        Progress = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SimpleTasks");
        }
    }
}
