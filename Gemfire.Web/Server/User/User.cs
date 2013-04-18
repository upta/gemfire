using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Gemfire
{
    public class User
    {
        [BsonIgnore]
        public string ConnectionId { get; set; }

        public RegisteredClient RegistrationTicket { get; set; }

        [BsonId]
        [BsonRepresentation( BsonType.ObjectId )]
        public string Id { get; set; }


        public User( string connectionId, RegisteredClient rc )
        {
            this.ConnectionId = connectionId;
            this.RegistrationTicket = rc;
        }
    }
}