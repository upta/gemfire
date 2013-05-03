using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Dynamic;
using System.Threading.Tasks;

namespace Gemfire.Tests
{
    [TestClass]
    public class LobbyTests
    {
        [TestMethod]
        public void CreateGame_ThrowsIfUserDoesntExist()
        {
            var connectionId = "123";

            var hub = this.GetHub( connectionId );

            AssertIt.Throws<InvalidOperationException>( () =>
            {
                hub.CreateGame( "game-name", "scenario" );
            } );
        }

        [TestMethod]
        public void CreateGame_ThrowsIfScenarioDoesntExist()
        {
            var connectionId = "123";

            #region UserHandler
            var userHandler = new Mock<IUserHandler>();

            userHandler.Setup( a => a.GetUser( connectionId ) )
                       .Returns( new User( connectionId, new RegisteredClient() ) );
            #endregion

            var hub = this.GetHub( connectionId, userHandler: userHandler );

            AssertIt.Throws<InvalidOperationException>( () =>
            {
                hub.CreateGame( "game-name", "scenario" );
            } );
        }

        [TestMethod]
        public void CreateGame_ThrowsIfCreateGameFailed()
        {
            var connectionId = "123";

            #region UserHandler
            var userHandler = new Mock<IUserHandler>();

            userHandler.Setup( a => a.GetUser( connectionId ) )
                       .Returns( new User( connectionId, new RegisteredClient() ) );
            #endregion

            #region ScenarioHandler
            var scenarioHandler = new Mock<IScenarioHandler>();

            scenarioHandler.Setup( a => a.ValidScenario( It.IsAny<string>() ) )
                           .Returns( true );
            #endregion

            var hub = this.GetHub( connectionId,
                                   userHandler: userHandler,
                                   scenarioHandler: scenarioHandler );

            AssertIt.Throws<InvalidOperationException>( () =>
            {
                hub.CreateGame( "game-name", "scenario" );
            } );
        }

        [TestMethod]
        public void CreateGame_AddsGameAfterCreate()
        {
            var connectionId = "123";
            var gameName = "game-name";
            var scenario = "scenario";
            var user = new User( connectionId, new RegisteredClient() );
            var game = new Game( gameName, user.Id );

            #region UserHandler
            var userHandler = new Mock<IUserHandler>();

            userHandler.Setup( a => a.GetUser( connectionId ) )
                       .Returns( user );
            #endregion

            #region GameHandler
            var gameHandler = new Mock<IGameHandler>();

            gameHandler.Setup( a => a.CreateGameWithScenario( It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>() ) )
                       .Returns( game );
            #endregion

            #region ScenarioHandler
            var scenarioHandler = new Mock<IScenarioHandler>();

            scenarioHandler.Setup( a => a.ValidScenario( scenario ) )
                           .Returns( true );
            #endregion

            var hub = this.GetHub( connectionId,
                                   userHandler: userHandler,
                                   gameHandler: gameHandler,
                                   scenarioHandler: scenarioHandler );

            hub.CreateGame( gameName, scenario );

            gameHandler.Verify( a => a.AddGame( It.IsAny<Game>() ) );
        }

        [TestMethod]
        public void CreateGame_AddsUserToGameGroup()
        {
            var connectionId = "123";
            var gameName = "game-name";
            var scenario = "scenario";
            var user = new User( connectionId, new RegisteredClient() );
            var game = new Game( gameName, user.Id );
            var addedGroups = new List<string>();

            #region UserHandler
            var userHandler = new Mock<IUserHandler>();

            userHandler.Setup( a => a.GetUser( connectionId ) )
                       .Returns( user );
            #endregion

            #region GameHandler
            var gameHandler = new Mock<IGameHandler>();

            gameHandler.Setup( a => a.CreateGameWithScenario( It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>() ) )
                       .Returns( game );
            #endregion

            #region ScenarioHandler
            var scenarioHandler = new Mock<IScenarioHandler>();

            scenarioHandler.Setup( a => a.ValidScenario( scenario ) )
                           .Returns( true );
            #endregion

            #region GroupManager
            var groupManager = new Mock<IGroupManager>();

            groupManager.Setup( a => a.Add( It.IsAny<string>(), It.IsAny<string>() ) )
                        .Callback<string, string>( ( c, g ) => addedGroups.Add( g ) );
            #endregion

            var hub = this.GetHub( connectionId,
                                   userHandler: userHandler,
                                   gameHandler: gameHandler,
                                   groupManager: groupManager,
                                   scenarioHandler: scenarioHandler );

            hub.CreateGame( gameName, scenario );

            Assert.IsTrue( addedGroups.Any( a => a == game.GroupName ) );
        }

        [TestMethod]
        public void CreateGame_TellsAllThatGameWasCreated()
        {
            var connectionId = "123";
            var gameName = "game-name";
            var scenario = "scenario";
            var calledGameCreated = false;
            var user = new User( connectionId, new RegisteredClient() );
            var game = new Game( gameName, user.Id );

            #region UserHandler
            var userHandler = new Mock<IUserHandler>();

            userHandler.Setup( a => a.GetUser( connectionId ) )
                       .Returns( user );
            #endregion

            #region GameHandler
            var gameHandler = new Mock<IGameHandler>();

            gameHandler.Setup( a => a.CreateGameWithScenario( It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>() ) )
                       .Returns( game );
            #endregion

            #region ScenarioHandler
            var scenarioHandler = new Mock<IScenarioHandler>();

            scenarioHandler.Setup( a => a.ValidScenario( scenario ) )
                           .Returns( true );
            #endregion

            var hub = this.GetHub( connectionId,
                                   userHandler: userHandler,
                                   gameHandler: gameHandler,
                                   scenarioHandler: scenarioHandler );

            dynamic all = new ExpandoObject();
            all.gameCreated = new Action<GameDto>( ( dto ) =>
            {
                calledGameCreated = true;
            } );
            hub.Clients.All = all;

            hub.CreateGame( gameName, scenario );

            Assert.IsTrue( calledGameCreated );
        }


        [TestMethod]
        public void InitializeClient_ReturnsNullIfBadRegistration()
        {
            var hub = this.GetHub( "123" );
            
            var result = hub.InitializeClient( "not-real-reg" );

            Assert.IsNull( result );
        }

        [TestMethod]
        public void InitializeClient_RemovesRegistration()
        {
            #region RegistrationHandler
            var registrationHandler = new Mock<IRegistrationHandler>();

            registrationHandler.Setup( a => a.RegistrationExists( It.IsAny<string>() ) )
                        .Returns( true );
            #endregion

            #region UserHandler
            var userHandler = new Mock<IUserHandler>();

            userHandler.Setup( a => a.UserExists( It.IsAny<string>() ) )
                .Returns( true );
            #endregion

            var hub = this.GetHub( "conn",
                                   registrationHandler: registrationHandler,
                                   userHandler: userHandler );

            hub.InitializeClient( "reg" );

            registrationHandler.Verify( a => a.RemoveRegistration( It.IsAny<string>() ) );
        }

        [TestMethod]
        public void InitializeClient_ReturnsNullIfUserAlreadyExists()
        {
            #region RegistrationHandler
            var registration = new Mock<IRegistrationHandler>();

            registration.Setup( a => a.RegistrationExists( It.IsAny<string>() ) )
                        .Returns( true );
            #endregion

            #region UserHandler
            var user = new Mock<IUserHandler>();

            user.Setup( a => a.UserExists( It.IsAny<string>() ) )
                .Returns( true );
            #endregion

            var hub = this.GetHub( "conn", 
                                   registrationHandler: registration, 
                                   userHandler: user );

            var result = hub.InitializeClient( "reg" );

            Assert.IsNull( result );
        }

        [TestMethod]
        public void InitializeClient_ReassignsConnectionIfUserFound()
        {
            var connection = "123";
            var rc = new RegisteredClient
            {
                DisplayName = "name",
                Identity = "id",
                Photo = "photo",
                RegistrationId = "reg-id"
            };

            #region RegistrationHandler
            var registrationHandler = new Mock<IRegistrationHandler>();

            registrationHandler.Setup( a => a.RegistrationExists( It.IsAny<string>() ) )
                               .Returns( true );

            registrationHandler.Setup( a => a.RemoveRegistration( It.IsAny<string>() ) )
                               .Returns( rc );
            #endregion

            #region UserHandler
            var userHandler = new Mock<IUserHandler>();

            userHandler.Setup( a => a.UserExists( It.IsAny<string>() ) )
                       .Returns( false );

            userHandler.Setup( a => a.FindUserByIdentity( rc.Identity ) )
                       .Returns( new User( connection, rc ) );
            #endregion

            var hub = this.GetHub( connection, 
                                   registrationHandler: registrationHandler, 
                                   userHandler: userHandler );

            var result = hub.InitializeClient( "reg" );

            userHandler.Verify( a => a.ReassignUser( It.IsAny<string>(), It.IsAny<User>() ) );
        }

        [TestMethod]
        public void InitializeClient_SavesUser()
        {
            var connection = "123";
            var rc = new RegisteredClient
            {
                DisplayName = "name",
                Identity = "id",
                Photo = "photo",
                RegistrationId = "reg-id"
            };

            #region RegistrationHandler
            var registrationHandler = new Mock<IRegistrationHandler>();

            registrationHandler.Setup( a => a.RegistrationExists( It.IsAny<string>() ) )
                               .Returns( true );

            registrationHandler.Setup( a => a.RemoveRegistration( It.IsAny<string>() ) )
                               .Returns( rc );
            #endregion

            #region UserHandler
            var userHandler = new Mock<IUserHandler>();

            userHandler.Setup( a => a.UserExists( It.IsAny<string>() ) )
                       .Returns( false );
            #endregion

            var hub = this.GetHub( connection, 
                                   registrationHandler: registrationHandler,
                                   userHandler: userHandler );

            var result = hub.InitializeClient( "reg" );

            userHandler.Verify( a => a.AddUser( It.IsAny<User>() ) );
        }

        [TestMethod]
        public void InitializeClient_AddsUserToTheirGameGroups()
        {
            var connection = "123";
            var userid = "user-id";

            var rc = new RegisteredClient
            {
                DisplayName = "name",
                Identity = "id",
                Photo = "photo",
                RegistrationId = "reg-id"
            };

            var userGame = new Game( "name", "creator" )
            {
                Id = "game-id",
                Players = new List<string>() { userid }
            };

            var addedGroups = new List<string>();

            #region RegistrationHandler
            var registration = new Mock<IRegistrationHandler>();

            registration.Setup( a => a.RegistrationExists( It.IsAny<string>() ) )
                        .Returns( true );

            registration.Setup( a => a.RemoveRegistration( It.IsAny<string>() ) )
                        .Returns( rc );
            #endregion

            #region UserHandler
            var user = new Mock<IUserHandler>();

            user.Setup( a => a.UserExists( It.IsAny<string>() ) )
                .Returns( false );

            user.Setup( a => a.FindUserByIdentity( It.IsAny<string>() ) )
                .Returns( new User( connection, rc ) { Id = userid } );
            #endregion

            #region GameHandler
            var game = new Mock<IGameHandler>();

            game.Setup( a => a.GetGames() )
                .Returns( new List<Game>() { userGame } );
            #endregion

            #region GroupManager
            var groups = new Mock<IGroupManager>();

            groups.Setup( a => a.Add( It.IsAny<string>(), It.IsAny<string>() ) )
                  .Callback<string, string>( ( c, g ) => addedGroups.Add( g ) );
            #endregion

            var hub = this.GetHub( connection,
                                   groupManager: groups,
                                   registrationHandler: registration, 
                                   userHandler: user, 
                                   gameHandler: game );

            var result = hub.InitializeClient( "reg" );

            Assert.IsTrue( addedGroups.Any( a => a == userGame.GroupName ) );
        }

        [TestMethod]
        public void InitializeClient_ReturnsAvailableScenarios()
        {
            var connection = "123";
            var userid = "user-id";

            var rc = new RegisteredClient
            {
                DisplayName = "name",
                Identity = "id",
                Photo = "photo",
                RegistrationId = "reg-id"
            };

            var userGame = new Game( "name", "creator" )
            {
                Id = "game-id",
                Players = new List<string>() { userid }
            };

            var scenarioNames = new List<string>() { "s1", "s2" };

            #region RegistrationHandler
            var registration = new Mock<IRegistrationHandler>();

            registration.Setup( a => a.RegistrationExists( It.IsAny<string>() ) )
                        .Returns( true );

            registration.Setup( a => a.RemoveRegistration( It.IsAny<string>() ) )
                        .Returns( rc );
            #endregion

            #region UserHandler
            var user = new Mock<IUserHandler>();

            user.Setup( a => a.UserExists( It.IsAny<string>() ) )
                .Returns( false );

            user.Setup( a => a.FindUserByIdentity( It.IsAny<string>() ) )
                .Returns( new User( connection, rc ) { Id = userid } );
            #endregion

            #region GameHandler
            var game = new Mock<IGameHandler>();

            game.Setup( a => a.GetGames() )
                .Returns( new List<Game>() { userGame } );
            #endregion

            #region ScenarioHandler
            var scenarioHandler = new Mock<IScenarioHandler>();

            scenarioHandler.Setup( a => a.GetScenarioNames() )
                           .Returns( scenarioNames );
            #endregion

            var hub = this.GetHub( connection,
                                   registrationHandler: registration,
                                   userHandler: user,
                                   gameHandler: game,
                                   scenarioHandler: scenarioHandler );

            var result = hub.InitializeClient( "reg" );

            CollectionAssert.AreEquivalent( scenarioNames, result.Scenarios.ToList() );
        }


        [TestMethod]
        public void GetGames_ReturnsAllGames()
        {
            var connectionId = "123";

            #region GameHandler
            var gameHandler = new Mock<IGameHandler>();

            gameHandler.Setup( a => a.GetGames() )
                       .Returns( new List<Game>() { new Game( "game-name", "creator" ) } );
            #endregion

            var hub = this.GetHub( connectionId, gameHandler: gameHandler );

            var result = hub.GetGames();

            Assert.IsTrue( result.Count() == 1 );
        }


        [TestMethod]
        public void JoinGame_ThrowsIfUserDoesntExist()
        {
            var connectionId = "123";

            var hub = this.GetHub( connectionId );

            AssertIt.Throws<InvalidOperationException>( () =>
            {
                hub.JoinGame( "game-id" );
            } );
        }

        [TestMethod]
        public void JoinGame_ThrowsIfGameDoesntExist()
        {
            var connectionId = "123";

            #region UserHandler
            var userHandler = new Mock<IUserHandler>();

            userHandler.Setup( a => a.GetUser( It.IsAny<string>() ) )
                       .Returns( new User( connectionId, new RegisteredClient() ) );
            #endregion

            var hub = this.GetHub( connectionId, userHandler: userHandler );

            AssertIt.Throws<InvalidOperationException>( () =>
            {
                hub.JoinGame( "game-id" );
            } );
        }

        [TestMethod]
        public void JoinGame_AddsPlayerToGame()
        {
            var connectionId = "123";

            #region UserHandler
            var userHandler = new Mock<IUserHandler>();

            userHandler.Setup( a => a.GetUser( It.IsAny<string>() ) )
                       .Returns( new User( connectionId, new RegisteredClient() ) { Id = "id" } );
            #endregion

            #region GameHandler
            var gameHandler = new Mock<IGameHandler>();

            gameHandler.Setup( a => a.GetGameById( It.IsAny<string>() ) )
                       .Returns( new Game( "game-name", "creator" ) );
            #endregion

            #region GroupManager
            var groupManager = new Mock<IGroupManager>();

            groupManager.Setup( a => a.Add( It.IsAny<string>(), It.IsAny<string>() ) )
                        .Returns( Task.FromResult<object>( null ) );
            #endregion

            var hub = this.GetHub( connectionId, 
                                   userHandler: userHandler,
                                   gameHandler: gameHandler,
                                   groupManager: groupManager );

            hub.JoinGame( "game-id" );

            gameHandler.Verify( a => a.AddPlayer( It.IsAny<Game>(), It.IsAny<string>() ) );
        }

        [TestMethod]
        public void JoinGame_AddsPlayerToGameGroup()
        {
            var connectionId = "123";
            var game = new Game( "game-name", "creator" ) { Id = "game-id" };

            #region UserHandler
            var userHandler = new Mock<IUserHandler>();

            userHandler.Setup( a => a.GetUser( It.IsAny<string>() ) )
                       .Returns( new User( connectionId, new RegisteredClient() ) { Id = "id" } );
            #endregion

            #region GameHandler
            var gameHandler = new Mock<IGameHandler>();

            gameHandler.Setup( a => a.GetGameById( It.IsAny<string>() ) )
                       .Returns( game);
            #endregion

            #region GroupManager
            var groupManager = new Mock<IGroupManager>();

            groupManager.Setup( a => a.Add( It.IsAny<string>(), game.GroupName ) )
                        .Returns( Task.FromResult<object>( null ) );
            #endregion

            var hub = this.GetHub( connectionId,
                                   userHandler: userHandler,
                                   gameHandler: gameHandler,
                                   groupManager: groupManager );

            hub.JoinGame( game.Id );

            groupManager.Verify( a => a.Add( It.IsAny<string>(), game.GroupName ) );
        }

        [TestMethod]
        public void JoinGame_TellsGameGroupThatPlayerJoined()
        {
            Assert.Inconclusive( "Haven't figured a way to mock Client.Groups()" );

            var connectionId = "123";
            var game = new Game( "game-name", "creator" ) { Id = "game-id" };

            #region UserHandler
            var userHandler = new Mock<IUserHandler>();

            userHandler.Setup( a => a.GetUser( It.IsAny<string>() ) )
                       .Returns( new User( connectionId, new RegisteredClient() ) { Id = "id" } );
            #endregion

            #region GameHandler
            var gameHandler = new Mock<IGameHandler>();

            gameHandler.Setup( a => a.GetGameById( It.IsAny<string>() ) )
                       .Returns( game );
            #endregion

            #region GroupManager
            var groupManager = new Mock<IGroupManager>();

            groupManager.Setup( a => a.Add( It.IsAny<string>(), game.GroupName ) )
                        .Returns( Task.FromResult<object>( null ) );
            #endregion
            
            var hub = this.GetHub( connectionId,
                                   userHandler: userHandler,
                                   gameHandler: gameHandler,
                                   groupManager: groupManager );

            hub.JoinGame( game.Id );
        }


        [TestMethod]
        public void LeaveGame_ThrowsIfUserDoesntExist()
        {
            var connectionId = "123";

            var hub = this.GetHub( connectionId );

            AssertIt.Throws<InvalidOperationException>( () =>
            {
                hub.LeaveGame( "game-id" );
            } );
        }

        [TestMethod]
        public void LeaveGame_ThrowsIfGameDoesntExist()
        {
            var connectionId = "123";

            #region UserHandler
            var userHandler = new Mock<IUserHandler>();

            userHandler.Setup( a => a.GetUser( It.IsAny<string>() ) )
                       .Returns( new User( connectionId, new RegisteredClient() ) );
            #endregion

            var hub = this.GetHub( connectionId, userHandler: userHandler );

            AssertIt.Throws<InvalidOperationException>( () =>
            {
                hub.LeaveGame( "game-id" );
            } );
        }

        [TestMethod]
        public void LeaveGame_RemovesGameIfIsCreator()
        {
            var connectionId = "123";
            var user = new User( connectionId, new RegisteredClient() ) { Id = "user-id" };
            var game = new Game( "game-name", user.Id );

            #region UserHandler
            var userHandler = new Mock<IUserHandler>();

            userHandler.Setup( a => a.GetUser( It.IsAny<string>() ) )
                       .Returns( user );
            #endregion

            #region GameHandler
            var gameHandler = new Mock<IGameHandler>();

            gameHandler.Setup( a => a.GetGameById( It.IsAny<string>() ) )
                       .Returns( game );
            #endregion

            var hub = this.GetHub( connectionId,
                                   userHandler: userHandler,
                                   gameHandler: gameHandler );

            hub.LeaveGame( game.Id );

            gameHandler.Verify( a => a.RemoveGame( It.IsAny<string>() ) );
        }

        [TestMethod]
        public void LeaveGame_TellsAllRemovedGameIfIsCreator()
        {
            var calledGameRemoved = false;
            var connectionId = "123";
            var user = new User( connectionId, new RegisteredClient() ) { Id = "user-id" };
            var game = new Game( "game-name", user.Id );

            #region UserHandler
            var userHandler = new Mock<IUserHandler>();

            userHandler.Setup( a => a.GetUser( It.IsAny<string>() ) )
                       .Returns( user );
            #endregion

            #region GameHandler
            var gameHandler = new Mock<IGameHandler>();

            gameHandler.Setup( a => a.GetGameById( It.IsAny<string>() ) )
                       .Returns( game );
            #endregion

            var hub = this.GetHub( connectionId,
                                   userHandler: userHandler,
                                   gameHandler: gameHandler );

            dynamic all = new ExpandoObject();
            all.gameRemoved = new Action<string>( ( gameId ) =>
            {
                calledGameRemoved = true;
            } );
            hub.Clients.All = all;

            hub.LeaveGame( game.Id );

            Assert.IsTrue( calledGameRemoved );
        }

        [TestMethod]
        public void LeaveGame_TellsGroupLeftGameIfIsntCreator()
        {
            Assert.Inconclusive( "Haven't figured a way to mock Client.Groups()" );

            var connectionId = "123";
            var user = new User( connectionId, new RegisteredClient() ) { Id = "user-id" };
            var game = new Game( "game-name", "not-me" );

            #region UserHandler
            var userHandler = new Mock<IUserHandler>();

            userHandler.Setup( a => a.GetUser( It.IsAny<string>() ) )
                       .Returns( user );
            #endregion

            #region GameHandler
            var gameHandler = new Mock<IGameHandler>();

            gameHandler.Setup( a => a.GetGameById( It.IsAny<string>() ) )
                       .Returns( game );
            #endregion

            var hub = this.GetHub( connectionId,
                                   userHandler: userHandler,
                                   gameHandler: gameHandler );
        }

        [TestMethod]
        public void LeaveGame_RemovesFromGroupIfIsntCreator()
        {
            var connectionId = "123";
            var user = new User( connectionId, new RegisteredClient() ) { Id = "user-id" };
            var game = new Game( "game-name", "not-me" );

            #region UserHandler
            var userHandler = new Mock<IUserHandler>();

            userHandler.Setup( a => a.GetUser( It.IsAny<string>() ) )
                       .Returns( user );
            #endregion

            #region GameHandler
            var gameHandler = new Mock<IGameHandler>();

            gameHandler.Setup( a => a.GetGameById( It.IsAny<string>() ) )
                       .Returns( game );
            #endregion

            #region GroupManager
            var groupManager = new Mock<IGroupManager>();
            #endregion

            var hub = this.GetHub( connectionId,
                                   userHandler: userHandler,
                                   gameHandler: gameHandler,
                                   groupManager: groupManager );
           
            hub.LeaveGame( game.Id );

            groupManager.Verify( a => a.Remove( It.IsAny<string>(), game.GroupName ) );
        }


        private TestableLobby GetHub( string connectionId,
                                      StateChangeTracker clientState  = null,
                                      IDictionary<string, Cookie> cookies = null,
                                      Mock<IGroupManager> groupManager = null,
                                      Mock<IGameHandler> gameHandler = null,
                                      Mock<IRegistrationHandler> registrationHandler = null,
                                      Mock<IUserHandler> userHandler = null,
                                      Mock<IMappingHandler> mappingHandler = null,
                                      Mock<IScenarioHandler> scenarioHandler = null )
        {
            clientState = clientState ?? new StateChangeTracker();
            cookies = cookies ?? new Dictionary<string, Cookie>();

            var lobby = new TestableLobby( gameHandler ?? new Mock<IGameHandler>(),
                                           registrationHandler ?? new Mock<IRegistrationHandler>(),
                                           userHandler ?? new Mock<IUserHandler>(),
                                           mappingHandler ?? new Mock<IMappingHandler>(),
                                           scenarioHandler ?? new Mock<IScenarioHandler>() );

            lobby.Clients = new HubConnectionContext( new Mock<IHubPipelineInvoker>().Object,
                                                      new Mock<IConnection>().Object,
                                                      "Lobby",
                                                      connectionId,
                                                      clientState );

            var request = new Mock<IRequest>();
            request.Setup( a => a.Cookies ).Returns( cookies );

            lobby.Context = new HubCallerContext( request.Object, connectionId );
            lobby.Groups = ( groupManager ?? new Mock<IGroupManager>() ).Object;

            return lobby;
        }
    }

    public class TestableLobby : Lobby
    {
        public TestableLobby( Mock<IGameHandler> gameHandler,
                              Mock<IRegistrationHandler> registrationHandler,
                              Mock<IUserHandler> userHandler,
                              Mock<IMappingHandler> mappingHandler,
                              Mock<IScenarioHandler> scenarioHandler )
            : base( gameHandler.Object, 
                    registrationHandler.Object, 
                    userHandler.Object, 
                    mappingHandler.Object,
                    scenarioHandler.Object )
        {
        }
    }
}
