using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelegramFuhrer.Data.Entities
{
	public class UserChat
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[ForeignKey("User")]
		public int UserId { get; set; }

		public User User { get; set; }

		[Required]
		[ForeignKey("Chat")]
		public int ChatId { get; set; }

		public Chat Chat { get; set; }
	}
}
