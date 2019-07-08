
using System;

namespace Caesura.Standard.Logging
{
    using System.Collections.Generic;
    
    public interface ILoggingHandler
    {
        void Enqueue(LogInformation info);
        void AddHandler(BaseLogEventHandler handler);
        Boolean RemoveHandler(BaseLogEventHandler handler);
    }
}
