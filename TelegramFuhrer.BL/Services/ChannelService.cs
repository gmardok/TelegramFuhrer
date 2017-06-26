using System.Threading.Tasks;
using TelegramFuhrer.BL.Models;
using TelegramFuhrer.BL.TL;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.Services
{
	public class ChannelService : IChannelService
	{
		private readonly IUserService _userService;

		private readonly IChannelTL _channelTL;

		public ChannelService(IUserService userService, IChannelTL channelTL)
		{
			_userService = userService;
			_channelTL = channelTL;
		}

		public async Task<ChatActionResult> AddUserAsync(string channelName, string username, User actionUser)
		{
			var user = await _userService.FindUserByUsernameAsync(username);
			var channels = await _channelTL.FindByTitleAsync(channelName);
			var errorResult = new ChatActionResult {Success = false};
			if (channels.Count == 0) return errorResult;
			var channel = channels[0];
			if (!channel.access_hash.HasValue) return errorResult;

			await _channelTL.AddUserAsync(channel, user);
			return new ChatActionResult { Success = true };
		}

		public async Task<ChatActionResult> RemoveUserAsync(string channelName, string username, User actionUser)
		{
			var user = await _userService.FindUserByUsernameAsync(username);
			var channels = await _channelTL.FindByTitleAsync(channelName);
			var errorResult = new ChatActionResult {Success = false};
			if (channels.Count == 0) return errorResult;
			var channel = channels[0];
			if (!channel.access_hash.HasValue) return errorResult;

			await _channelTL.RemoveUserAsync(channel, user);
			return new ChatActionResult { Success = true };
		}
	}
}
