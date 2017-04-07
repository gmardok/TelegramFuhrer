using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramFuhrer.BL.Services;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.Commands.UserCommands
{
    public class UpdateUsersHashCommand : ICommand
    {
        private IUserService _userService;

        public UpdateUsersHashCommand(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<CommandResult> Execute(string args)
        {
            await _userService.UpdateHashes();
            return new CommandResult
            {
                Message = "Done.",
                Success = true
            };
        }

        public User User { get; set; }

        public bool RequireAdmin => true;
    }
}
