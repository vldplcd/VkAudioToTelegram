using Newtonsoft.Json;

namespace VK_Audio_Bot.DTO.Response
{
    class Item
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("artist")]
        public string Artist { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("lyrics_id")]
        public int Lyrics_id { get; set; }
    }
}
