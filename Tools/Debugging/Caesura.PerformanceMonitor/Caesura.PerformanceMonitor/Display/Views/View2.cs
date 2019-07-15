
using System;

namespace Caesura.PerformanceMonitor.Display.Views
{
    using System.Collections.Generic;
    using System.Linq;
    using Display;
    using Monitor;
    
    public class View2
    {
        public void MainView(View self, MonitorResult result)
        {
            Console.Title = $"Caesura Performance Monitor";
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.SetCursorPosition(0, 0);
            
            // TODO: simple split-screen, one side has CPU/RAM info and the other
            // side has thread info. thread info should be scrollable with up/down keys
            // (need to change the keyboard handler to handle that)
            
            // --- TITLE BAR --- //
            var prebar = "--- ";
            Console.Write(prebar);
            var title = $"Monitoring \"{result.WindowTitle}\" ({result.Name})";
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(title);
            Console.ResetColor();
            Console.Write(" ");
            Console.WriteLine(new String('-', Console.WindowWidth - (prebar.Length + title.Length) - 2));
            
            for (var i = 0; i < (Console.WindowHeight - 5); i++)
            {
                Console.WriteLine($"Number {i}");
            }
            Console.WriteLine("End");
            
            // OLD
            /*
            Console.WriteLine($"Process: {result.WindowTitle} ({result.Name}) ({result.ProcessId}) Refresh Rate: {self.RefreshRate}");
            Console.WriteLine($"Processor: {result.ProcessorUsagePercent}");
            Console.WriteLine($"Memory: {result.MemoryMegabytesWorkingSet} MB ({result.MemoryBytesWorkingSet / 1024}K)");
            Console.WriteLine("Threads: ");
            var height = Console.WindowHeight - (4 + 4);
            foreach (var thread in result.Threads)
            {
                if (height == 0)
                {
                    break;
                }
                Console.WriteLine($" Thread ID {thread.ThreadId}: Process: {thread.ProcessorUsagePercent}");
                height--;
            } */
            Console.SetCursorPosition(0, Console.WindowHeight - 3);
            Console.Write(new String('-', Console.WindowWidth - 1));
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.Write(new String(' ', Console.WindowWidth - 1));
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.ForegroundColor = self.TextAreaColor;
            Console.Write(self.TextArea);
            Console.ResetColor();
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write(new String('-', Console.WindowWidth - 1));
        }
        
        public void HelpView(View self, MonitorResult result)
        {
            
        }
    }
}
