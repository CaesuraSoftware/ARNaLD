
using System;

namespace Caesura.PerformanceMonitor.Display.Views
{
    using System.Collections.Generic;
    using System.Linq;
    using Display;
    using Monitor;
    
    public class View2
    {
        private Int32 b_ThreadPage;
        public Int32 ThreadPage { get => this.b_ThreadPage; set => this.b_ThreadPage = value < 1 ? 1 : value; }
        public Int32 MaxThreadsPerPage { get; set; }
        private MonitorResult LastResult { get; set; }
        
        public View2()
        {
            this.b_ThreadPage = 1;
            this.MaxThreadsPerPage = 30;
        }
        
        public void MainView(View self, MonitorResult result)
        {
            // --- INIT SCREEN --- //
            
            if (this.LastResult is null)
            {
                this.LastResult = result;
            }
            Console.Title = $"Caesura Performance Monitor";
            if (Console.WindowWidth >= Console.BufferWidth && Console.WindowHeight >= Console.BufferHeight)
            {
                Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            }
            Console.CursorVisible  = false;
            Console.SetCursorPosition(0, 0);
            
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
            
            // --- SPLIT SCREEN CPU+RAM / THREADS --- //
            
            var threadPos = this.MaxThreadsPerPage * (this.ThreadPage - 1);
            for (var i = 0; i < (Console.WindowHeight - 4); i++)
            {
                // CPU + RAM VIEW:
                var indicator = String.Empty;
                switch (i)
                {
                    case 0:
                        {
                            indicator = $"Process ID: {result.ProcessId}";
                        } break;
                    case 1:
                        {
                            indicator = $"Threads: {result.ThreadCount}";
                            /**/ if (result.ThreadCount > this.LastResult.ThreadCount)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                            }
                            else if (result.ThreadCount < this.LastResult.ThreadCount)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGreen;
                            }
                        } break;
                    case 2:
                        {
                            indicator = $"CPU: {result.ProcessorUsagePercent} %";
                            /**/ if (Math.Abs(result.ProcessorUsage - this.LastResult.ProcessorUsage) < 0.001)
                            {
                                // Equal, do nothing.
                            }
                            else if (result.ProcessorUsage > this.LastResult.ProcessorUsage)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                            }
                            else if (result.ProcessorUsage < this.LastResult.ProcessorUsage)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGreen;
                            }
                        } break;
                    case 3:
                        {
                            indicator = $"RAM: {(result.MemoryBytesWorkingSet / 1024).ToString("N0")} K ({result.MemoryMegabytesWorkingSet.ToString("N0")} MB)";
                            /**/ if (result.MemoryBytesWorkingSet  > this.LastResult.MemoryBytesWorkingSet)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                            }
                            else if (result.MemoryBytesWorkingSet  < this.LastResult.MemoryBytesWorkingSet)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGreen;
                            }
                        } break;
                    case 5:
                        {
                            indicator = new String('-', (((Console.WindowWidth - 5) / 2)));
                        } break;
                    default:
                        indicator = String.Empty;
                        break;
                }
                indicator = " " + indicator;
                Console.Write(indicator);
                var size = ((Console.WindowWidth - 4) / 2) - indicator.Length;
                if (size >= 0)
                {
                    Console.Write(new String(' ', size));
                }
                Console.ResetColor();
                Console.Write("| ");
                
                // THREAD VIEW:
                if (i == 0)
                {
                    var pagenum = (result.ThreadCount / this.MaxThreadsPerPage) + 1;
                    Console.Write($"Page {this.ThreadPage} of {pagenum} (▲/◄ = Back ▼/► = Forward)");
                }
                else
                {
                    for (var j = 0; j < 2; j++)
                    {
                        if (threadPos < result.Threads.Count)
                        {
                            var t = result.Threads.ElementAt(threadPos);
                            var ti = $"Thread {t.ThreadId.ToString().PadRight(6)} % {t.ProcessorUsagePercent.PadRight(6)}";
                            if (!this.LastResult.Threads.Exists(x => x.ThreadId == t.ThreadId))
                            {
                                Console.BackgroundColor = ConsoleColor.DarkCyan;
                            }
                            else
                            {
                                var nt = this.LastResult.Threads.Find(x => x.ThreadId == t.ThreadId);
                                /**/ if (Math.Abs(t.ProcessorUsage - nt.ProcessorUsage) < 0.001)
                                {
                                    // Equal, do nothing.
                                }
                                else if (t.ProcessorUsage > nt.ProcessorUsage)
                                {
                                    Console.BackgroundColor = ConsoleColor.DarkRed;
                                }
                                else if (t.ProcessorUsage < nt.ProcessorUsage)
                                {
                                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                                }
                            }
                            Console.Write(ti);
                            Console.ResetColor();
                            Console.Write(new String(' ', 3));
                            threadPos++;
                        }
                        else
                        {
                            Console.Write(new String(' ', 26));
                        }
                    }
                }
                Console.WriteLine();
            }
            
            // --- COMMAND INPUT/OUTPUT FIELD --- //
            
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
            
            this.LastResult = result;
        }
        
        public void HelpView(View self, MonitorResult result)
        {
            
        }
    }
}
