using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VKAudioInfoGetter.Model;
using VKAudioInfoGetter.DTO.Request;
using VKAudioInfoGetter.DTO.Response;
using Newtonsoft.Json;
using System;

namespace VKAudioInfoGetter
{
    public class Repository
    {
        const string VK_Access_Token = "798d601fa08a554ba2cacbb92b0828d30f5da996c629ad3f2291e979039e150fbd6864c4e8a2a99edc7c7";
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
                var responseMsg = await httpClient.PostAsync(string.Format(TemplateUrl, vkRequest.Q, Convert.ToByte(vkRequest.Auto_complete),
                    Convert.ToByte(vkRequest.Lyrics), Convert.ToByte(vkRequest.Performer_only), vkRequest.Sort, Convert.ToByte(vkRequest.Search_own), vkRequest.Offset, 
                    vkRequest.Count, VK_Access_Token), content);

                var resultString = await responseMsg.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<VKResponse>(resultString);

                var items = result.TrackInfo.Items.Select(ai => new AudioInfo
                {
                    Id = ai.Id,
                    Artist = ai.Artist,
                    Title = ai.Title,
                    Duration = ai.Duration,
                    Url = ai.Url,
                    Lyrics_id = ai.Lyrics_id
                }).ToList();
                return items;
            }
        }
    }
}
