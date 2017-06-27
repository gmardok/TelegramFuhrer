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

		public bool IsChannel { get; set; }

		public long? AccessHash { get; set; }

		public ICollection<UserChat> UserChats { get; set; }

        public Chat() { }

		public Chat(TLChat tlChat)
		{
			Copy(tlChat);
		}

		public Chat(TLChannel tlChannel)
		{
			Id = tlChannel.id;
			Title = tlChannel.title;
			AccessHash = tlChannel.access_hash;
			IsChannel = true;
		}

		public void Copy(TLChat chat)
		{
			Id = chat.id;
			Title = chat.title;
			IsChannel = false;
		}
	}
}
