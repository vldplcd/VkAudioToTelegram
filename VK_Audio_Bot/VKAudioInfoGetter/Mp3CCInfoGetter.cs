using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VKAudioInfoGetter.Model;

namespace VKAudioInfoGetter
{
    public class Mp3CCInfoGetter : IInfoGetter
    {
        public event GetApiKey getApiKeyEvent;

        public async Task<List<AudioInfo>> GetMusic(string requestText)
        {
            requestText = requestText.Trim().Replace(' ', '+');
            var result = new List<AudioInfo>();

            using (WebClient webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                var htmlResult = webClient.DownloadString($"http://mp3.cc/search/f/{requestText}/");
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(htmlResult);

                foreach (var li in doc.DocumentNode.SelectNodes("//li"))
                    foreach (var b in doc.DocumentNode.SelectNodes("//li/b/a"))
                        foreach (var em in doc.DocumentNode.SelectNodes("//li/em/a"))
                        {
                            result.Add(new AudioInfo
                            {
                                Artist = b.InnerText,
                                Title = em.InnerText,
                                Id = int.Parse(li.Attributes["data-id"].Value),
                                Url = li.Attributes["data-url_song"].Value,
                                Duration = int.Parse(li.Attributes["duration"].Value) / 1000,
                            });
                        }
                return result;
            }
        }
    }
}

