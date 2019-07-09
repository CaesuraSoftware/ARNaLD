
using System;

namespace Caesura.Standard.Logging
{
    using System.Collections.Generic;
    using System.Linq;
    
    public class LogEventHandler : BaseLogEventHandler
    {
        protected LogEventKind _eventKind;
        public override LogEventKind EventKind => this._eventKind;
        public event LogDelegate OnLog;
        
        public LogEventHandler()
        {
            
        }
        
        public LogEventHandler(LogEventKind kind, LogDelegate onLog) : this()
        {
            this._eventKind = kind;
            this.OnLog += onLog;
        }
        
        public override void Log(LogInformation info)
        {
            this.OnLog.Invoke(info);
        }
    }
}
