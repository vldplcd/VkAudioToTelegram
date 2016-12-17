﻿using System;
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

                var music = doc.DocumentNode.SelectSingleNode("//ul[@class='playlist']");

                foreach (var li in music.SelectNodes("//li[@class='track']"))
                {
                    result.Add(new AudioInfo
                    {
                        Artist = li.SelectSingleNode("h2").SelectSingleNode("b").SelectSingleNode("a").InnerText.ToString().Replace("&quot;", "\""),
                        Title = li.SelectSingleNode("h2").SelectSingleNode("em").SelectSingleNode("a").InnerText.ToString().Replace("&quot;", "\""),
                        Id = int.Parse(li.Attributes["data-id"].Value),
                        Url = li.Attributes["data-url_song"].Value,
                        Duration = int.Parse(li.Attributes["data-duration"].Value) / 1000,
                    });
                }
                return result;
            }
        }
    }
}

