using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Gemfire
{
    public class UserDto
    {
        [JsonProperty( "id" )]
        public string Id { get; set; }

        [JsonProperty( "name" )]
        public string Name { get; set; }
    }
}