using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramFuhrer.Data.Entities;
using TeleSharp.TL;

namespace TelegramFuhrer.Data
{
    public class FuhrerContext : DbContext
    {
		public DbSet<User> Users { get; set; }

		public DbSet<Chat> Chats { get; set; }

		public DbSet<UserChat> UserChats { get; set; }

	    public FuhrerContext() : base()
	    {
			//configure app domain
			string absolute = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data"));
			AppDomain.CurrentDomain.SetData("DataDirectory", absolute);

			//create or update db with lates migrations
			Database.SetInitializer(new System.Data.Entity.MigrateDatabaseToLatestVersion<FuhrerContext, Migrations.Configuration>());

			var configuration = new Migrations.Configuration();
			var migrator = new System.Data.Entity.Migrations.DbMigrator(configuration);
			if (migrator.GetPendingMigrations().Any())
			{
				migrator.Update();
			}
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			// Configure Code First to ignore PluralizingTableName convention 
			// If you keep this convention then the generated tables will have pluralized names. 
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}
	}
}
