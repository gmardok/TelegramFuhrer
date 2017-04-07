namespace TelegramFuhrer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatAutoRemove : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chat", "AutoRemove", c => c.Boolean(nullable: false));
            Sql("update chat set AutoRemove = AutoAdd");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chat", "AutoRemove");
        }
    }
}
