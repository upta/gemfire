using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;

namespace Gemfire
{
    public class LoginHandler : ILoginHandler
    {
        public void AddOrUpdateState( RegisteredClient rc, HttpContextBase context )
        {
            var state = JsonConvert.SerializeObject( new RegisteredClient( rc.RegistrationId, EncryptIdentity( rc.Identity ), rc.DisplayName, rc.Photo ) );

            var cookie = new HttpCookie( "gemfire.state" )
            {
                Expires = DateTime.Now.AddDays( 30 ),
                Value = state
            };

            context.Response.Cookies.Set( cookie );
        }

        public string DecryptIdentity( string identity )
        {
            var encrypted = HttpServerUtility.UrlTokenDecode( identity );
            var id = MachineKey.Unprotect( encrypted, "Gemfire.Identity" );

            return Encoding.UTF8.GetString( id );
        }

        public string EncryptIdentity( string identity )
        {
            var id = Encoding.UTF8.GetBytes( identity );
            var encrypted = MachineKey.Protect( id, "Gemfire.Identity" );

            return HttpServerUtility.UrlTokenEncode( encrypted );
        }
    }
}