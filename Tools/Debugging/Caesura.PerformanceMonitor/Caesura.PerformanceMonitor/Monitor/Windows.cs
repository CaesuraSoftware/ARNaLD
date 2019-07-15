
using System;

namespace Caesura.PerformanceMonitor.Monitor
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Diagnostics;
    
    public class Windows : IMonitor
    {
        private Int32 b_ProcessId;
        public Int32 ProcessId { 
            get => this.b_ProcessId; 
            set { 
                if (value == this.b_ProcessId)
                {
                    return;
                }
                this.Started        = false;
                this.OnTargetExit?.Invoke();
                this.Exited         = false; 
                this.b_ProcessId    = value;
            } 
        }
        public Boolean ProcessIsAlive => !this.TargetProcess.HasExited;
        public event Action OnTargetExit;
        public String Name => this.TargetProcess.ProcessName;
        private Process TargetProcess { get; set; }
        private Boolean Started { get; set; }
        private TimeSpan InitialTime { get; set; }
        private TimeSpan PreviousTime { get; set; }
        private DateTime LastMonitorTime { get; set; }
        private DateTime StartTime { get; set; }
        private List<ThreadTimeTracker> ThreadTimes { get; set; }
        private Boolean Exited { get; set; }
        
        public Windows()
        {
            this.Started            = false;
            this.PreviousTime       = new TimeSpan(0);
            this.LastMonitorTime    = DateTime.UtcNow;
            this.StartTime          = DateTime.UtcNow;
            this.ThreadTimes        = new List<ThreadTimeTracker>();
            this.Exited             = false;
        }
        
        public Windows(Int32 pid) : this()
        {
            this.ProcessId = pid;
        }
        
        public MonitorResult GetStatus()
        {
            if (this.Exited)
            {
                return null;
            }
            
            try
            {
                return this.BackingStatus();
            }
            catch (ArgumentException)
            {
                if (!this.Exited)
                {
                    this.OnTargetExit?.Invoke();
                }
                this.Exited = true;
                return null;
            }
        }
        
        private MonitorResult BackingStatus()
        {
            this.TargetProcess              = Process.GetProcessById(this.ProcessId);
            
            this.TargetProcess.Exited += (s, e) => 
            {
                if (!this.Exited)
                {
                    this.Started = false;
                    this.OnTargetExit?.Invoke();
                }
                this.Exited = true;
            };
            
            if (!this.Started)
            {
                this.InitialTime            = this.TargetProcess.TotalProcessorTime;
                this.Started                = true;
            }
            
            var currentTime                 = this.TargetProcess.TotalProcessorTime - this.InitialTime;
            
            var result                      = new MonitorResult();
            
            result.ProcessId                = this.TargetProcess.Id;
            
            result.Name                     = this.TargetProcess.ProcessName;
            
            result.WindowTitle              = this.TargetProcess.MainWindowTitle;
            
            result.ProcessorUsage           = (
                (currentTime - this.PreviousTime).TotalSeconds / 
                (Environment.ProcessorCount * DateTime.UtcNow.Subtract(this.LastMonitorTime).TotalSeconds)
            );
            
            result.ProcessorTotalUsage      = (
                currentTime.TotalSeconds / 
                (Environment.ProcessorCount * DateTime.UtcNow.Subtract(StartTime).TotalSeconds)
            );
            
            result.MemoryBytesWorkingSet    = this.TargetProcess.WorkingSet64;
            
            result.MemoryBytesTotal         = this.TargetProcess.PrivateMemorySize64;
            
            var upthreads                   = this.TargetProcess.Threads;
            var threads                     = new List<ProcessThread>(upthreads.Count);
            foreach (ProcessThread thread in upthreads)
            {
                threads.Add(thread);
            }
            foreach (var thread in threads)
            {
                Int32 cid = 0;
                try
                {
                    cid = thread.Id;
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
                        ((currentThreadTime - tracker.PreviousTime).TotalSeconds / 
                        DateTime.UtcNow.Subtract(this.LastMonitorTime).TotalSeconds) /
                        Environment.ProcessorCount
                    );
                    
                    tracker.PreviousTime    = currentThreadTime;
                    result.Threads.Add(mt);
                }
                catch (InvalidOperationException)
                {
                    var ttt = this.ThreadTimes.Find(x => x.ThreadId == cid);
                    this.ThreadTimes.Remove(ttt);
                }
            }
            // remove all threads that have exited
            this.ThreadTimes.RemoveAll(x => threads.All(y => x.ThreadId != y.Id));
            foreach (var thread in threads)
            {
                thread.Dispose();
            }
            
            this.LastMonitorTime            = DateTime.UtcNow;
            this.PreviousTime               = currentTime;
            
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
