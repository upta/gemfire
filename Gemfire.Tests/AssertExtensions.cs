using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

static public class AssertIt
{
    static public void Throws<T>( Action action ) where T : Exception
    {
        try
        {
            action();
            Assert.Fail( "Didn't throw exception of type " + typeof( T ).Name );
        }
        catch ( T ex )
        { }
    }
}
