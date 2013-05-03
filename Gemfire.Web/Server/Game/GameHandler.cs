using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gemfire
{
    public class GameHandler : IGameHandler
    {
        private readonly IRepository repository;
        private ConcurrentDictionary<string, Game> games = new ConcurrentDictionary<string, Game>();

        public GameHandler( IRepository repository )
        {
            this.repository = repository;
        }


        public void AddGame( Game game )
        {
            this.repository.Save<Game>( game );

            this.games.TryAdd( game.Id, game );
        }

        public void AddPlayer( Game game, string userId )
        {
            if ( !game.Players.Contains( userId ) )
            {
                game.Players.Add( userId );
                this.repository.Save<Game>( game );
            }
        }

        public Game CreateGameWithScenario( User creator, string scenario, string name )
        {
            var game = new Game( name, creator.Id );
            game.Scenario = scenario;

            this.AddPlayer( game, creator.Id );

            return game;
        }

        public Game GetGameById( string id )
        {
            if ( this.games.ContainsKey( id ) )
            {
                return this.games[ id ];
            }

            return this.repository.Find<Game>().FirstOrDefault( a => a.Id == id );
        }

        public IEnumerable<Game> GetGames()
        {
            if ( !this.games.Any() )
            {
                foreach ( var game in this.repository.Find<Game>() )
                {
                    this.games.TryAdd( game.Id, game );
                }                
            }

            return this.games.Values;
        }

        public void RemoveGame( string id )
        {
            Game garbage;
            this.games.TryRemove( id, out garbage );

            this.repository.Delete<Game>( id );
        }

        public void RemovePlayer( Game game, string userId )
        {
            if ( game.Players.Contains( userId ) )
            {
                game.Players.Remove( userId );
            }

            this.repository.Save<Game>( game );
        }
    }
}