using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramFuhrer.BL.Commands
{
    public class CommandsCommand : ICommand
    {
        public Task<CommandResult> Execute(string args)
        {
            var commands = new StringBuilder();
            commands.AppendLine("help, h, ? - List of commands");
            commands.AppendLine("chatadd <user> <chat title> - Add user to chat");
            commands.AppendLine("chatremove <user> <chat title> - Remove user from chat");
            commands.AppendLine("chataddall <user> - Add user to all chats with autoadd property");
            commands.AppendLine("chatremoveall <user> - Remove user from all chats with autoadd property");
            commands.AppendLine("chatcreate <user> <chat title> - Create chat with user and admin itself");
            commands.AppendLine("chatlist - list of registered chats with properties");
            commands.AppendLine("chatedit <true> <true> <id> - change autoadd or autokick property (use true/false or 1/0) for chat. Id can be taken from chatlist");
            commands.AppendLine("chatregister <title> - register existing chat");
            commands.AppendLine("adminadd <user> - add user to admins");
            commands.AppendLine("adminremove <user> - remove user from admins");
            commands.AppendLine("adminlist - list of admin users");
            return Task.FromResult(new CommandResult
            {
                Success = true,
                Message = commands.ToString()
            });
        }
    }
}
