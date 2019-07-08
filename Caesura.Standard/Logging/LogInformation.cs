
using System;

namespace Caesura.Standard.Logging
{
    using System.Collections.Generic;
    using System.Linq;
    
    public class LogInformation
    {
        public LogSource Source { get; set; }
        public LogEventKind Kind { get; set; }
        public TimeSpan Timestamp { get; set; }
        public String Message { get; set; }
        public Exception Exception { get; set; }
        public Object[] Items { get; set; }
    }
}
