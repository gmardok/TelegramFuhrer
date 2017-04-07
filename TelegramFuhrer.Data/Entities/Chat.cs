using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeleSharp.TL;

namespace TelegramFuhrer.Data.Entities
{
	public class Chat
	{
		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		public string Title { get; set; }

        [Column("AutoCick")]
        public bool AutoKick { get; set; }

        public bool AutoAdd { get; set; }

        public bool AutoRemove { get; set; }

        public ICollection<UserChat> UserChats { get; set; }

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
