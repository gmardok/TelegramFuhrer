using log4net;
using Microsoft.Practices.Unity;
using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using TelegramFuhrer.BL;
using TelegramFuhrer.Data;

namespace TelegramFuhrer.Service
{
    public partial class TelegramFuhrerService : ServiceBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TelegramFuhrerService));

        public TelegramFuhrerService()
        {
            InitializeComponent();
        }

        public static void StartWork()
        {
            new TelegramFuhrerService().OnStart(null);
        }

        protected override async void OnStart(string[] args)
        {
            try
            {
                new Thread(WorkerThread).Start();
            }
            catch (Exception ex)
            {
                Log.Error("Error while initializing service", ex);
                throw;
            }
        }

        protected override void OnStop()
        {
        }

        private static async void WorkerThread()
        {
            try
            {
                using (var container = new UnityContainer())
                {
                    //FuhrerContext.Init();
                    await BL.Bootstrap.RegisterTypesAsync(container);
                    Data.Bootstrap.RegisterTypes(container);
                    var commandReader = new CommandReader(container);
                    await commandReader.Execute();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in CommandReader", ex);
                WorkerThread();
            }
        }
    }
}
