using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gemfire
{
    public class Family
    {
        [BsonId]
        [BsonRepresentation( BsonType.ObjectId )]
        public string Id { get; set; }

        public IEnumerable<Character> Members { get; set; }

        public string Name { get; set; }
    }
}