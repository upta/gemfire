using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Gemfire
{
    public class InitDto
    {
        [JsonProperty( "scenarios" )]
        public IEnumerable<string> Scenarios { get; set; }

        [JsonProperty( "userId" )]
        public string UserId { get; set; }
    }
}