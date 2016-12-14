namespace VK_Audio_Bot.Service
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AudioBotProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.AudioBotInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // AudioBotProcessInstaller
            // 
            this.AudioBotProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.AudioBotProcessInstaller.Password = null;
            this.AudioBotProcessInstaller.Username = null;
            this.AudioBotProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.AudioBotProcessInstaller_AfterInstall);
            // 
            // AudioBotInstaller
            // 
            this.AudioBotInstaller.ServiceName = "BotService";
            this.AudioBotInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.AudioBotProcessInstaller,
            this.AudioBotInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller AudioBotProcessInstaller;
        private System.ServiceProcess.ServiceInstaller AudioBotInstaller;
    }
}