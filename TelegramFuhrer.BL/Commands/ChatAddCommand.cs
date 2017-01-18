using System.Threading.Tasks;
using TelegramFuhrer.BL.Services;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.Commands
{
	public class ChatAddCommand : ChatAddRemoveCommand, ICommand
	{
		private readonly IChatService _chatService;

		public ChatAddCommand(IChatService chatService)
		{
			_chatService = chatService;
		}

		protected override Task<ChatActionResult> ActionAsync(string title, string username)
		{
			return _chatService.AddUserAsync(title, username);
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
