using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;

namespace Gemfire.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILoginHandler loginHandler;
        private readonly IRegistrationHandler registrationHandler;
        private readonly string verifyTokenUrl = "https://rpxnow.com/api/v2/auth_info?apiKey={0}&token={1}";

        public AuthController( ILoginHandler loginHandler, IRegistrationHandler registrationHandler )
        {
            this.loginHandler = loginHandler;
            this.registrationHandler = registrationHandler;
        }

        public ActionResult Index( string path, string token )
        {
            var apiKey = ConfigurationManager.AppSettings[ "janrainAPIKey" ];

            if ( string.IsNullOrEmpty( apiKey ) )
            {
                return this.RedirectToAction( "Index", "Home" );
            }

            var registeredClient = this.GetClientState();

            if ( registeredClient.Identity != null )
            {
                registeredClient = this.registrationHandler.Register( registeredClient );
                this.loginHandler.AddOrUpdateState( registeredClient, this.HttpContext );
            }
            else
            {
                if ( string.IsNullOrEmpty( token ) )
                {
                    return this.RedirectToAction( "Index", "Home" );
                }

                var response = new WebClient().DownloadString( string.Format( this.verifyTokenUrl, apiKey, token ) );

                if ( string.IsNullOrEmpty( response ) )
                {
                    return this.RedirectToAction( "Index", "Home" );
                }

                dynamic j = JsonConvert.DeserializeObject( response );

                if ( j.stat.ToString() != "ok" )
                {
                    return this.RedirectToAction( "Index", "Home" );
                }

                var identity = j.profile.identifier.ToString();
                var displayName = WebUtility.HtmlEncode( j.profile.preferredUsername.ToString() );
                var photo = "";

                if ( j.profile.photo != null )
                {
                    photo = j.profile.photo;
                }
                else if ( j.profile.email != null )
                {
                    photo = "http://www.gravatar.com/avatar/" + ToMD5( j.profile.email.ToString() ) + "?d=404";
                }

                registeredClient = this.registrationHandler.Register( identity, displayName, photo );
                this.loginHandler.AddOrUpdateState( registeredClient, this.HttpContext );
            }

            return this.Redirect( path );
        }

        protected override void OnException( ExceptionContext filterContext )
        {
            ErrorLog.Instance.Log( filterContext.Exception, "Auth crash" );

            base.OnException( filterContext );
        }
        

        private RegisteredClient GetClientState()
        {
            var cookie = this.Request.Cookies[ "gemfire.state" ];
            var cookieState = cookie == null ? null : HttpUtility.UrlDecode( cookie.Value );

            if ( string.IsNullOrEmpty( cookieState ) )
            {
                return new RegisteredClient();
            }
            else
            {
                return JsonConvert.DeserializeObject<RegisteredClient>( cookieState );
            }
        }

        private string ToMD5( string value )
        {
            if ( string.IsNullOrEmpty( value ) )
            {
                return null;
            }

            return string.Join( "", MD5.Create().ComputeHash( Encoding.Default.GetBytes( value ) ).Select( a => a.ToString( "x2" ) ) );
        }
    }
}
