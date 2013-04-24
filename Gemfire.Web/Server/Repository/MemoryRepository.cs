using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gemfire
{
    public class MemoryRepository : IRepository
    {
        public void Delete<T>( string id )
        {
            
        }

        public IQueryable<T> Find<T>()
        {
            return new List<T>().AsQueryable();
        }

        public T Save<T>( T entity )
        {
            var idProperty = entity.GetType().GetProperty( "Id" );

            if ( idProperty != null )
            {
                if ( idProperty.GetValue( entity ) == null )
                {
                    idProperty.SetValue( entity, Guid.NewGuid().ToString() );
                }
            }

            return entity;
        }
    }
}