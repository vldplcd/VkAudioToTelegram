using Newtonsoft.Json;

namespace VK_Audio_Bot.DTO.Response
{
    class TrackInfo
    {
       [JsonProperty("items")]
        public Item[] Items { get; set; }
    }
}
