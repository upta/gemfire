using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Gemfire
{
    [HubName("lobby")]
    public class Lobby : Hub
    {
        private readonly IGameHandler gameHandler;
        private readonly IRegistrationHandler registrationHandler;
        private readonly IUserHandler userHandler;
        private readonly IMappingHandler mappingHandler;
        private readonly IScenarioHandler scenarioHandler;

        private object locker = new object();

        public Lobby( IGameHandler gameHandler,
                      IRegistrationHandler registrationHandler,
                      IUserHandler userHandler,
                      IMappingHandler mappingHandler,
                      IScenarioHandler scenarioHandler )
        {
            this.gameHandler = gameHandler;
            this.registrationHandler = registrationHandler;
            this.userHandler = userHandler;
            this.mappingHandler = mappingHandler;
            this.scenarioHandler = scenarioHandler;
        }


        public void CreateGame( string name, string scenario )
        {
            var user = this.GetCurrentUser();

            if ( !this.scenarioHandler.ValidScenario( scenario ) )
            {
                throw new InvalidOperationException( string.Format( "Scenario '{0}' wasn't found", scenario ) );
            }

            var game = this.gameHandler.CreateGameWithScenario( user, scenario, name );

            if ( game == null )
            {
                throw new InvalidOperationException( string.Format( "Failed to create game from scenario '{0}', named '{1}'", scenario, name ) );
            }

            this.gameHandler.AddGame( game );
            this.Groups.Add( this.Context.ConnectionId, game.GroupName );

            var dto = this.mappingHandler.Map<GameDto>( game );
            this.Clients.All.gameCreated( dto );
        }

        public InitDto InitializeClient( string registrationId )
        {
            if ( this.registrationHandler.RegistrationExists( registrationId ) )
            {
                var rc = this.registrationHandler.RemoveRegistration( registrationId );

                if ( this.userHandler.UserExists( this.Context.ConnectionId ) )
                {
                    return null;
                }

                var user = this.userHandler.FindUserByIdentity( rc.Identity );

                if ( user == null )
                {
                    user = new User( this.Context.ConnectionId, rc );
                }
                else
                {
                    user.RegistrationTicket = rc;
                    this.userHandler.ReassignUser( this.Context.ConnectionId, user );
                }

                this.userHandler.AddUser( user ); // will ensure that they're persisted

                // add the user into the groups for any games to which he belongs
                var usersGames = this.gameHandler.GetGames().Where( a => a.Players.Any( b => b == user.Id ) );

                foreach ( var game in usersGames )
                {
                    this.Groups.Add( user.ConnectionId, game.GroupName );
                }

                return new InitDto
                {
                    Scenarios = this.scenarioHandler.GetScenarioNames(),
                    UserId = user.Id
                };
            }

            return null;
        }

        public IEnumerable<GameDto> GetGames()
        {
            var result = this.gameHandler.GetGames().Select( a => this.mappingHandler.Map<GameDto>( a ) );
            
            return result;
        }

        public void JoinGame( string gameId )
        {
            var user = this.GetCurrentUser();

            var game = this.gameHandler.GetGameById( gameId );

            if ( game == null )
            {
                throw new InvalidOperationException( string.Format( "Couldn't find game {0}", gameId ) );
            }
            
            this.gameHandler.AddPlayer( game, user.Id );

            this.Groups.Add( this.Context.ConnectionId, game.GroupName ).Wait();

            this.Clients.Group( game.GroupName ).joinedGame( game.Id, this.mappingHandler.Map<UserDto>( user ) );
        }

        public void LeaveGame( string gameId )
        {
            var user = this.GetCurrentUser();

            var game = this.gameHandler.GetGameById( gameId );

            if ( game == null )
            {
                throw new InvalidOperationException( string.Format( "Couldn't find game {0}", gameId ) );
            }

            if ( game.Creator == user.Id ) // they own it, so we just delete it instead of leaving
            {
                this.gameHandler.RemoveGame( gameId );

                this.Clients.All.gameRemoved( gameId );
            }
            else
            {
                this.gameHandler.RemovePlayer( game, user.Id );

                this.Clients.Group( game.GroupName ).leftGame( game.Id, user.Id );

                this.Groups.Remove( this.Context.ConnectionId, game.GroupName );
            }
        }


        private User GetCurrentUser()
        {
            var user = this.userHandler.GetUser( this.Context.ConnectionId );

            if ( user == null )
            {
                throw new InvalidOperationException( string.Format( "User wasn't found for connection {0}", this.Context.ConnectionId ) );
            }

            return user;
        }
    }
}