using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Gemfire
{
    public class RegistrationHandler : IRegistrationHandler
    {
        private readonly TimeSpan timeoutAfter = TimeSpan.FromSeconds( 30 );

        private ConcurrentDictionary<string, RegisteredClient> registrationList;
        private Timer timeoutLoop;

        public RegistrationHandler()
            : this( TimeSpan.FromSeconds( 30 ) )
        {
        }

        public RegistrationHandler( TimeSpan timeoutAfter )
        {
            this.timeoutAfter = timeoutAfter;
            this.registrationList = new ConcurrentDictionary<string, RegisteredClient>();
            this.timeoutLoop = new Timer( CheckTimeouts, null, Convert.ToInt32( timeoutAfter.TotalMilliseconds * 0.5 ), Convert.ToInt32( timeoutAfter.TotalMilliseconds * 0.5 ) );
        }


        public bool RegistrationExists( string registrationId )
        {
            return this.registrationList.ContainsKey( registrationId );
        }

        public RegisteredClient Register( RegisteredClient existing )
        {
            existing.RegistrationId = Guid.NewGuid().ToString();
            this.registrationList.TryAdd( existing.RegistrationId, existing );

            return existing;
        }

        public RegisteredClient Register( string identity, string displayName, string photo )
        {
            var rc = new RegisteredClient( Guid.NewGuid().ToString(), identity, displayName, photo );
            this.registrationList.TryAdd( rc.RegistrationId, rc );

            return rc;
        }

        public RegisteredClient RemoveRegistration( string registrationId )
        {
            RegisteredClient rc;
            this.registrationList.TryRemove( registrationId, out rc );

            return rc;
        }


        private void CheckTimeouts( object state )
        {
            try
            {
                // Excuted once every TIMEOUT_AFTER / 2 seconds
                var now = DateTime.UtcNow;
                RegisteredClient garbage;

                foreach ( var rc in this.registrationList.Values )
                {
                    if ( ( now - rc.InitializedAt ) > timeoutAfter )
                    {
                        this.registrationList.TryRemove( rc.RegistrationId, out garbage );
                    }
                }
            }
            catch ( Exception ex )
            {
                ErrorLog.Instance.Log( ex );
            }
        }
    }
}