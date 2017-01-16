using System.ComponentModel.DataAnnotations;
using TeleSharp.TL;

namespace TelegramFuhrer.Data.Entities
{
	public class Chat
	{
		[Key]
		[Required]
		public int Id { get; set; }

		public string Title { get; set; }

		public Chat() { }

		public Chat(TLChat tlChat)
		{
			Copy(tlChat);
		}

		public void Copy(TLChat chat)
		{
			Id = chat.id;
			Title = chat.title;
		}
	}
}
