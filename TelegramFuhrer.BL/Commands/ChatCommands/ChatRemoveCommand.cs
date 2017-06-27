using System.Threading.Tasks;
using TelegramFuhrer.BL.Models;
using TelegramFuhrer.BL.Services;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.Commands.ChatCommands
{
	public class ChatRemoveCommand : ChatAddRemoveCommand
	{
		private readonly IChatService _chatService;

		private readonly IChannelService _channelService;

		public ChatRemoveCommand(IChatService chatService, IChannelService channelService)
		{
			_chatService = chatService;
			_channelService = channelService;
		}

		protected override Task<ChatActionResult> ActionAsync(string title, string username)
		{
			return _chatService.RemoveUserAsync(title, username, User);
		}

		protected override Task ActionAsync(Chat chat, User user)
		{
			return _chatService.RemoveUserAsync(chat, user);
		}

		protected override string SuccessMessage(string username)
		{
			return $"User {username} removed";
		}
	}
}
