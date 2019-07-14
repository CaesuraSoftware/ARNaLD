
using System;

namespace Caesura.PerformanceMonitor.Monitor
{
    using System.Collections.Generic;
    using System.Linq;
    
    public class MonitorResult
    {
        public Double ProcessorUsage { get; set; }
        public String ProcessorUsagePercent => String.Format("{0:0.0}", this.ProcessorUsage);
        public Int64 MemoryBytesUsed { get; set; }
        public Int64 MemoryMegabytesUsed => this.MemoryBytesUsed / 1024 / 1024;
        public Int32 ClrThreads { get; set; }
        public Int32 ThreadCount => this.Threads?.Count ?? 0;
        public List<MonitorThread> Threads { get; set; }
        
        public MonitorResult()
        {
            this.Threads = new List<MonitorThread>();
        }
        
        public class MonitorThread
        {
            public Int32 ThreadId { get; set; }
            public Int32 BasePriority { get; set; }
            public Int32 CurrentPriority { get; set; }
            public Double ProcessorUsage { get; set; }
            public String ProcessorUsagePercent => String.Format("{0:0.0}", this.ProcessorUsage);
        }
    }
}
