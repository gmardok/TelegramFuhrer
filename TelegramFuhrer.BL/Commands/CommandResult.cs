using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramFuhrer.BL.Commands
{
	public class CommandResult
	{
		public bool Success { get; set; }

		public string Message { get; set; }

		public Func<string, Task<CommandResult>> NextAction { get; set; }
	}
}
