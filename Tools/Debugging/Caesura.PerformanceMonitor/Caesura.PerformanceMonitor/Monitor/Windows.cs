
using System;

namespace Caesura.PerformanceMonitor.Monitor
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Diagnostics;
    
    public class Windows : IMonitor
    {
        public Boolean ProcessIsAlive => !this.TargetProcess.HasExited;
        public String Name => this.TargetProcess.ProcessName;
        private Process TargetProcess { get; set; }
        private PerformanceCounter CpuCounter { get; set; }
        private PerformanceCounter RamCounter { get; set; }
        private PerformanceCounter ThreadCounter { get; set; }
        
        public Windows()
        {
            
        }
        
        public Windows(Int32 pid)
        {
            this.TargetProcess = Process.GetProcessById(pid);
            
            // TODO: this returns Time * CPU Cores, meaning a process taking up 100% on a 4 core machine would return 400%
            this.CpuCounter = new PerformanceCounter("Process", "% Processor Time", this.TargetProcess.ProcessName);
            // TODO: display with (ram/1024/1024) for megabytes
            this.RamCounter = new PerformanceCounter("Process", "Working Set", this.TargetProcess.ProcessName);
            this.ThreadCounter = new PerformanceCounter("Process", "Thread Count", this.TargetProcess.ProcessName);
        }
        
        // TODO: method that calls all NextValue()'s for all PerformanceCounters and puts them all in some POCO
        // TODO: foreach (ProcessThread pt in p.Threads) pt.TotalProcessorTime
        public MonitorResult GetStatus()
        {
            throw new NotImplementedException();
        }
        
        public void Dispose()
        {
            this.CpuCounter?.Dispose();
            this.RamCounter?.Dispose();
            this.ThreadCounter?.Dispose();
        }
    }
}
