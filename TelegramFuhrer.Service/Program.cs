using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using TelegramFuhrer.Data;

namespace TelegramFuhrer.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            XmlConfigurator.Configure();
            ServiceBase[] ServicesToRun;
            FuhrerContext.Init();
            ServicesToRun = new ServiceBase[]
            {
                new TelegramFuhrerService()
            };
            ServiceBase.Run(ServicesToRun);
        }
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            var log = LogManager.GetLogger("ThreadError");
            log.Error(e.Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var log = LogManager.GetLogger(typeof(Program));
            log.Error(e.ExceptionObject);
        }
    }
}
