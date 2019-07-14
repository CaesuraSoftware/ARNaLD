
using System;

namespace Caesura.PerformanceMonitor.Monitor
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Diagnostics;
    
    public class Windows : IMonitor
    {
        public Boolean ProcessIsAlive => !this.TargetProcess.HasExited;
        public String Name => this.TargetProcess.ProcessName;
        private Process TargetProcess { get; set; }
        private Boolean Started { get; set; }
        private TimeSpan InitialTime { get; set; }
        private TimeSpan PreviousTime { get; set; }
        private DateTime LastMonitorTime { get; set; }
        private DateTime StartTime { get; set; }
        private List<ThreadTimeTracker> ThreadTimes { get; set; }
        
        public Windows()
        {
            this.Started            = false;
            this.PreviousTime       = new TimeSpan(0);
            this.LastMonitorTime    = DateTime.UtcNow;
            this.StartTime          = DateTime.UtcNow;
            this.ThreadTimes        = new List<ThreadTimeTracker>();
        }
        
        public Windows(Int32 pid) : this()
        {
            this.TargetProcess = Process.GetProcessById(pid);
        }
        
        public MonitorResult GetStatus()
        {
            if (!this.Started)
            {
                this.InitialTime        = this.TargetProcess.TotalProcessorTime;
                this.Started            = true;
            }
            
            var currentTime             = this.TargetProcess.TotalProcessorTime - this.InitialTime;
            
            var result                  = new MonitorResult();
            
            result.ProcessorUsage       = (
                (currentTime - this.PreviousTime).TotalSeconds / 
                (Environment.ProcessorCount * DateTime.UtcNow.Subtract(this.LastMonitorTime).TotalSeconds)
            );
            
            result.MemoryBytesUsed      = this.TargetProcess.WorkingSet64;
            
            var upthreads               = this.TargetProcess.Threads;
            var threads                 = new List<ProcessThread>(upthreads.Count);
            foreach (ProcessThread thread in upthreads)
            {
                threads.Add(thread);
            }
            foreach (var thread in threads)
            {
                if (!this.ThreadTimes.Exists(x => x.ThreadId == thread.Id))
                {
                    var ttt             = new ThreadTimeTracker();
                    ttt.ThreadId        = thread.Id;
                    ttt.InitialTime     = thread.TotalProcessorTime;
                    this.ThreadTimes.Add(ttt);
                }
                
                var tracker             = this.ThreadTimes.Find(x => x.ThreadId == thread.Id);
                var currentThreadTime   = thread.TotalProcessorTime - tracker.InitialTime;
                
                var mt                  = new MonitorResult.MonitorThread();
                mt.ThreadId             = thread.Id;
                mt.BasePriority         = thread.BasePriority;
                mt.CurrentPriority      = thread.CurrentPriority;
                mt.ProcessorUsage       = (
                        (currentTime - tracker.PreviousTime).TotalSeconds / 
                        DateTime.UtcNow.Subtract(this.LastMonitorTime).TotalSeconds
                    );
                
                tracker.PreviousTime    = currentThreadTime;
                result.Threads.Add(mt);
            }
            // remove all threads that have exited
            this.ThreadTimes.RemoveAll(x => threads.All(y => x.ThreadId != y.Id));
            
            this.LastMonitorTime        = DateTime.UtcNow;
            this.PreviousTime           = currentTime;
            
            return result;
        }
        
        public void Dispose()
        {
            
        }
        
        internal class ThreadTimeTracker
        {
            public Int32 ThreadId { get; set; }
            public TimeSpan InitialTime { get; set; }
            public TimeSpan PreviousTime { get; set; }
            
            public ThreadTimeTracker()
            {
                this.InitialTime  = new TimeSpan(0);
                this.PreviousTime = new TimeSpan(0);
            }
        }
    }
}
