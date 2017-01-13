using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeleSharp.TL;

namespace TelegramFuhrer.Data.Entities
{
	public class UserChat
	{
		[Required]
		[ForeignKey("User")]
		public int UserId { get; set; }

		[Required]
		public User User { get; set; }

		[Required]
		[ForeignKey("Chat")]
		public int ChatId { get; set; }

		[Required]
		public TLChat Chat { get; set; }
	}
}
