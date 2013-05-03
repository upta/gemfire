using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gemfire
{
    public interface IGameHandler
    {
        void AddGame( Game game );
        void AddPlayer( Game game, string userId );
        Game CreateGameWithScenario( User creator, string scenario, string name );
        Game GetGameById( string id );
        IEnumerable<Game> GetGames();
        void RemoveGame( string id );
        void RemovePlayer( Game game, string userId );
    }
}