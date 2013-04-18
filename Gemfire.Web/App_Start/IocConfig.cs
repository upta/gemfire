using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Ninject;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Configuration;

namespace Gemfire
{
    public class IocConfig
    {
        static public StandardKernel Register( HttpConfiguration config )
        {
            var kernel = new StandardKernel();

            kernel.Bind<IRepository>().ToConstant( new MongoRepository( ConfigurationManager.ConnectionStrings[ "Mongo" ].ConnectionString, "gemfire" ) );

            var repo = kernel.Get<IRepository>();

            kernel.Bind<IGameHandler>().ToConstant( new GameHandler( repo ) );
            kernel.Bind<ILoginHandler>().ToConstant( new LoginHandler() );
            kernel.Bind<IRegistrationHandler>().ToConstant( new RegistrationHandler() );
            kernel.Bind<IUserHandler>().ToConstant( new UserHandler( repo ) );
            kernel.Bind<IMappingHandler>().ToConstant( new AutoMapMappingHandler( kernel.Get<IUserHandler>() ) );

            DependencyResolver.SetResolver( new NinjectMVCDependencyResolver( kernel ) );
            GlobalHost.DependencyResolver = new NinjectSignalRDependencyResolver( kernel );

            return kernel;
        }
    }
}