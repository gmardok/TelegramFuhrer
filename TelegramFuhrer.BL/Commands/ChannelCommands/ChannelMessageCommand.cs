using System;
using System.Threading.Tasks;
using TelegramFuhrer.BL.Services;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.Commands.ChannelCommands
{
	public class ChannelMessageCommand : ICommand
	{
		private readonly MessagesService _messagesService;

		public ChannelMessageCommand(MessagesService messagesService)
		{
			_messagesService = messagesService;
		}

		public async Task<CommandResult> Execute(string args)
		{
			var argsArray = args.Split(new[] { ' ' }, 2);
			if (argsArray.Length != 2)
				throw new ArgumentException("Incorrect command parameters. Should contains chat id and message");

			var channel = argsArray[0];
			var message = argsArray[1];
			var result = await _messagesService.SendChannelMessageAsync(channel, message);
			return new CommandResult { Message = result ? "Done." : $"Channel {channel} not found.", Success = true };
		}

		public User User { get; set; }

		public bool RequireAdmin => true;
	}
}
