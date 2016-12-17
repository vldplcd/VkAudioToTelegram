using System;
using System.Windows.Forms;

namespace VKAudioInfoGetter
{
    public partial class AuthorizationForm : Form
    {
        public AuthorizationForm()
        {
            InitializeComponent();
        }
        public string result;
        private void AuthorizationForm_Load(object sender, EventArgs e)
        {
            TokenGetter.DocumentCompleted += TokenGetter_DocumentCompleted;
            TokenGetter.Navigate("https://oauth.vk.com/authorize?client_id=5763628&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=audio&response_type=token&v=5.60");
        }

        private void TokenGetter_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (TokenGetter.Url.ToString().IndexOf("access_token=") > 0)
            {
                result = GetUserToken();
                Close();
            }
        }

        private string GetUserToken()
        {
            char[] Symbols = { '=', '&' };
            string[] URL = TokenGetter.Url.ToString().Split(Symbols);

            Visible = false;
            return URL[1].ToString();
        }
    }
}
