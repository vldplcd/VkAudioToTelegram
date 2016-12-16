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
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                var htmlResult = wc.DownloadString($"http://mp3.cc/search/f/{requestText}/");
                Regex trackUrlEx = new Regex(".*data-id=\"(.*)\" data-mp3=\"(.*.mp3)\" data-url_song=\".*\" data-duration=\"(.*)\".*");
                Regex trackArtistEx = new Regex(".*<b><a href=\".*\">(.*)</a></b>");
                Regex trackTitleEx = new Regex(".*<em><a href=\".*\">(.*)</a></em>.*");
                var trackUrl = trackUrlEx.Match(htmlResult);
                var trackArtist = trackArtistEx.Match(htmlResult);
                var trackTitle = trackTitleEx.Match(htmlResult);
                while(trackTitle.Success && trackArtist.Success && trackUrl.Success)
                {
                    result.Add(new AudioInfo
                    {
                        Artist = trackArtist.Groups[1].Value.Replace("&quot;", "\""),
                        Title = trackTitle.Groups[1].Value.Replace("&quot;","\""),
                        Url = trackUrl.Groups[2].Value,
                        Id = int.Parse(trackUrl.Groups[1].Value),
                        Duration = int.Parse(trackUrl.Groups[3].Value)/1000
                    });

                    trackUrl = trackUrl.NextMatch();
                    trackTitle = trackTitle.NextMatch();
                    trackArtist = trackArtist.NextMatch();
                }
                return result;

            }
        }
    }
}
