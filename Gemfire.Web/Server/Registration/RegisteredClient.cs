using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Gemfire
{
    public class RegisteredClient
    {
        [JsonProperty( "displayName" )]
        public string DisplayName { get; set; }

        [JsonProperty( "identity" )]
        public string Identity { get; set; }

        [JsonProperty( "initializedAt" )]
        public DateTime InitializedAt
        {
            get;
            private set;
        }

        [JsonProperty( "photo" )]
        public string Photo { get; set; }

        [BsonIgnore]
        [JsonProperty( "registrationId" )]
        public string RegistrationId { get; set; }

        public RegisteredClient()
        {
        }

        public RegisteredClient( string registrationId, string identity, string displayName, string photo )
        {
            this.RegistrationId = registrationId;
            this.Identity = identity;
            this.DisplayName = displayName;
            this.Photo = photo;
            this.InitializedAt = DateTime.UtcNow;
        }
    }
}