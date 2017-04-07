using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using TelegramFuhrer.Data.Repositories;

namespace TelegramFuhrer.Data
{
	public class Bootstrap
	{
		public static void RegisterTypes(IUnityContainer container)
		{
			container.RegisterType<UserRepository, UserRepository>();
			container.RegisterType<ChatRepository, ChatRepository>();
            container.RegisterType<UserChatRepository, UserChatRepository>();
        }
    }
}
