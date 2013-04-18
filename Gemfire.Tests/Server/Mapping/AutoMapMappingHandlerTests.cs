using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Gemfire.Tests
{
    [TestClass]
    public class AutoMapMappingHandlerTests
    {
        [TestMethod]
        public void MapsGameToGameDto()
        {
            var mock = new Mock<IUserHandler>();

            var user = new User( "test-conn", new RegisteredClient() );
            mock.Setup( a => a.FindUserById( user.Id ) )
                .Returns( user );

            var handler = new AutoMapMappingHandler( mock.Object );
            var game = new Game( "test-game", user.Id );
            game.Players.Add( user.Id );

            var dto = handler.Map<GameDto>( game );

            Assert.AreEqual( dto.CreatedAt, game.CreatedAt );
            Assert.AreEqual( dto.Creator, game.Creator );
            Assert.AreEqual( dto.Id, game.Id );
            Assert.AreEqual( dto.Name, game.Name );
            Assert.AreEqual( dto.Scenario, game.Scenario );
            Assert.AreEqual( dto.Players.Count, game.Players.Count );
            Assert.AreEqual( dto.Players.First().Id, game.Players.First() );
        }

        [TestMethod]
        public void MapsUserToUserDto()
        {
            var mock = new Mock<IUserHandler>();
            var handler = new AutoMapMappingHandler( mock.Object );
            var user = new User( "test-conn", new RegisteredClient
            {
                DisplayName = "test-name"
            } );

            var dto = handler.Map<UserDto>( user );

            Assert.AreEqual( dto.Id, user.Id );
            Assert.AreEqual( dto.Name, user.RegistrationTicket.DisplayName );
        }
    }
}
