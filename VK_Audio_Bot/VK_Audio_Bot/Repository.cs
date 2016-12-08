using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VK_Audio_Bot.Model;
using VK_Audio_Bot.DTO.Request;
using VK_Audio_Bot.DTO.Response;
using Newtonsoft.Json;

namespace VK_Audio_Bot
{
    class Repository
    {
        const string VK_Access_Token = "c3ca8f7857dc7a324ecfea0b815a42954926dbfcea20976c83242f4cba59d4149b9bf1fddd4e74ef1e0ac";
        const string TemplateUrl = "https://api.vk.com/method/audio.search?q={0}&auto_complete={1}&lyrics={2}&performer_only={3}&sort={4}&search_own={5}&offset={6}&count={7}&v=5.60&access_token={8}";

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
                var responseMsg = await httpClient.PostAsync(string.Format(TemplateUrl, vkRequest.Q, vkRequest.Auto_complete, 
                    vkRequest.Lyrics,vkRequest.Performer_only, vkRequest.Sort, vkRequest.Search_own, vkRequest.Offset, 
                    vkRequest.Count, VK_Access_Token), content);

                var resultString = await responseMsg.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<VKResponse>(resultString);

                var items = result.TrackInfo.Items.Select(ai => new AudioInfo
                {
                    Artist = ai.Artist,
                    Title = ai.Title,
                    Duration = ai.Duration
                }).ToList();
                return items;
            }
        }
    }
}
