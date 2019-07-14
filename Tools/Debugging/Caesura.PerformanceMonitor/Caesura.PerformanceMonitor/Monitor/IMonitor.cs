
using System;

namespace Caesura.PerformanceMonitor.Monitor
{
    using System.Collections.Generic;
    
    public interface IMonitor
    {
        Boolean ProcessIsAlive { get; }
        String Name { get; }
        MonitorResult GetStatus();
    }
}
