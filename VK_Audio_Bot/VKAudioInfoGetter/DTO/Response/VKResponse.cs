using Newtonsoft.Json;

namespace VKAudioInfoGetter.DTO.Response
{
    class VKResponse
    {
        [JsonProperty("response")]
        public TrackInfo TrackInfo { get; set; }
    }
}
