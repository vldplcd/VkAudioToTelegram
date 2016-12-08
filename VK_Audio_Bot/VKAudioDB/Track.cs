
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VKAudioDB
{
    class Track
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        public int dbID { get; set; }

        public string artist { get; set; }

        public string title { get; set; }

        public int duration { get; set; }

        public string url { get; set; }

        public int lyrics_id { get; set; }
    }
}
