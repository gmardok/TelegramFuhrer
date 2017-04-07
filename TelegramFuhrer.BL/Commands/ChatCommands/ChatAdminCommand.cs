using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramFuhrer.BL.Services;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.Commands.ChatCommands
{
    public class ChatAdminCommand : ICommand
    {
        private readonly ChatService _chatService;

        public ChatAdminCommand(ChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<CommandResult> Execute(string args)
        {
            var argsArray = args.Split(new[] { ' ' }, 3);
            if (argsArray.Length <= 2 || string.IsNullOrEmpty(argsArray[2])) throw new CommandException("Command expect action(add/remove), user name and chat title");
            var command = argsArray[0].ToLower();
            var username = argsArray[1].TrimStart("@");
            var chatTitle = argsArray[2];

            if (command != "add" && command != "remove") throw new CommandException("Action can be add or remove");

            var result = await (command == "add" ? _chatService.AddChatAdminAsync(chatTitle, username) : _chatService.RemoveChatAdminAsync(chatTitle, username));
            if (result.Success)
                return new CommandResult { Success = true, Message = "Done." };

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

                await
                    (command == "add"
                        ? _chatService.AddChatAdminAsync(result.Chats[num - 1], result.User)
                        : _chatService.RemoveChatAdminAsync(result.Chats[num - 1], result.User));
                return new CommandResult
                {
                    Success = true,
                    Message = "Done."
                };
            };

            return new CommandResult
            {
                Success = false,
                Message = sb.ToString(),
                NextAction = nextAction
            };

        }

        public User User { get; set; }

        public bool RequireAdmin => true;
    }
}
