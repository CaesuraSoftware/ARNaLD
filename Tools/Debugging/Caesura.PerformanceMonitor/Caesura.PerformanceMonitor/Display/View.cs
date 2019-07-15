
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
        private String TextArea { get; set; }
        
        public View()
        {
            this.RenderThread               = new Thread(this.Run);
            this.RenderThread.IsBackground  = true;
            this.RefreshStartTime           = DateTime.UtcNow;
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
            Console.CursorVisible  = false;
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            while (this.Running)
            {
                Thread.Sleep(50);
                var elapsed = (DateTime.UtcNow - this.RefreshStartTime).TotalMilliseconds;
                if (this.CurrentStatus is null || elapsed > this.RefreshRate)
                {
                    this.CurrentStatus = this.Monitor.GetStatus();
                    this.RefreshStartTime = DateTime.UtcNow;
                }
                this.Render(this.CurrentStatus);
            }
        }
        
        public void SetInput(String input)
        {
            this.TextArea = input;
        }
        
        public void ClearInputBuffer()
        {
            this.TextArea = String.Empty;
        }
        
        public void ClearScreen()
        {
            Console.Clear();
        }
        
        public void Render(MonitorResult result)
        {
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"Process: {result.WindowTitle} ({result.Name}) ({result.ProcessId})");
            Console.WriteLine($"Processor %: {result.ProcessorUsagePercent}");
            Console.WriteLine($"Memory (MB): {result.MemoryMegabytesWorkingSet} ({result.MemoryBytesWorkingSet / 1024}K)");
            Console.WriteLine("Threads: ");
            var height = Console.WindowHeight - (4 + 4);
            foreach (var thread in result.Threads)
            {
                if (height == 0)
                {
                    break;
                }
                Console.WriteLine($" Thread ID {thread.ThreadId}: Process %: {thread.ProcessorUsagePercent}");
                height--;
            }
            Console.SetCursorPosition(0, Console.WindowHeight - 3);
            Console.Write(new String('-', Console.WindowWidth - 1));
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.Write(new String(' ', Console.WindowWidth - 1));
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.Write(this.TextArea);
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write(new String('-', Console.WindowWidth - 1));
        }
    }
}
