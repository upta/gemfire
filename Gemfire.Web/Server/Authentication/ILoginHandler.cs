using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gemfire
{
    public interface ILoginHandler
    {
        void AddOrUpdateState( RegisteredClient rc, HttpContextBase context );
        string DecryptIdentity( string identity );
        string EncryptIdentity( string identity );
    }
}