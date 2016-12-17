using System;
using System.ServiceProcess;

namespace VK_Audio_Bot.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                BotService service1 = new BotService();
                service1.TestStartupAndStop(args);
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new BotService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
