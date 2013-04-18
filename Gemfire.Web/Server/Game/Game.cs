using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Gemfire
{
    public class Game
    {
        public DateTime CreatedAt { get; private set; }

        public string Creator { get; set; }

        [BsonId]
        [BsonRepresentation( BsonType.ObjectId )]
        public string Id { get; set; }

        [BsonIgnore]
        public string GroupName
        {
            get
            {
                return "Game" + this.Id;
            }
        }

        public string Name { get; set; }

        public string Scenario { get; set; }

        public List<string> Players { get; set; }

        public Game( string name, string creator )
        {
            this.CreatedAt = DateTime.Now;
            this.Creator = creator;
            this.Name = name;
            this.Players = new List<string>();
        }
    }
}