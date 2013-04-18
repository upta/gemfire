using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Gemfire.Tests
{
    [TestClass]
    public class UserHandlerTests
    {
        [TestMethod]
        public void AddUser_GetsAdded()
        {
            var handler = new UserHandler( new Mock<IRepository>().Object );
            var user = new User( "test-conn", new RegisteredClient() );

            handler.AddUser( user );

            Assert.IsTrue( handler.UserExists( user.ConnectionId ) );
        }

        [TestMethod]
        public void AddUser_GetsSaved()
        {
            bool saveCalled = false;
            var mock = new Mock<IRepository>();
            mock.Setup( a => a.Save<User>( It.IsAny<User>() ) )
                .Callback( () => saveCalled = true );

            var handler = new UserHandler( mock.Object );
            var user = new User( "test-conn", new RegisteredClient() );

            handler.AddUser( user );

            Assert.IsTrue( saveCalled );
        }

        [TestMethod]
        public void FindUserById_FindsInCollection()
        {
            var handler = new UserHandler( new Mock<IRepository>().Object );

            var user = new User( "test-conn", new RegisteredClient() );
            handler.AddUser( user );

            var foundUser = handler.FindUserById( user.Id );

            Assert.AreEqual( user.Id, foundUser.Id );
        }

        [TestMethod]
        public void FindUserById_FindsInRepository()
        {
            var user = new User( "test-conn", new RegisteredClient() );

            var mock = new Mock<IRepository>();
            mock.Setup( a => a.Find<User>() )
                .Returns( new List<User>() { user }.AsQueryable() );

            var handler = new UserHandler( mock.Object );

            var foundUser = handler.FindUserById( user.Id );

            Assert.AreEqual( user.Id, foundUser.Id );
        }

        [TestMethod]
        public void FindUserByIdentity_FindsInCollection()
        {
            var handler = new UserHandler( new Mock<IRepository>().Object );

            var user = new User( "test-conn", new RegisteredClient
            {
                Identity = "test-identity"
            } );

            handler.AddUser( user );

            var foundUser = handler.FindUserByIdentity( user.RegistrationTicket.Identity );

            Assert.AreEqual( user.RegistrationTicket.Identity, foundUser.RegistrationTicket.Identity );
        }

        [TestMethod]
        public void FindUserByIdentity_FindsInRepository()
        {
            var user = new User( "test-conn", new RegisteredClient
            {
                Identity = "test-identity"
            } );

            var mock = new Mock<IRepository>();
            mock.Setup( a => a.Find<User>() )
                .Returns( new List<User>() { user }.AsQueryable() );

            var handler = new UserHandler( mock.Object );

            var foundUser = handler.FindUserByIdentity( user.RegistrationTicket.Identity );

            Assert.AreEqual( user.RegistrationTicket.Identity, foundUser.RegistrationTicket.Identity );
        }

        [TestMethod]
        public void GetUser_FindsInCollection()
        {
            var handler = new UserHandler( new Mock<IRepository>().Object );
            var user = new User( "test-conn", new RegisteredClient() );
            handler.AddUser( user );

            var foundUser = handler.GetUser( user.ConnectionId );

            Assert.AreEqual( user.Id, foundUser.Id );
        }

        [TestMethod]
        public void ReassignUser_ChangesConnectionId()
        {
            var handler = new UserHandler( new Mock<IRepository>().Object );
            var user = new User( "test-conn", new RegisteredClient() );

            handler.AddUser( user );

            handler.ReassignUser( "new-test-conn", user );

            Assert.AreEqual( user.ConnectionId, "new-test-conn" );
            Assert.IsNull( handler.GetUser( "test-conn" ) );
            Assert.IsNotNull( handler.GetUser( "new-test-conn" ) );
        }

        [TestMethod]
        public void UserExists_ReturnsFalseIfDoesntExist()
        {
            var handler = new UserHandler( new Mock<IRepository>().Object );

            var exists = handler.UserExists( "test-conn-doesnt-exist" );

            Assert.IsFalse( exists );
        }

        [TestMethod]
        public void UserExists_ReturnsTrueIfExists()
        {
            var handler = new UserHandler( new Mock<IRepository>().Object );
            var user = new User( "test-conn", new RegisteredClient() );
            handler.AddUser( user );

            var exists = handler.UserExists( "test-conn" );

            Assert.IsTrue( exists );
        }
    }
}
