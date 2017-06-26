using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramFuhrer.Data.Entities;
using TeleSharp.TL;
using TeleSharp.TL.Channels;
using TeleSharp.TL.Messages;

namespace TelegramFuhrer.BL.TL
{
	public class ChannelTL : IChannelTL
	{
		private readonly TelegramClientEx _telegramClient;

		public ChannelTL(TelegramClientEx telegramClient)
		{
			_telegramClient = telegramClient;
		}

		public async Task<IList<TLChannel>> FindByTitleAsync(string keywords)
		{
			var rd = new TLRequestGetDialogs
			{
				offset_date = 0,
				offset_id = 0,
				offset_peer = new TLInputPeerEmpty(),
				limit = 100
			};


			var dialogs = await _telegramClient.SendRequestAsync<TLAbsDialogs>(rd);

			if (dialogs is TLDialogs)
				return ((TLDialogs) dialogs).chats.lists.OfType<TLChannel>()
					.Where(c => c.title.IndexOf(keywords, StringComparison.OrdinalIgnoreCase) >= 0)
					.ToList();
			return ((TLDialogsSlice) dialogs).chats.lists.OfType<TLChannel>()
				.Where(c => c.title.IndexOf(keywords, StringComparison.OrdinalIgnoreCase) >= 0)
				.ToList();
		}

		public async Task AddUserAsync(TLChannel channel, User user)
		{
			if (!channel.access_hash.HasValue) return;
			CheckUser(user);

			var r = new TLRequestInviteToChannel
			{
				channel = new TLInputChannel { channel_id = channel.id, access_hash = channel.access_hash.Value},
				users = new TLVector<TLAbsInputUser> { lists = new List<TLAbsInputUser>{ new TLInputUser { user_id = user.Id, access_hash = user.AccessHash.Value } } }
			};

			await _telegramClient.SendRequestAsync<object>(r);
		}

		public async Task RemoveUserAsync(TLChannel channel, User user)
		{
			if (!channel.access_hash.HasValue) return;
			CheckUser(user);

			var r = new TLRequestKickFromChannel
			{
				channel = new TLInputChannel { channel_id = channel.id, access_hash = channel.access_hash.Value },
				user_id = new TLInputUser { user_id = user.Id, access_hash = user.AccessHash.Value }
			};

			var result = await _telegramClient.SendRequestAsync<object>(r);
		}

		private void CheckUser(User user)
		{
			if (user == null)
				throw new ArgumentException("User doesnot exists");
			if (!user.AccessHash.HasValue)
				throw new ArgumentException("Usename was not found: " + user.Username);
		}
	}
}
