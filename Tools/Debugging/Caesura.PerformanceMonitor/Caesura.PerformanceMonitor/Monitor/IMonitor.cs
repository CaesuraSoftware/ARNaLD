
using System;

namespace Caesura.PerformanceMonitor.Monitor
{
    using System.Collections.Generic;
    
    public interface IMonitor : IDisposable
    {
        Boolean ProcessIsAlive { get; }
        String Name { get; }
        MonitorResult GetStatus();
    }
}
