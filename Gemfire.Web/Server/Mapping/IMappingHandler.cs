using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemfire
{
    public interface IMappingHandler
    {
        U Map<U>( object entity );
    }
}
