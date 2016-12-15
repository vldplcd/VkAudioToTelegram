using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VKAudioDB
{
    class ak
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }
        public string akID { get; set; }
        public string value { get; set; }
    }
}
