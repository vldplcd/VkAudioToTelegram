namespace VKAudioInfoGetter
{
    partial class AuthorizationForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TokenGetter = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // TokenGetter
            // 
            this.TokenGetter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TokenGetter.Location = new System.Drawing.Point(0, 0);
            this.TokenGetter.MinimumSize = new System.Drawing.Size(20, 20);
            this.TokenGetter.Name = "TokenGetter";
            this.TokenGetter.Size = new System.Drawing.Size(284, 261);
            this.TokenGetter.TabIndex = 0;
            // 
            // AuthorizationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.TokenGetter);
            this.Name = "AuthorizationForm";
            this.Text = "AuthorizationForm";
            this.Load += new System.EventHandler(this.AuthorizationForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser TokenGetter;
    }
}