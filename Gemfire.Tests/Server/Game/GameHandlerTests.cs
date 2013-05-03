using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Gemfire.Tests
{
    [TestClass]
    public class GameHandlerTests
    {
        [TestMethod]
        public void AddGame_AddsToCollection()
        {
            var handler = new GameHandler( new Mock<IRepository>().Object );
            var game = new Game( "test-game", "creator-id" )
            {
                Id = "test-id"
            };

            handler.AddGame( game );
            var foundGame = handler.GetGameById( game.Id );

            Assert.AreEqual( foundGame.Id, game.Id );
        }

        [TestMethod]
        public void AddGame_AddsToRepository()
        {
            var repository = new Mock<IRepository>();

            var handler = new GameHandler( repository.Object );
            var game = new Game( "test-game", "creator-id" )
            {
                Id = "test-id"
            };

            handler.AddGame( game );

            repository.Verify( a => a.Save<Game>( It.IsAny<Game>() ) );
        }


        [TestMethod]
        public void AddPlayer_AddsToCollection()
        {
            var handler = new GameHandler( new Mock<IRepository>().Object );
            var userId = "test-user-id";
            var game = new Game( "test-game", "creator-id" );

            handler.AddPlayer( game, userId );

            Assert.IsTrue( game.Players.Any( a => a == userId ) );            
        }

        [TestMethod]
        public void AddPlayer_DoesntDuplicate()
        {
            var handler = new GameHandler( new Mock<IRepository>().Object );
            var userId = "test-user-id";
            var game = new Game( "test-game", "creator-id" );

            handler.AddPlayer( game, userId );
            handler.AddPlayer( game, userId );

            Assert.IsTrue( game.Players.Count( a => a == userId ) == 1 );
        }

        [TestMethod]
        public void AddPlayer_SavesAfterAddingPlayer()
        {
            var repository = new Mock<IRepository>();

            var handler = new GameHandler( repository.Object );
            var userId = "test-user-id";
            var game = new Game( "test-game", "creator-id" );

            handler.AddPlayer( game, userId );

            repository.Verify( a => a.Save<Game>( It.IsAny<Game>() ) );
        }


        [TestMethod]
        public void CreateGameFromScenario_AddsCreatorToPlayers()
        {
            var repository = new Mock<IRepository>();
            var handler = new GameHandler( repository.Object );
            var user = new User( "test-conn", new RegisteredClient() )
            {
                Id = "test-id"
            };

            var game = handler.CreateGameWithScenario( user, "test-scenario", "test-name" );

            Assert.IsTrue( game.Players.Any( a => a == user.Id ) );
        }


        [TestMethod]
        public void GetGameById_FindsInCollection()
        {
            var handler = new GameHandler( new Mock<IRepository>().Object );
            var game = new Game( "test-name", "creator" )
            {
                Id = "test-game-id"
            };

            handler.AddGame( game );

            var foundGame = handler.GetGameById( game.Id );

            Assert.IsNotNull( foundGame );
            Assert.AreEqual( game.Id, foundGame.Id );
        }

        [TestMethod]
        public void GetGameById_FindsInRepository()
        {
            var game = new Game( "test-name", "creator" )
            {
                Id = "test-game-id"
            };

            var repository = new Mock<IRepository>();
            repository.Setup( a => a.Find<Game>() )
                      .Returns( new List<Game>() { game }.AsQueryable() );

            var handler = new GameHandler( repository.Object );
            var foundGame = handler.GetGameById( game.Id );

            Assert.IsNotNull( foundGame );
            Assert.AreEqual( game.Id, foundGame.Id );
        }


        [TestMethod]
        public void GetGames_ReturnsCollectionIfAny()
        {
            var handler = new GameHandler( new Mock<IRepository>().Object );
            var game = new Game( "test-name", "creator" )
            {
                Id = "test-game-id"
            };

            handler.AddGame( game );

            var games = handler.GetGames();

            Assert.IsTrue( games.Any() );
        }

        [TestMethod]
        public void GetGames_PopulatesFromRepositoryIfNotAny()
        {
            var game = new Game( "test-name", "creator" )
            {
                Id = "test-game-id"
            };

            var repository = new Mock<IRepository>();
            repository.Setup( a => a.Find<Game>() )
                      .Returns( new List<Game>() { game }.AsQueryable() );

            var handler = new GameHandler( repository.Object );

            var games = handler.GetGames();

            Assert.IsTrue( games.Any() );
        }


        [TestMethod]
        public void RemoveGame_RemovesFromCollection()
        {
            var handler = new GameHandler( new Mock<IRepository>().Object );
            var game = new Game( "test-name", "creator" )
            {
                Id = "test-game-id"
            };

            handler.AddGame( game );
            handler.RemoveGame( game.Id );

            var games = handler.GetGames();

            Assert.IsFalse( games.Any( a => a.Id == game.Id ) );
        }

        [TestMethod]
        public void RemoveGame_RemovesFromRepository()
        {
            var repository = new Mock<IRepository>();

            var handler = new GameHandler( repository.Object );
            var game = new Game( "test-name", "creator" )
            {
                Id = "test-game-id"
            };

            handler.AddGame( game );
            handler.RemoveGame( game.Id );

            repository.Verify( a => a.Delete<Game>( It.IsAny<string>() ) );
        }


        [TestMethod]
        public void RemovePlayer_RemovesFromCollection()
        {
            var handler = new GameHandler( new Mock<IRepository>().Object );
            var userId = "test-user-id";
            var game = new Game( "test-name", "creator" )
            {
                Id = "test-game-id"
            };

            handler.AddPlayer( game, userId );
            handler.RemovePlayer( game, userId );

            Assert.IsFalse( game.Players.Any( a => a == userId ) );
        }

        [TestMethod]
        public void RemovePlayer_SavesGameAfterRemove()
        {
            var repository = new Mock<IRepository>();

            var handler = new GameHandler( repository.Object );
            var userId = "test-user-id";
            var game = new Game( "test-name", "creator" )
            {
                Id = "test-game-id"
            };

            handler.AddPlayer( game, userId );
            handler.RemovePlayer( game, userId );

            repository.Verify( a => a.Save<Game>( It.IsAny<Game>() ) );
        }
    }
}
