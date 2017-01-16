using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using TelegramFuhrer.BL.Services;
using TelegramFuhrer.BL.TL;
using TLSharp.Core;

namespace TelegramFuhrer.BL
{
	public static class Bootstrap
	{
		private static string ApiHash => ConfigurationManager.AppSettings["ApiHash"];

		private static int ApiId => int.Parse(ConfigurationManager.AppSettings["ApiId"]);

		public static async Task RegisterTypesAsync(IUnityContainer container)
		{
			TelegramClient client;
			try
			{
				client = new TelegramClient(ApiId, ApiHash);
				await client.ConnectAsync();
			}
			catch (MissingApiConfigurationException ex)
			{
				throw new Exception($"Please add your API settings to the `app.config` file. (More info: {MissingApiConfigurationException.InfoUrl})",
									ex);
			}

			container.RegisterInstance(typeof (TelegramClient), client);
			container.RegisterType<IUserTL, UserTL>();
			container.RegisterType<IUserService, UserService>();
			container.RegisterType<IChatTL, ChatTL>();
			container.RegisterType<IChatService, ChatService>();
		}
	}
}
