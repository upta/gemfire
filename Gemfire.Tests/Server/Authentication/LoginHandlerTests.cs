using System;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace Gemfire.Tests
{
    [TestClass]
    public class LoginHandlerTests
    {
        [TestMethod]
        public void AddOrUpdateState_AddsStateCookie()
        {
            var cookies = new HttpCookieCollection();
            var handler = new LoginHandler();
            var rc = new RegisteredClient
            {
                DisplayName = "test-name",
                Identity = "test-identity",
                Photo = "test-photo",
                RegistrationId = "test-reg-id"
            };

            var mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup( a => a.Cookies )
                        .Returns( cookies );

            var mockContext = new Mock<HttpContextBase>();
            mockContext.Setup( a => a.Response )
                       .Returns( mockResponse.Object );

            handler.AddOrUpdateState( rc, mockContext.Object );

            Assert.IsNotNull( cookies[ "gemfire.state" ] );
        }

        [TestMethod]
        public void AddOrUpdateState_StoresValidState()
        {
            var cookies = new HttpCookieCollection();
            var handler = new LoginHandler();
            var rc = new RegisteredClient
            {
                DisplayName = "test-name",
                Identity = "test-identity",
                Photo = "test-photo",
                RegistrationId = "test-reg-id"
            };

            var mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup( a => a.Cookies )
                        .Returns( cookies );

            var mockContext = new Mock<HttpContextBase>();
            mockContext.Setup( a => a.Response )
                       .Returns( mockResponse.Object );

            handler.AddOrUpdateState( rc, mockContext.Object );

            var cookie = cookies[ "gemfire.state" ];
            var state = JsonConvert.DeserializeObject<RegisteredClient>( cookie.Value );
            var decryptedIdentity = handler.DecryptIdentity( state.Identity );

            Assert.AreEqual( rc.Identity, decryptedIdentity );
            Assert.AreEqual( rc.DisplayName, state.DisplayName );
            Assert.AreEqual( rc.Photo, state.Photo );
            Assert.AreEqual( rc.RegistrationId, state.RegistrationId );
        }


        [TestMethod]
        public void EncryptedAndDecryptedMatch()
        {
            var handler = new LoginHandler();
            var identity = "identity-value";

            var encrypted = handler.EncryptIdentity( identity );
            var decrypted = handler.DecryptIdentity( encrypted );

            Assert.AreEqual( identity, decrypted );
        }
    }
}
