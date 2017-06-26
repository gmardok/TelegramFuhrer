using System.Threading.Tasks;
using TelegramFuhrer.BL.Models;
using TelegramFuhrer.BL.Services;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.Commands.ChatCommands
{
	public class ChatAddCommand : ChatAddRemoveCommand
	{
		private readonly IChatService _chatService;

		public ChatAddCommand(IChatService chatService)
		{
			_chatService = chatService;
		}

		protected override Task<ChatActionResult> ActionAsync(string title, string username)
		{
			return _chatService.AddUserAsync(title, username, User);
		}

	    protected override Task ActionAsync(Chat chat, User user)
		{
			return _chatService.AddUserAsync(chat, user);
		}

		protected override string SuccessMessage(string username)
		{
			return $"User {username} added";
		}
	}
}
