using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemfire
{
    public interface ILobbyClient
    {
        void gameCreated( GameDto game );
        void gameRemoved( string gameId );
        void joinedGame( string gameId, UserDto user );
        void leftGame( string gameId, string userId );
    }
}
