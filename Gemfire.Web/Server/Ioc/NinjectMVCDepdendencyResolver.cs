using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;

namespace Ninject
{
    public class NinjectMVCDependencyResolver : IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectMVCDependencyResolver( IKernel kernel )
        {
            this.kernel = kernel;
        }

        public object GetService( Type serviceType )
        {
            return this.kernel.TryGet( serviceType );
        }

        public IEnumerable<object> GetServices( Type serviceType )
        {
            return this.kernel.GetAll( serviceType );
        }
    }
}