﻿using Newtonsoft.Json;

namespace VKAudioInfoGetter.DTO.Response
{
    class Item
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("owner_id")]
        public int Owner_id { get; set; }

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
