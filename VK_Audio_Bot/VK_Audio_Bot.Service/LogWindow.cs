using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VK_Audio_Bot.Service
{
    public delegate void OnButtonClickDel();
    public delegate Task OnSendButtonClickDel(long chID, string message);

    public partial class LogWindow : Form
    {
        public event OnButtonClickDel onPauseEvent;
        public event OnButtonClickDel onContinueEvent;
        public event OnSendButtonClickDel onSendEvent;
         
        public LogWindow()
        {
            InitializeComponent();
        }

        public void ReloadUsers(Dictionary<long, string> usernames)
        {
            var bs = new BindingSource(usernames, null);

            cbUsers.DataSource = bs;
            cbUsers.DisplayMember = "Value";
            cbUsers.ValueMember = "Key";
        }
        public void AppendLog(string log)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke(() => rtbLogs.AppendText(log));
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            onPauseEvent?.Invoke();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            onContinueEvent?.Invoke();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            onSendEvent?.Invoke((long)cbUsers.SelectedValue, rtbMessage.Text);
        }
    }
}
