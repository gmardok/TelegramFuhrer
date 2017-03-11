using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramFuhrer.BL.Services;

namespace TelegramFuhrer.BL.Commands.UserCommands
{
    public class AdminAddRemoveCommand : ICommand
    {
        private readonly IUserService _userService;

        private readonly bool _isAdmin;

        public AdminAddRemoveCommand(IUserService userService, bool isAdmin)
        {
            _userService = userService;
            _isAdmin = isAdmin;
        }

        public async Task<CommandResult> Execute(string args)
        {
            await _userService.FindUserByUsernameAsync(args, _isAdmin);
            return new CommandResult
            {
                Message = "Done",
                Success = true
            };
        }
    }
}
