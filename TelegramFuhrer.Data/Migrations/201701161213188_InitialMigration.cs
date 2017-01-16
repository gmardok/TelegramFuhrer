namespace TelegramFuhrer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chat",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: false),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserChat",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ChatId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Chat", t => t.ChatId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ChatId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        IsGlobalAdmin = c.Boolean(nullable: false),
                        Id = c.Int(nullable: false),
                        AccessHash = c.Long(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Username = c.String(nullable: false),
                        Phone = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserChat", "UserId", "dbo.User");
            DropForeignKey("dbo.UserChat", "ChatId", "dbo.Chat");
            DropIndex("dbo.UserChat", new[] { "ChatId" });
            DropIndex("dbo.UserChat", new[] { "UserId" });
            DropTable("dbo.User");
            DropTable("dbo.UserChat");
            DropTable("dbo.Chat");
        }
    }
}
