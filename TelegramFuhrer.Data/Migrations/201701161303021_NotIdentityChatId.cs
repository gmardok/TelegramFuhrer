namespace TelegramFuhrer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotIdentityChatId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserChat", "ChatId", "dbo.Chat");
            DropPrimaryKey("dbo.Chat");
            AlterColumn("dbo.Chat", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Chat", "Id");
            AddForeignKey("dbo.UserChat", "ChatId", "dbo.Chat", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserChat", "ChatId", "dbo.Chat");
            DropPrimaryKey("dbo.Chat");
            AlterColumn("dbo.Chat", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Chat", "Id");
            AddForeignKey("dbo.UserChat", "ChatId", "dbo.Chat", "Id", cascadeDelete: true);
        }
    }
}
