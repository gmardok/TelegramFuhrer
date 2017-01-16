using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace TelegramFuhrer.Console
{
	class Program
	{
		static async void Main(string[] args)
		{
			using (var container = new UnityContainer())
			{
				await BL.Bootstrap.RegisterTypesAsync(container);

				System.Console.WriteLine("Done");
			    var command = string.Empty;
			    while (string.IsNullOrEmpty(command) || !command.Equals("exit", StringComparison.OrdinalIgnoreCase))
			    {
			        command = System.Console.ReadLine();
			    }
			}
		}
	}
}
