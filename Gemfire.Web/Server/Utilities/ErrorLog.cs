using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Gemfire
{
    public class ErrorLog
    {
        private readonly static Lazy<ErrorLog> instance = new Lazy<ErrorLog>( () => new ErrorLog() );
        public static ErrorLog Instance
        {
            get
            {
                return instance.Value;
            }
        }

        private const string source = ".Net Runtime";

        public void Log( Exception ex, string customMessage = "" )
        {
            Task.Factory.StartNew( () =>
            {
                EventLog.WriteEntry( source, ex.ToString() + "    CALLSTACK: " + ex.StackTrace + "    CUSTOM MESSAGE: " + customMessage );
            } );
        }
    }
}