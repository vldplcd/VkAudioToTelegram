﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKAudioDB
{
    class ak
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        public string value { get; set; }
    }
}
