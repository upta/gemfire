using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;

namespace Gemfire.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILoginHandler loginHandler;
        private readonly IRegistrationHandler registrationHandler;

        public HomeController( ILoginHandler loginHandler, IRegistrationHandler registrationHandler )
        {
            this.loginHandler = loginHandler;
            this.registrationHandler = registrationHandler;
        }

        public ActionResult Index()
        {
            var state = Request.Cookies[ "gemfire.state" ];
            var vm = new Gemfire.ViewModels.Home.Index();

            if ( state != null )
            {
                var rc = JsonConvert.DeserializeObject<RegisteredClient>( HttpUtility.UrlDecode( state.Value ) );

                rc.Identity = this.loginHandler.DecryptIdentity( rc.Identity );
                rc.DisplayName = WebUtility.HtmlEncode( rc.DisplayName );

                if ( rc.RegistrationId == null )
                {
                    this.registrationHandler.Register( rc );

                    this.loginHandler.AddOrUpdateState( rc, this.HttpContext );
                }

                vm.IsAuthenticated = true;
            }

            return View( vm );
        }
    }
}
