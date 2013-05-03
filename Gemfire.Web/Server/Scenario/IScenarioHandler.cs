using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gemfire
{
    public interface IScenarioHandler
    {
        void AddScenario( string name, GameState initialState );
        GameState CopyGameState( string scenario );
        IEnumerable<string> GetScenarioNames();
        bool ValidScenario( string scenario );
    }
}