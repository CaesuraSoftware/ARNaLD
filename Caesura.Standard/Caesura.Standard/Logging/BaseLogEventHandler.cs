
using System;

namespace Caesura.Standard.Logging
{
    using System.Collections.Generic;
    using System.Linq;
    
    [Flags]
    public enum LogEventKind : Int32
    {
        Information     = 1 << 0,
        Warning         = 1 << 1,
        Error           = 1 << 2,
        Critical        = 1 << 3,
        Debug           = 1 << 4,
        All             = Information | Warning | Error | Critical,
        AllDebug        = All | Debug,
    }
    
    public delegate void LogDelegate(LogInformation info);
    
    public abstract class BaseLogEventHandler
    {
        public abstract LogEventKind EventKind { get; }
        public abstract void Log(LogInformation info);
    }
}
