using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Gemfire
{
    public class GameDto
    {
        [JsonProperty( "createdAt" )]
        public DateTime CreatedAt { get; set; }

        [JsonProperty( "creator" )]
        public string Creator { get; set; }

        [JsonProperty( "id" )]
        public string Id { get; set; }

        [JsonProperty( "name" )]
        public string Name { get; set; }

        [JsonProperty( "players" )]
        public List<UserDto> Players { get; set; }

        [JsonProperty( "scenario" )]
        public string Scenario { get; set; }
    }
}