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
            this.TokenGetter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TokenGetter.MinimumSize = new System.Drawing.Size(27, 25);
            this.TokenGetter.Name = "TokenGetter";
            this.TokenGetter.Size = new System.Drawing.Size(668, 424);
            this.TokenGetter.TabIndex = 0;
            // 
            // AuthorizationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 424);
            this.Controls.Add(this.TokenGetter);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "AuthorizationForm";
            this.Text = "AuthorizationForm";
            this.Load += new System.EventHandler(this.AuthorizationForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser TokenGetter;
    }
}