using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VK_Audio_Bot.SpeechRecognition
{
    public class SpeechRepository
    {
        string requestUrl = "https://asr.yandex.net/asr_xml?uuid={0}&key={1}&topic=queries";

        private byte[] StreamToBytes(Stream stream)
        {
            byte[] b;

            using (BinaryReader br = new BinaryReader(stream))
            {
                b = br.ReadBytes((int)stream.Length);
            }
            return b;
        }

        private string Get16Random()
        {
            string alfa = "0123456789abcdef";
            string result = "";
            Random rnd = new Random();

            for (int i = 0; i < 32; i++)
            {
                if (i == 0)
                    result += alfa[rnd.Next(1, 16)];
                else
                    result += alfa[rnd.Next(0, 16)];
            }

            return result;
        }

        public async Task<string> Result(Stream stream, string api_key)
        {
            string result;

            using (Stream fs = stream)
            {
                using (var client = new HttpClient())
                {
                    var content = new ByteArrayContent(StreamToBytes(fs));
                    var request = new HttpRequestMessage(HttpMethod.Post, string.Format(requestUrl, Get16Random(), api_key));
                    request.Content = content;
                    request.Content.Headers.Add("Content-Type", "audio/ogg; codecs=opus");

                    var responseMsg = await client.SendAsync(request);
                    var responseStr = responseMsg.Content.ReadAsStringAsync();

                    if (responseStr.Exception == null)
                    {
                        Regex dexml = new Regex(".*confidence=\"([0-9.]{1,4})\">(.*)</variant>");
                        var xml = responseStr.Result;
                        Match match = dexml.Match(xml);

                        if (match.Success)
                        {
                            double confidence = double.Parse(match.Groups[1].Value.Replace('.', ','));
                            result = match.Groups[2].Value;

                            if (confidence < 0.5)
                                throw new Exception("Sorry, I didn't undersand you");
                        }
                        else
                            throw new Exception("Sorry, I didn't undersand you");
                    }
                    else
                        throw new Exception("Sorry, I didn't undersand you");
                }
                return result;
            }
        }
    }
}
