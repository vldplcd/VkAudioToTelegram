using System.ComponentModel;
using System.Configuration.Install;

namespace VK_Audio_Bot.Service
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void AudioBotProcessInstaller_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
