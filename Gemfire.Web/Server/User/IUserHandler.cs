using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gemfire
{
    public interface IUserHandler
    {
        void AddUser( User user );
        User FindUserById( string id);
        User FindUserByIdentity( string identity );
        User GetUser( string connectionId );
        void ReassignUser( string connectionId, User user );
        bool UserExists( string connectionId );
    }
}