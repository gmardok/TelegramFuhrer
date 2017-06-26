using System.Threading.Tasks;
using TelegramFuhrer.BL.Models;
using TelegramFuhrer.BL.Services;

namespace TelegramFuhrer.BL.Commands.ChannelCommands
{
	public class ChannelRemoveCommand : ChannelAddRemoveCommand
	{
		private readonly IChannelService _channelService;

		public ChannelRemoveCommand(IChannelService channelService)
		{
			_channelService = channelService;
		}

		protected override Task<ChatActionResult> ActionAsync(string channel, string username)
		{
			return _channelService.RemoveUserAsync(channel, username, User);
		}

		protected override string SuccessMessage(string username)
		{
			return $"User {username} removed";
		}
	}
}
