using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramFuhrer.BL.Services;

namespace TelegramFuhrer.BL.Commands.ChatCommands
{
    public class ChatListCommand : ICommand
    {
        private readonly IChatService _chatService;

        public ChatListCommand(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<CommandResult> Execute(string args)
        {
            return new CommandResult
            {
                Message = await _chatService.GetChatList(),
                Success = true
            };
        }
    }
}
