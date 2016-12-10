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
        const string VK_Access_Token = "72ff749103be066406915619f0e28c5b2f66526139229eceab7bf23d75b1e94a4f7bfcd1c022edb014e60";
        const string TemplateUrl = "https://api.vk.com/method/audio.search?q={0}&auto_complete={1}&lyrics={2}&performer_only={3}&sort={4}&search_own={5}&offset={6}&count={7}&v=5.60&access_token={8}";
        const string TemplateIdUrl = "https://api.vk.com/method//audio.getById?audios={0}&v=5.60&access_token={1}";

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
