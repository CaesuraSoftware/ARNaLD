
using System;

namespace Caesura.PerformanceMonitor.Monitor
{
    using System.Collections.Generic;
    
    public interface IMonitor : IDisposable
    {
        Int32 ProcessId { get; set; }
        Boolean ProcessIsAlive { get; }
        String Name { get; }
        MonitorResult GetStatus();
    }
}
