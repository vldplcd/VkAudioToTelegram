using System;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;

namespace VKAudioInfoGetter
{
    public partial class AuthorizationForm : Form
    {
        public AuthorizationForm()
        {
            InitializeComponent();
        }

        private void AuthorizationForm_Load(object sender, EventArgs e)
        {
            TokenGetter.DocumentCompleted += TokenGetter_DocumentCompleted;

            using (var webClient = new WebClient())
            {
                using (var httpClient = new HttpClient())
                {
                    string page = webClient.DownloadString("https://vk.com/login");
                    const string AuthorisationTemplateUrl = "https://login.vk.com/?act=login&ip_h={0}&lg_h={1}&role=al_frame&email=89629656128&pass=Vk_audio_bot&expire=&captcha_sid=&captcha_key=&_origin=http://vk.com&q=1";
                    string ip_h;
                    string lg_h;

                    var st_ind = page.IndexOf("<input type=\"hidden\" name=\"ip_h\" value=\"") + "<input type=\"hidden\" name=\"ip_h\" value=\"".Length;
                    var f_ind = page.IndexOf("\" />", st_ind);
                    var length = f_ind - st_ind;

                    ip_h = page.Substring(st_ind, length);
                    st_ind = page.IndexOf("<input type=\"hidden\" name=\"lg_h\" value=\"") + "<input type=\"hidden\" name=\"lg_h\" value=\"".Length;

                    f_ind = page.IndexOf("\" />", st_ind);
                    length = f_ind - st_ind;
                    lg_h = page.Substring(st_ind, length);

                    StringContent content = new StringContent("");
                    var responseMsg = httpClient.PostAsync(string.Format(AuthorisationTemplateUrl, ip_h, lg_h), content);

                    TokenGetter.Navigate("https://oauth.vk.com/authorize?client_id=5763628&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=audio&response_type=token&v=5.60");
                }
            }
        }

        private void TokenGetter_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (TokenGetter.Url.ToString().IndexOf("access_token=") != 0)
            {
                GetUserToken();
                //Push the token to database
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
