using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramFuhrer.BL.Commands
{
	public interface ICommand
	{
		Task<CommandResult> Execute(string args);
	}
}
