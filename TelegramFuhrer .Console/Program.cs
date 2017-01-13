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
		static void Main(string[] args)
		{
			using (var container = new UnityContainer())
			{
				BL.Bootstrap.RegisterTypes(container);

				System.Console.WriteLine("Done");
				System.Console.ReadLine();
			}
		}
	}
}
