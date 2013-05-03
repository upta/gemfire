using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gemfire
{
    public class Character
    {
        public byte Charm { get; set; }

        [BsonId]
        [BsonRepresentation( BsonType.ObjectId )]
        public string Id { get; set; }

        public byte Intelligence { get; set; }

        public string Name { get; set; }       

        public byte War { get; set; }
    }
}