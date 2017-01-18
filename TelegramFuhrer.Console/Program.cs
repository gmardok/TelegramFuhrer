using Microsoft.Practices.Unity;
using System;
using System.Threading.Tasks;
using TelegramFuhrer.BL;
using TelegramFuhrer.BL.Commands;
using TelegramFuhrer.BL.Services;
using TelegramFuhrer.Data;
using TelegramFuhrer.Data.Entities;

namespace TelegramFuhrer.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var task = StartBot();
			task.Wait();
		}

		static async Task StartBot()
		{
			using (var container = new UnityContainer())
			{
				FuhrerContext.Init();
				await BL.Bootstrap.RegisterTypesAsync(container);
				Data.Bootstrap.RegisterTypes(container);
				System.Console.WriteLine("Input command in formet \"<command> <user name> <chat title>\"");
				var commandLine = string.Empty;
				while (string.IsNullOrEmpty(commandLine) || !commandLine.Equals("exit", StringComparison.OrdinalIgnoreCase))
				{
					commandLine = System.Console.ReadLine();
					if (string.IsNullOrEmpty(commandLine)) continue;
					var commandLineArray = commandLine.Split(' ');
					if (commandLineArray.Length < 2)
					{
						System.Console.WriteLine("Command arguments requiered");
						continue;
					}

					var command = commandLineArray[0];
					ICommand cmd;
					try
					{
						cmd = container.Resolve<ICommand>(command.ToLower());
					}
					catch 
					{
						System.Console.WriteLine("Incorrect command.");
						continue;
					}

					try
					{
						var cmdResult = await cmd.Execute(commandLine.TrimStart(command + " "));
						System.Console.WriteLine(cmdResult.Message);
						while (!cmdResult.Success)
						{
							cmdResult = await cmdResult.NextAction(System.Console.ReadLine());
							System.Console.WriteLine(cmdResult.Message);
						}
					}
					catch (Exception ex)
					{
						System.Console.WriteLine(ex.Message);
					}
				}
			}
		}
	}
}
