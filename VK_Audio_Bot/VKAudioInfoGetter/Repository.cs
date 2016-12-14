using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VKAudioInfoGetter.Model;
using VKAudioInfoGetter.DTO.Request;
using VKAudioInfoGetter.DTO.Response;
using Newtonsoft.Json;
using System;
using System.Net;

namespace VKAudioInfoGetter
{
    public class Repository
    {
        const string VK_Access_Token = "2717c824b5d24f385833eaebe7e07304d3efc79fce18559015174f5042d05fbde6744343f65a692bbacc9";
        const string AuthorisationTemplateUrl = "https://login.vk.com/?act=login&ip_h={0}&lg_h={1}&role=al_frame&email=89629656128&pass=Vk_audio_bot&expire=&captcha_sid=&captcha_key=&_origin=http://vk.com&q=1";
        const string AccessTokenUrl = "https://oauth.vk.com/authorize?client_id=5763628&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=audio&response_type=token&v=5.60";
        const string TemplateUrl = "https://api.vk.com/method/audio.search?q={0}&auto_complete={1}&lyrics={2}&performer_only={3}&sort={4}&search_own={5}&offset={6}&count={7}&v=5.60&access_token={8}";
        const string TemplateIdUrl = "https://api.vk.com/method/audio.getById?audios={0}&v=5.60&access_token={1}";

        public async Task<string> GetAccessToken()
        {
            using (var webClient = new WebClient())
            {
                using (var httpClient = new HttpClient())
                {
                    string page = webClient.DownloadString("https://vk.com/login");
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
                    var responseMsg = await httpClient.PostAsync(string.Format(AuthorisationTemplateUrl, ip_h, lg_h), content);
                    var authMsg = await httpClient.PostAsync("https://oauth.vk.com/authorize?client_id=5763628&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=audio&response_type=token&v=5.60", content);

                    var result = await authMsg.Content.ReadAsStringAsync();
                }
            }
            return "";
        }

        public async Task<List<AudioInfo>> GetAudioList(AudioRequest request)
        {
            using (var httpClient = new HttpClient())
            {
                var vkRequest = new VKRequest
                {
                    Q = request.Q,
                    Auto_complete = request.Auto_complete,
                    Lyrics = request.Lyrics,
                    Performer_only = request.Performer_only,
                    Sort = request.Sort,
                    Search_own = request.Search_own,
                    Offset = request.Offset,
                    Count = request.Count
                };

                StringContent content = new StringContent("");
                var responseMsg = await httpClient.PostAsync(string.Format(TemplateUrl, vkRequest.Q, Convert.ToByte(vkRequest.Auto_complete),
                                                             Convert.ToByte(vkRequest.Lyrics), Convert.ToByte(vkRequest.Performer_only), vkRequest.Sort, 
                                                             Convert.ToByte(vkRequest.Search_own), vkRequest.Offset, vkRequest.Count, VK_Access_Token), content);

                var resultString = await responseMsg.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<VKResponse>(resultString);

                var items = result.TrackInfo.Items.Select(ai => new AudioInfo
                {
                    Id = ai.Id,
                    Owner_id = ai.Owner_id,
                    Artist = ai.Artist,
                    Title = ai.Title,
                    Duration = ai.Duration,
                    Url = ai.Url,
                    Lyrics_id = ai.Lyrics_id
                }).ToList();
                return items;
            }
        }

        public async Task<AudioInfo> GetAudioById(AudioIdRequest request)
        {
            using (var httpClient = new HttpClient())
            {
                var vkIdRequest = new VKIdRequest
                {
                    Audios = request.Audios
                };

                StringContent content = new StringContent("");
                var responseMsg = await httpClient.PostAsync(string.Format(TemplateIdUrl, vkIdRequest.Audios, VK_Access_Token), content);

                var resultString = await responseMsg.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<VKResponse>(resultString);

                var item = result.TrackInfo.Items.Select(ai => new AudioInfo
                {
                    Id = ai.Id,
                    Owner_id = ai.Owner_id,
                    Artist = ai.Artist,
                    Title = ai.Title,
                    Duration = ai.Duration,
                    Url = ai.Url,
                    Lyrics_id = ai.Lyrics_id
                });
                return item.First();
            }
        }
    }
}
