using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gemfire
{
    public interface IRegistrationHandler
    {
        bool RegistrationExists( string registrationId );
        RegisteredClient Register( RegisteredClient existing );
        RegisteredClient Register( string identity, string displayName, string photo );
        RegisteredClient RemoveRegistration( string registrationId );
    }
}