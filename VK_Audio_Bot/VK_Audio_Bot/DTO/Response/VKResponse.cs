using Newtonsoft.Json;

namespace VK_Audio_Bot.DTO.Response
{
    class VKResponse
    {
        [JsonProperty("response")]
        public TrackInfo TrackInfo { get; set; }
    }
}
