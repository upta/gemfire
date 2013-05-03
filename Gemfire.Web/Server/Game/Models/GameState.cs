using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gemfire
{
    public class GameState
    {
        [BsonId]
        [BsonRepresentation( BsonType.ObjectId )]
        public string Id { get; set; }

        [BsonRepresentation( BsonType.ObjectId )]
        public string GameId { get; set; }

        public IEnumerable<Family> Families { get; set; }

        public IEnumerable<Province> Provinces { get; set; }

        public int Turn { get; set; }
    }
}