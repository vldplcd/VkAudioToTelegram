using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace VKAudioDB
{
    class Track
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        [JsonProperty("id")]
        public int dbID { get; set; }

        [JsonProperty("artist")]
        public string artist { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("duration")]
        public int duration { get; set; }

        [JsonProperty("url")]
        public string url { get; set; }

        [JsonProperty("lyrics_id")]
        public int lyrics_id { get; set; }
    }
}
