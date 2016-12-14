using System;
using System.ServiceProcess;
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
            botm.userAddedEvent += UpdateUsers;
        }
        LogWindow lw = new LogWindow();
        protected override async void OnStart(string[] args)
        {
            await botm.StartBot();
            botm.logevent += lw.AppendLog;
            lw.onContinueEvent += OnContinue;
            lw.onPauseEvent += OnPause;
            lw.onSendEvent += botm.SendMessage;
            lw.Show();
        }

        private async void UpdateUsers()
        {
            lw.ReloadUsers(await botm.Usernames());
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnContinue()
        {
            base.OnContinue();
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
