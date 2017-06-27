namespace TelegramFuhrer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatAccessHash : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chat", "AccessHash", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chat", "AccessHash");
        }
    }
}
