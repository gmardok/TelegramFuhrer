using System.Threading.Tasks;
using TelegramFuhrer.BL.Services;
using TelegramFuhrer.BL.TL;

namespace TelegramFuhrer.BL.Commands.ChatCommands
{
	public class ChatCreateCommand : ICommand
	{
		private readonly IChatTL _chatTL;

		private readonly IUserService _userService;

		public ChatCreateCommand(IChatTL chatTL, IUserService userService)
		{
			_chatTL = chatTL;
			_userService = userService;
		}

		public async Task<CommandResult> Execute(string args)
		{
			var argsArray = args.Split(' ');
			if (argsArray.Length <= 1) throw new CommandException("Command expect user name and chat title");
			var username = argsArray[0].TrimStart("@");
			var user = await _userService.FindUserByUsernameAsync(username);
			var chatTitle = args.TrimStart($"{username} ");
			await _chatTL.CreateChatAsync(chatTitle, user);
			return new CommandResult {Success = true, Message = $"Chat {chatTitle} created successfully!"};
		}
	}
}
