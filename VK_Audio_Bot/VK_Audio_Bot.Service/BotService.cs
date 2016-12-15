using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;


namespace VK_Audio_Bot.Service
{
    public partial class BotService : ServiceBase
    {
        BotManager.BotManager botm;
        public BotService()
        {
            InitializeComponent();
            botm = new BotManager.BotManager();
            botm.logevent += AppendLog;
        }

        public async void TestStartupAndStop(string[] args)
        {
            OnStart(args);
            while(true) { }
            OnStop();
        }

        protected override async void OnStart(string[] args)
        {
            await botm.StartBot();
            
        }
        
        private async void UpdateUsers()
        {
            //lw.ReloadUsers(await botm.Usernames());
        }

        protected override void OnPause()
        {
            base.OnPause();
            botm.StopBot();
        }

        protected override async void OnContinue()
        {
            base.OnContinue();
            await botm.StartBot();
        }

        protected override void OnStop()
        {
            botm.OnClosing();
        }

        protected override void OnShutdown()
        {
            botm.OnClosing();
        }

        protected void AppendLog(string log)
        {
            
            try
            {
                if (!System.Diagnostics.EventLog.SourceExists("BotLog"))
                {
                    System.Diagnostics.EventLog.CreateEventSource("BotLog", "BotLog");
                }
                botLog.Source = "BotLog";
                botLog.WriteEntry($"{DateTime.Now.ToString()}: {log}\n");
            }
            catch { }
        }
    }
}
