using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.Practices.Unity;
using TelegramFuhrer.BL.Commands;
using TelegramFuhrer.BL.Commands.ChatCommands;
using TelegramFuhrer.BL.Commands.UserCommands;
using TelegramFuhrer.BL.Services;
using TelegramFuhrer.BL.TL;
using TelegramFuhrer.Data.Repositories;
using TLSharp.Core;

namespace TelegramFuhrer.BL
{
	public static class Bootstrap
	{
		private static string ApiHash => ConfigurationManager.AppSettings["ApiHash"];

		private static int ApiId => int.Parse(ConfigurationManager.AppSettings["ApiId"]);

		public static async Task RegisterTypesAsync(IUnityContainer container)
		{
            TelegramClientEx client;
			try
			{
				client = new TelegramClientEx(ApiId, ApiHash, new ServiceSessionStore());
				await client.ConnectAsync();
			}
			catch (MissingApiConfigurationException ex)
			{
                LogManager.GetLogger(typeof(Bootstrap)).Error("Telegram client initialization", ex);
				throw new Exception($"Please add your API settings to the `app.config` file. (More info: {MissingApiConfigurationException.InfoUrl})",
									ex);
			}

			container.RegisterInstance(typeof (TelegramClientEx), client);
			container.RegisterType<IUserTL, UserTL>();
			container.RegisterType<IUserService, UserService>();
			container.RegisterType<IChatTL, ChatTL>();
			container.RegisterType<IChatService, ChatService>();
		    container.RegisterType<CommandReader, CommandReader>();
		    container.RegisterType<IMessagesTL, MessagesTL>();
            container.RegisterType<IMessagesService, MessagesService>();
            container.RegisterType<ICommand, ChatAddCommand>("chatadd");
			container.RegisterType<ICommand, ChatRemoveCommand>("chatremove");
            container.RegisterType<ICommand, ChatAddRemoveAllCommand>("chataddall", new InjectionConstructor(typeof(IChatService), typeof(IUserService), typeof(ChatRepository),  true));
            container.RegisterType<ICommand, ChatAddRemoveAllCommand>("chatremoveall", new InjectionConstructor(typeof(IChatService), typeof(IUserService), typeof(ChatRepository), false));
            container.RegisterType<ICommand, ChatCreateCommand>("chatcreate");
            container.RegisterType<ICommand, ChatListCommand>("chatlist");
            container.RegisterType<ICommand, ChatEditCommand>("chatedit");
            container.RegisterType<ICommand, ChatRegisterCommand>("chatregister");
            container.RegisterType<ICommand, AdminAddRemoveCommand>("adminadd", new InjectionConstructor(typeof(IUserService), true));
            container.RegisterType<ICommand, AdminAddRemoveCommand>("adminremove", new InjectionConstructor(typeof(IUserService), false));
            container.RegisterType<ICommand, AdminListCommand>("adminlist");
            container.RegisterType<ICommand, CommandsCommand>("help");
            container.RegisterType<ICommand, CommandsCommand>("h");
            container.RegisterType<ICommand, CommandsCommand>("?");
        }
    }
}
