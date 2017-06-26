using System.Threading.Tasks;
using TelegramFuhrer.BL.Models;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.Commands.ChannelCommands
{
	public abstract class ChannelAddRemoveCommand : ICommand
	{
		public async Task<CommandResult> Execute(string args)
		{
			var argsArray = args.Split(new[] { ' ' }, 2);
			if (argsArray.Length <= 1 || string.IsNullOrEmpty(argsArray[1])) throw new CommandException("Command expect user name and channel name");
			var username = argsArray[0].TrimStart("@");
			var channel = argsArray[1];
			var result = await ActionAsync(channel, username);
			if (result.Success)
				return new CommandResult { Success = true, Message = SuccessMessage(username) };
			return new CommandResult { Success = true, Message = "Something went wrong." };
		}

		public User User { get; set; }

		public bool RequireAdmin => true;

		protected abstract Task<ChatActionResult> ActionAsync(string channel, string username);

		protected abstract string SuccessMessage(string username);
	}
}
