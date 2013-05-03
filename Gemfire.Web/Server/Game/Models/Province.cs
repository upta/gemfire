using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gemfire
{
    public class Province
    {
        public string Character { get; set; }

        [BsonId]
        [BsonRepresentation( BsonType.ObjectId )]
        public string Id { get; set; }

        public string Family { get; set; }

        public string Name { get; set; }

        public byte Number { get; set; }
    }
}