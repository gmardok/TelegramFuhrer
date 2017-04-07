using System.Threading.Tasks;
using TelegramFuhrer.BL.Services;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.Commands.ChatCommands
{
    public class ChatRegisterCommand : ICommand
    {
        private readonly IChatService _chatService;

        public ChatRegisterCommand(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<CommandResult> Execute(string args)
        {
            await _chatService.RegisterChat(args);

            return new CommandResult
            {
                Success = true,
                Message = "Done"
            };
        }

        public User User { get; set; }
        public bool RequireAdmin => true;
    }
}
