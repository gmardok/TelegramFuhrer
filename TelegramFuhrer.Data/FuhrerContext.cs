using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using System.Linq;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.Data
{
    public class FuhrerContext : DbContext
    {
		public DbSet<User> Users { get; set; }

		public DbSet<Chat> Chats { get; set; }

		public DbSet<UserChat> UserChats { get; set; }

	    public FuhrerContext() : base(GetConnectionString())
        {
        }

        public FuhrerContext(string connectionString) : base(connectionString)
        {
        }

        public static void Init()
	    {
			//configure app domain
			AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);

			//create or update db with lates migrations
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<FuhrerContext, Migrations.Configuration>());

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

        private static string GetConnectionString()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["FuhrerContext"].ToString();
            if (connectionString.IndexOf("{0}", StringComparison.Ordinal) == -1) return "FuhrerContext";

            var dbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fuhrer.mdf");

            return string.Format(connectionString, dbFile);
        }
    }
}
