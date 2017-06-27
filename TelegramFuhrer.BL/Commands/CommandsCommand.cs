using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.Commands
{
    public class CommandsCommand : ICommand
    {
        public Task<CommandResult> Execute(string args)
        {
            var commands = new StringBuilder();
            commands.AppendLine("help, h, ? - List of commands");
            commands.AppendLine("chatadd <user> <chat title> - Add user to chat. Can be used with chatadd or just add");
            commands.AppendLine("chatremove <user> <chat title> - Remove user from chat. Can be used with chatremove or kick");
            if (User.IsGlobalAdmin)
            {
                commands.AppendLine("chataddall <user> - Add user to all chats with autoadd property");
                commands.AppendLine("chatremoveall <user> - Remove user from all chats with autoadd property");
                commands.AppendLine("chatcreate <user> <chat title> - Create chat with user and admin itself");
                commands.AppendLine("chatlist - list of registered chats with properties");
                commands.AppendLine(
                    "chatedit <id> <autokick> <autoadd> <autoremove> - change autoadd, autoremove and autokick property (use true/false or 1/0) for chat. Id can be taken from chatlist");
				commands.AppendLine("chatregister <title> - register existing chat");
                commands.AppendLine("msg <chat id> <message> - send message to the chat. Chat id can be taken from chatlist");
                commands.AppendLine("adminadd <user> - add user to admins");
                commands.AppendLine("adminremove <user> - remove user from admins");
                commands.AppendLine("adminlist - list of admin users");
                commands.AppendLine("chatadmin <action> <user> <chat title> - add (action - add) or remove (action - remove) chat administrator");
	            commands.AppendLine("channelmsg <channel title> <message> - send message to the channel or suppergroup.");
            }
			return Task.FromResult(new CommandResult
            {
                Success = true,
                Message = commands.ToString()
            });
        }

        public User User { get; set; }

        public bool RequireAdmin => false;
    }
}
