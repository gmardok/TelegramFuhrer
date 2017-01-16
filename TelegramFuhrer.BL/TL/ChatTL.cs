using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramFuhrer.Data.Entities;
using TeleSharp.TL;
using TeleSharp.TL.Messages;
using TLSharp.Core;

namespace TelegramFuhrer.BL.TL
{
	public class ChatTL : IChatTL
	{
		private readonly TelegramClient _telegramClient;

		public ChatTL(TelegramClient telegramClient)
		{
			_telegramClient = telegramClient;
		}

		public async Task<IList<Chat>> FindByTitleAsync(string keywords)
		{
			var rd = new TLRequestGetDialogs
			{
				offset_date = 0,
				offset_id = 0,
				offset_peer = new TLInputPeerEmpty(),
				limit = 10
			};

			var dialogs = await _telegramClient.SendRequestAsync<TLDialogs>(rd);

			return dialogs.chats.lists.OfType<TLChat>().Where(c => c.title.IndexOf(keywords, StringComparison.OrdinalIgnoreCase) >= 0)
				.Select(c => new Chat(c)).ToList();
		}

		public async Task AddUserAsync(Chat chat, User user)
		{
			if (!user.AccessHash.HasValue)
			{
				throw new ArgumentException("Usename was not found: " + user.Username);
			}

			var r = new TLRequestAddChatUser
			{
				user_id = new TLInputUser { user_id = user.Id, access_hash = user.AccessHash.Value },
				chat_id = chat.Id
			};

			await _telegramClient.SendRequestAsync<object>(r);
		}

		public async Task RemoveUserAsync(Chat chat, User user)
		{
			if (!user.AccessHash.HasValue)
			{
				throw new ArgumentException("Usename was not found: " + user.Username);
			}

			var r = new TLRequestDeleteChatUser
			{
				user_id = new TLInputUser { user_id = user.Id, access_hash = user.AccessHash.Value },
				chat_id = chat.Id
			};

			await _telegramClient.SendRequestAsync<object>(r);
		}
	}
}
