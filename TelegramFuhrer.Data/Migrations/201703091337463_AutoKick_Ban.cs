namespace TelegramFuhrer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AutoKick_Ban : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chat", "AutoAdd", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chat", "AutoAdd");
        }
    }
}
