using System.Threading.Tasks;
using TelegramFuhrer.BL.Models;
using TelegramFuhrer.BL.Services;

namespace TelegramFuhrer.BL.Commands.ChannelCommands
{
	public class ChannelAddCommand : ChannelAddRemoveCommand
	{
		private readonly IChannelService _channelService;

		public ChannelAddCommand(IChannelService channelService)
		{
			_channelService = channelService;
		}

		protected override Task<ChatActionResult> ActionAsync(string channel, string username)
		{
			return _channelService.AddUserAsync(channel, username, User);
		}

		protected override string SuccessMessage(string username)
		{
			return $"User {username} added";
		}
	}
}
