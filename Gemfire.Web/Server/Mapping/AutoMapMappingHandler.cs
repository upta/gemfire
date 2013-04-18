using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace Gemfire
{
    public class AutoMapMappingHandler : IMappingHandler
    {
        public AutoMapMappingHandler( IUserHandler userHandler )
        {
            Mapper.Reset();

            Mapper.CreateMap<User, UserDto>()
                  .ForMember( a => a.Name, a => a.MapFrom( b => b.RegistrationTicket.DisplayName ) );

            Mapper.CreateMap<Game, GameDto>()
                  .ForMember( a => a.Players, a => a.MapFrom( b => b.Players.Select( c => userHandler.FindUserById( c ) ) ) );
        }

        public U Map<U>( object source )
        {
            return Mapper.Map<U>( source );
        }
    }
}