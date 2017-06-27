namespace TelegramFuhrer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatIsChannel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chat", "IsChannel", c => c.Boolean(nullable: false));
			Sql("update dbo.Chat set IsChannel = 0");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chat", "IsChannel");
        }
    }
}
