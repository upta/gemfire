using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Gemfire
{
    public class ScenarioHandler : IScenarioHandler
    {
        private ConcurrentDictionary<string, GameState> scenarios = new ConcurrentDictionary<string, GameState>();


        public ScenarioHandler()
        {

        }

        public ScenarioHandler( string jsonPath )
        {
            var data = File.ReadAllText( jsonPath );

            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, GameState>>( data );

            foreach ( var scenario in dictionary )
            {
                this.AddScenario( scenario.Key, scenario.Value );
            }
        }


        public void AddScenario( string name, GameState initialState )
        {
            this.scenarios.TryAdd( name, initialState );
        }

        public GameState CopyGameState( string scenario )
        {
            return JsonConvert.DeserializeObject<GameState>( JsonConvert.SerializeObject( this.scenarios[ scenario ] ) );
        }

        public IEnumerable<string> GetScenarioNames()
        {
            return this.scenarios.Keys;
        }

        public bool ValidScenario( string scenario )
        {
            return this.scenarios.ContainsKey( scenario );
        }
    }
}