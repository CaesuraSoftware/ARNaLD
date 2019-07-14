
using System;

namespace Caesura.PerformanceMonitor
{
    using System.Collections.Generic;
    using System.Linq;
    
    class Program
    {
        // TODO:
        //  - seperate monitoring and displaying info
        //  - make a nice console display, not just printing in a loop
        //  - check args for: 
        //    - processid (default: itself, for debugging purposes)
        //    - update interval (default: 1000ms)
        //    - shutdown after process ends (default: true)
        //  - put a simple static launcher method for this program
        //    in Caesura.Standard.
        //  - Linux version probing /proc/{pid}
        static void Main(String[] args)
        {
            Int32 ProcessId      = 0;
            Int32 UpdateInterval = 500;
            Boolean AutoShutdown = true;
            
            var nargs = new List<String>(args);
            if ((nargs.Count == 1)
            && (String.Equals(nargs.First(), "-h"    , StringComparison.OrdinalIgnoreCase)
            ||  String.Equals(nargs.First(), "--help", StringComparison.OrdinalIgnoreCase) 
            ||  String.Equals(nargs.First(), "/?"    , StringComparison.OrdinalIgnoreCase) 
            ||  String.Equals(nargs.First(), "/h"    , StringComparison.OrdinalIgnoreCase)
            ||  String.Equals(nargs.First(), "/help" , StringComparison.OrdinalIgnoreCase)
            ))
            {
                Console.WriteLine("Caesura Performance Monitor");
                Console.WriteLine();
                Console.WriteLine("Usage: [Caesura.PerformanceMonitor] [-p|--pid] Int32 [-u|--update] [Int32] [-s|--shutdown] [Boolean]");
                Console.WriteLine();
                Console.WriteLine(" [-p|--pid] Int32 - Process ID for the process to monitor the resources of.");
                Console.WriteLine(" [-u|--update] [Int32] - Update interval in milliseconds. Default is 500.");
                Console.WriteLine(" [-s|--shutdown] [Boolean] - Automatically shut down after the process to be monitored ends. Default is true.");
            }
            else
            {
                var argsHandled = 0;
                
                // --- Process ID argument filter --- //
                var pidArg = nargs.Find(x => 
                   String.Equals(x, "-p"   , StringComparison.OrdinalIgnoreCase)
                || String.Equals(x, "--pid", StringComparison.OrdinalIgnoreCase)
                || String.Equals(x, "/p"   , StringComparison.OrdinalIgnoreCase)
                || String.Equals(x, "/pid" , StringComparison.OrdinalIgnoreCase)
                );
                if (pidArg != null)
                {
                    var pos = nargs.IndexOf(pidArg);
                    if (nargs.Count <= pos + 1)
                    {
                        Console.WriteLine("Bad argument. Process ID argument needs a process ID. Try '--help'.");
                        return;
                    }
                    else
                    {
                        var success = Int32.TryParse(nargs[pos + 1], out var pid);
                        if (success)
                        {
                            ProcessId = pid;
                            argsHandled += 2;
                        }
                        else
                        {
                            Console.WriteLine("Bad argument. Process ID argument needs a process ID. Try '--help'.");
                            return;
                        }
                    }
                }
                else
                {
                    var myPid = System.Diagnostics.Process.GetCurrentProcess().Id;
                    ProcessId = myPid;
                }
                
                // --- Update Interval argument filter. --- //
                var updateArg = nargs.Find(x => 
                   String.Equals(x, "-u"      , StringComparison.OrdinalIgnoreCase)
                || String.Equals(x, "--update", StringComparison.OrdinalIgnoreCase)
                || String.Equals(x, "/u"      , StringComparison.OrdinalIgnoreCase)
                || String.Equals(x, "/update" , StringComparison.OrdinalIgnoreCase)
                );
                if (updateArg != null)
                {
                    var pos = nargs.IndexOf(updateArg);
                    if (nargs.Count <= pos + 1)
                    {
                        Console.WriteLine("Bad argument. Update interval needs an integer. Try '--help'.");
                        return;
                    }
                    else
                    {
                        var success = Int32.TryParse(nargs[pos + 1], out var interval);
                        if (success && interval >= 0)
                        {
                            UpdateInterval = interval;
                            argsHandled += 2;
                        }
                        else
                        {
                            Console.WriteLine("Bad argument. Update interval needs an integer. Try '--help'.");
                            return;
                        }
                    }
                }
                else
                {
                    // no-op
                }
                
                // --- Automatic Shutdown argument filter. --- //
                var shutdownArg = nargs.Find(x => 
                   String.Equals(x, "-s"        , StringComparison.OrdinalIgnoreCase)
                || String.Equals(x, "--shutdown", StringComparison.OrdinalIgnoreCase)
                || String.Equals(x, "/s"        , StringComparison.OrdinalIgnoreCase)
                || String.Equals(x, "/shutdown" , StringComparison.OrdinalIgnoreCase)
                );
                if (shutdownArg != null)
                {
                    var pos = nargs.IndexOf(shutdownArg);
                    if (nargs.Count <= pos + 1)
                    {
                        Console.WriteLine("Bad argument. Auto-shutdown argument needs to be a boolean [True|False]. Try '--help'.");
                        return;
                    }
                    else
                    {
                        var success = Boolean.TryParse(nargs[pos + 1], out var autoshutdown);
                        if (success)
                        {
                            AutoShutdown = autoshutdown;
                            argsHandled += 2;
                        }
                        else
                        {
                            Console.WriteLine("Bad argument. Auto-shutdown argument needs to be a boolean [True|False]. Try '--help'.");
                            return;
                        }
                    }
                }
                else
                {
                    // no-op
                }
                
                if (nargs.Count > argsHandled)
                {
                    Console.WriteLine("Unrecognized argument(s). Try '--help'.");
                    return;
                }
            }
            
            // TODO:
            // call a viewer for the monitor here
            // TEST CODE, DELETE LATER
            
            var t1 = new System.Threading.Thread(() => RunMe(1)) { IsBackground = true };
            var t2 = new System.Threading.Thread(() => RunMe(2)) { IsBackground = true };
            var t3 = new System.Threading.Thread(() => RunMe(3)) { IsBackground = true };
            
            t1.Start();
            System.Threading.Thread.Sleep(300);
            t2.Start();
            System.Threading.Thread.Sleep(300);
            t3.Start();
            
            var mon = new Monitor.Windows();
            while (true)
            {
                Console.Write("> ");
                var input = Console.ReadLine();
                if (String.Equals(input, "quit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
                
                var result = mon.GetStatus();
                Console.WriteLine($"Process %: {result.ProcessorUsagePercent}");
                Console.WriteLine($"Memory (MB): {result.MemoryMegabytesUsed}");
                Console.WriteLine("Threads: ");
                foreach (var thread in result.Threads)
                {
                    Console.WriteLine($" Thread ID {thread.ThreadId}: Process %: {thread.ProcessorUsagePercent}. Priority {thread.CurrentPriority}.");
                }
                Console.WriteLine();
            }
            
            Console.ReadLine();
        }
        
        static void RunMe(Int32 number)
        {
            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine(number);
            }
        }
    }
}
