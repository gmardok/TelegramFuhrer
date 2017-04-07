using System.Threading.Tasks;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.BL.Commands
{
    public interface ICommand
	{
		Task<CommandResult> Execute(string args);

        User User { get; set; }

        bool RequireAdmin { get; }
	}
}
