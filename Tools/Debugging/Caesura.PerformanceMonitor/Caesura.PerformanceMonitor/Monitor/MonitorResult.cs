
using System;

namespace Caesura.PerformanceMonitor.Monitor
{
    using System.Collections.Generic;
    using System.Linq;
    
    public class MonitorResult
    {
        public Int32 ProcessId { get; set; }
        public String Name { get; set; }
        public String WindowTitle { get; set; }
        public Double ProcessorUsage { get; set; }
        public Double ProcessorUsageNormalized => this.ProcessorUsage * 100;
        public String ProcessorUsagePercent => String.Format("{0:0.0}", this.ProcessorUsage * 100);
        public Double ProcessorTotalUsage { get; set; }
        public String ProcessorTotalUsagePercent => String.Format("{0:0.0}", this.ProcessorTotalUsage * 100);
        public Int64 MemoryBytesWorkingSet { get; set; }
        public Int64 MemoryMegabytesWorkingSet => this.MemoryBytesWorkingSet / 1024 / 1024;
        public Int64 MemoryBytesTotal { get; set; }
        public Int64 MemoryMegabytesTotal => this.MemoryBytesTotal / 1024 / 1024;
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
            public Double ProcessorUsageNormalized => this.ProcessorUsage * 100;
            public String ProcessorUsagePercent => String.Format("{0:0.0}", this.ProcessorUsage * 100);
        }
    }
}
