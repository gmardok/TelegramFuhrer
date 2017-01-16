using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeleSharp.TL;

namespace TelegramFuhrer.Data.Entities
{
	public class User
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int UserId { get; set; }

		[Required]
		public bool IsGlobalAdmin { get; set; }

		[Required]
		public int Id { get; set; }

		public long? AccessHash { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		[Required]
		public string Username { get; set; }

		public string Phone { get; set; }

		public ICollection<UserChat> UserChats { get; set; }
	}
}
