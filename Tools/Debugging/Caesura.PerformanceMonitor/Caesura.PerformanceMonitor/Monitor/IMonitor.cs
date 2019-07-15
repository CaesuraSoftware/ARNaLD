
using System;

namespace Caesura.PerformanceMonitor.Monitor
{
    using System.Collections.Generic;
    
    public interface IMonitor : IDisposable
    {
        Int32 ProcessId { get; set; }
        Boolean ProcessIsAlive { get; }
        event Action OnTargetExit;
        String Name { get; }
        MonitorResult GetStatus();
    }
}
