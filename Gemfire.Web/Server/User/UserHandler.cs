using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gemfire
{
    public class UserHandler : IUserHandler
    {
        private readonly IRepository repository;

        private ConcurrentDictionary<string, User> users = new ConcurrentDictionary<string, User>();
        

        public UserHandler( IRepository repository )
        {
            this.repository = repository;
        }


        public void AddUser( User user )
        {
            this.repository.Save<User>( user );

            this.users.TryAdd( user.ConnectionId, user );
        }

        public User FindUserById( string id )
        {
            var user = this.users.Values.Where( a => a.Id == id ).FirstOrDefault();

            if ( user != null )
            {
                return user;
            }

            return this.repository.Find<User>().FirstOrDefault( a => a.Id == id );
        }

        public User FindUserByIdentity( string identity )
        {
            var user = this.users.Values.Where( a => a.RegistrationTicket.Identity == identity ).FirstOrDefault();

            if ( user != null )
            {
                return user;
            }

            return this.repository.Find<User>().FirstOrDefault( a => a.RegistrationTicket.Identity == identity );
        }

        public User GetUser( string connectionId )
        {
            if ( this.UserExists( connectionId ) )
            {
                return this.users[ connectionId ];
            }

            return null;
        }

        public void ReassignUser( string connectionId, User user )
        {
            if ( user.ConnectionId != null )
            {
                User garbage;
                this.users.TryRemove( user.ConnectionId, out garbage );
            }

            user.ConnectionId = connectionId;
            this.users.TryAdd( connectionId, user );
        }

        public bool UserExists( string connectionId )
        {
            return this.users.ContainsKey( connectionId );
        }
    }
}