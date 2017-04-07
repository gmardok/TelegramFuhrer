using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramFuhrer.BL.Services;
using TelegramFuhrer.BL.TL;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.Commands.ChatCommands
{
    public class ChatMessageCommad :  ICommand
    {
        private readonly MessagesService _messagesService;

        public ChatMessageCommad(MessagesService messagesService)
        {
            _messagesService = messagesService;
        }

        public async Task<CommandResult> Execute(string args)
        {
            var argsArray = args.Split(new[] { ' ' }, 2);
            if (argsArray.Length != 4)
                throw new ArgumentException("Incorrect command parameters. Should contains chat id and message");

            var chatid = int.Parse(argsArray[0]);
            var message = argsArray[1];
            await _messagesService.SendChatMessageAsync(chatid, message);
            return new CommandResult {Message = "Done.", Success = true};
        }

        public User User { get; set; }

        public bool RequireAdmin => true;
    }
}
