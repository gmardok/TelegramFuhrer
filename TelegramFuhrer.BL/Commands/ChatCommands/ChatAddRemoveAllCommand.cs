using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramFuhrer.BL.Services;
using TelegramFuhrer.Data.Entities;
using TelegramFuhrer.Data.Repositories;

namespace TelegramFuhrer.BL.Commands.ChatCommands
{
    public class ChatAddRemoveAllCommand : ICommand
    {
        private readonly IChatService _chatService;

        private readonly IUserService _userService;

        private readonly ChatRepository _chatRepository;

        private readonly bool _isAdd;

        public ChatAddRemoveAllCommand(IChatService chatService, IUserService userService, ChatRepository chatRepository, bool isAdd)
        {
            _isAdd = isAdd;
            _chatService = chatService;
            _userService = userService;
            _chatRepository = chatRepository;
        }

        public async Task<CommandResult> Execute(string args)
        {
            var chats = await (_isAdd ? _chatRepository.GetAutoAddAsync() : _chatRepository.GetAutoRemoveAsync());
            var user = await _userService.FindUserByUsernameAsync(args.TrimStart("@"));
            foreach (var chat in chats)
            {
                try
                {
                    if (_isAdd)
                        await _chatService.AddUserAsync(chat, user);
                    else
                        await _chatService.RemoveUserAsync(chat, user);
                }
                catch (InvalidOperationException ex)
                {
                    if (ex.Message == "USER_ALREADY_PARTICIPANT" || ex.Message == "USER_NOT_PARTICIPANT") continue;
                    throw;
                }
            }

            return new CommandResult
            {
                Message = "Done",
                Success = true
            };
        }

        public User User { get; set; }

        public bool RequireAdmin => true;
    }
}
