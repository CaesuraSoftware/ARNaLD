
using System;

namespace Caesura.PerformanceMonitor.Display
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Monitor;
    
    public class View
    {
        public Int32 RefreshRate { get; set; }
        public Boolean Running { get; private set; }
        private Thread RenderThread { get; set; }
        private IMonitor Monitor { get; set; }
        private MonitorResult CurrentStatus { get; set; }
        private DateTime RefreshStartTime { get; set; }
        private StringBuilder InputBuilder { get; set; }
        
        public View()
        {
            this.RenderThread               = new Thread(this.Run);
            this.RenderThread.IsBackground  = true;
            this.RefreshStartTime           = DateTime.UtcNow;
            this.InputBuilder               = new StringBuilder();
        }
        
        public View(Int32 refreshRate, IMonitor monitorHandle) : this()
        {
            this.RefreshRate                = refreshRate;
            this.Monitor                    = monitorHandle;
        }
        
        public void Start()
        {
            if (this.Running)
            {
                return;
            }
            this.Running = true;
            this.RenderThread.Start();
        }
        
        public void Stop()
        {
            this.Running = false;
        }
        
        public void Run()
        {
            while (this.Running)
            {
                Thread.Sleep(50);
                var elapsed = (DateTime.UtcNow - this.RefreshStartTime).TotalMilliseconds;
                if (this.CurrentStatus is null || elapsed > this.RefreshRate)
                {
                    this.CurrentStatus = this.Monitor.GetStatus();
                    this.RefreshStartTime = DateTime.UtcNow;
                    this.ClearScreen();
                }
                this.Render(this.CurrentStatus);
            }
        }
        
        public void AddInput(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Backspace)
            {
                if (this.InputBuilder.Length > 0)
                {
                    this.InputBuilder.Remove(this.InputBuilder.Length - 1, 1);
                }
                return;
            }
            this.InputBuilder.Append(key.KeyChar);
        }
        
        public void ClearInputBuffer()
        {
            this.InputBuilder.Clear();
        }
        
        public void ClearScreen()
        {
            Console.Clear();
        }
        
        public void Render(MonitorResult result)
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"Process: {result.WindowTitle} ({result.Name}) ({result.ProcessId})");
            Console.WriteLine($"Processor %: {result.ProcessorUsagePercent}");
            // Console.WriteLine($"Processor Total: {result.ProcessorTotalUsagePercent}");
            Console.WriteLine($"Memory (MB): {result.MemoryMegabytesWorkingSet} ({result.MemoryBytesWorkingSet / 1024}K)");
            // Console.WriteLine($"Total Memory (MB): {result.MemoryMegabytesTotal} ({result.MemoryBytesTotal / 1024}K)");
            Console.WriteLine("Threads: ");
            foreach (var thread in result.Threads)
            {
                Console.WriteLine($" Thread ID {thread.ThreadId}: Process %: {thread.ProcessorUsagePercent}");
            }
            Console.WriteLine();
            Console.WriteLine($"> {this.InputBuilder}_");
        }
    }
}
