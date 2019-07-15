
using System;

namespace Caesura.PerformanceMonitor.Display.Views
{
    using System.Collections.Generic;
    using System.Linq;
    using Display;
    using Monitor;
    
    public static class View1
    {
        public static void Run(View self, MonitorResult result)
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
            Console.Write(self.TextArea);
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write(new String('-', Console.WindowWidth - 1));
        }
    }
}
