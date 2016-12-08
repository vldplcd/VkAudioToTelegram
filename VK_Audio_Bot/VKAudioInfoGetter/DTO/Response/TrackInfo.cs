using Newtonsoft.Json;

namespace VKAudioInfoGetter.DTO.Response
{
    class TrackInfo
    {
       [JsonProperty("items")]
        public Item[] Items { get; set; }
    }
}
