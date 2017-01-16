using Microsoft.Practices.Unity;
using System;
using System.Threading.Tasks;
using TelegramFuhrer.BL;
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
						System.Console.WriteLine("User name and chat title required");
						continue;
					}

					if (commandLineArray.Length < 3)
					{
						System.Console.WriteLine("Chat title requiered");
						continue;
					}

					var command = commandLineArray[0];
					var chatService = container.Resolve<IChatService>();
					var username = commandLine.TrimStart(command + " ").Split(' ')[0];
					var chatTitle = commandLine.TrimStart($"{command} {username} ");
					try
					{
						Func<string, string, Task<ChatActionResult>> action1;
						Func<Chat, User, Task> action2;

						switch (command.ToLower())
						{
							case "chatadd":
								action1 = async (title, user) => await chatService.AddUserAsync(title, user);
								action2 = async (chat, user) => await chatService.AddUserAsync(chat, user);
								break;
							case "chatremove":
								action1 = async (title, user) => await chatService.RemoveUserAsync(title, user);
								action2 = async (chat, user) => await chatService.RemoveUserAsync(chat, user);
								break;
							default:
								continue;
						}

						var result = await action1(chatTitle, username);
						if (result.Success)
						{
							System.Console.WriteLine("User {0} added");
							continue;
						}

						System.Console.WriteLine("Several chats found. Select one by its number:");
						var num = 1;
						foreach (var chat in result.Chats)
						{
							System.Console.WriteLine("{0}. {1}", num, chat.Title);
							num++;
						}

						while (!ReadNumber(result.Chats.Count, out num))
						{
							System.Console.WriteLine("Input chat number");
						}

						await action2(result.Chats[num - 1], result.User);
						System.Console.WriteLine("Success. Waiting for next command.");
					}
					catch (Exception ex)
					{
						System.Console.WriteLine(ex.Message);
					}
				}
			}
		}

		private static bool ReadNumber(int max, out int num)
		{
			while (!int.TryParse(System.Console.ReadLine(), out num))
			{
				System.Console.WriteLine("Input chat number");
			}

			return num > 0 && num <= max;
		}
	}
}
