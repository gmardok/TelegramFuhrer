using System.Collections.Generic;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.Services
{
	public class ChatActionResult
	{
		public bool Success { get; set; }

		public IList<Chat> Chats { get; set; }

		public User User { get; set; }
	}
}
