using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gemfire.Tests
{
    [TestClass]
    public class RegistrationHandlerTests
    {
        [TestMethod]
        public void Register_ExistsWhenAddedByParams()
        {
            var handler = new RegistrationHandler();
            var rc = handler.Register( "test-identity", "test-name", "test-photo" );

            var exists = handler.RegistrationExists( rc.RegistrationId );

            Assert.IsTrue( exists );
        }

        [TestMethod]
        public void Register_RecyclesExisting()
        {
            var handler = new RegistrationHandler();
            var rc = new RegisteredClient
            {
                DisplayName = "test-name",
                Identity = "test-identity",
                Photo = "test-photo"
            };

            var registered = handler.Register( rc );
            var exists = handler.RegistrationExists( registered.RegistrationId );

            Assert.IsTrue( exists );
            Assert.AreEqual( rc.DisplayName, registered.DisplayName );
            Assert.AreEqual( rc.Identity, registered.Identity );
            Assert.AreEqual( rc.Photo, registered.Photo );
        }

        [TestMethod]
        public void RemoveRegistration_PopsFromList()
        {
            var handler = new RegistrationHandler();
            var rc = handler.Register( "test-identity", "test-name", "test-photo" );

            var removed = handler.RemoveRegistration( rc.RegistrationId );
            var exists = handler.RegistrationExists( rc.RegistrationId );

            Assert.IsFalse( exists );
            Assert.AreEqual( rc.DisplayName, removed.DisplayName );
            Assert.AreEqual( rc.Identity, removed.Identity );
            Assert.AreEqual( rc.Photo, removed.Photo );
        }

        [TestMethod]
        public void RemovesAfterTimeout()
        {
            var timeout = TimeSpan.FromSeconds( 1 );
            var handler = new RegistrationHandler( timeout );
            var rc = handler.Register( "test-identity", "test-name", "test-photo" );

            Thread.Sleep( timeout.Add( TimeSpan.FromSeconds( 0.5 ) ) );

            var exists = handler.RegistrationExists( rc.RegistrationId );

            Assert.IsFalse( exists );
        }
    }
}
