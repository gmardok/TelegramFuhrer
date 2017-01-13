using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
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

		public DbSet<TLChat> Chats { get; set; }

		public DbSet<UserChat> UserChats { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			// Configure Code First to ignore PluralizingTableName convention 
			// If you keep this convention then the generated tables will have pluralized names. 
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			modelBuilder.Entity<TLChat>().HasKey(t => t.id);
		}
	}
}
