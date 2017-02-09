using System.Threading.Tasks;
using TelegramFuhrer.BL.Models;
using TelegramFuhrer.BL.Services;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.Commands
{
	public class ChatRemoveCommand : ChatAddRemoveCommand, ICommand
	{
		private readonly IChatService _chatService;

		public ChatRemoveCommand(IChatService chatService)
		{
			_chatService = chatService;
		}

		protected override Task<ChatActionResult> ActionAsync(string title, string username)
		{
			return _chatService.RemoveUserAsync(title, username);
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
