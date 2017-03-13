using System;
using System.Text;
using System.Threading.Tasks;
using TelegramFuhrer.BL.Models;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.Commands.ChatCommands
{
	public abstract class ChatAddRemoveCommand : ICommand
	{
		public async Task<CommandResult> Execute(string args)
		{
			var argsArray = args.Split(new [] {' '}, 2);
			if (argsArray.Length <= 1 || string.IsNullOrEmpty(argsArray[1])) throw new CommandException("Command expect user name and chat title");
			var username = argsArray[0].TrimStart("@");
			var chatTitle = argsArray[1];
			var result = await ActionAsync(chatTitle, username);
			if (result.Success)
				return new CommandResult { Success = true, Message = SuccessMessage(username) };

			var sb = new StringBuilder("Several chats found. Select one by its number:");
			var num = 1;
			foreach (var chat in result.Chats)
			{
				sb.AppendFormat("\n{0}. {1}", num, chat.Title);
				num++;
			}

			Func<string, Task<CommandResult>> nextAction = null;
			nextAction = async s =>
			{
				if (!int.TryParse(s, out num) || num <= 0 || num > result.Chats.Count)
					return new CommandResult
					{
						Success = false,
						Message = "Input chat number",
						NextAction = nextAction
					};

				await ActionAsync(result.Chats[num - 1], result.User);
				return new CommandResult
				{
					Success = true,
					Message = SuccessMessage(username)
				};
			};

			return new CommandResult
			{
				Success = false,
				Message = sb.ToString(),
				NextAction = nextAction
			};
		}

		protected abstract Task<ChatActionResult> ActionAsync(string title, string username);

		protected abstract Task ActionAsync(Chat chat, User user);

		protected abstract string SuccessMessage(string username);
	}
}
