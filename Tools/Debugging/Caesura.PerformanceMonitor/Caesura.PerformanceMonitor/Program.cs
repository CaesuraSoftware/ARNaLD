﻿
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
            Int32 UpdateInterval = 1000;
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
                Console.WriteLine(" [-u|--update] [Int32] - Update interval in milliseconds. Default is 1000.");
                Console.WriteLine(" [-s|--shutdown] [Boolean] - Automatically shut down after the process to be monitored ends. Default is true.");
            }
            else if (nargs.Count == 1)
            {
                var success = Int32.TryParse(nargs[0], out var pid);
                if (success)
                {
                    ProcessId = pid;
                }
                else
                {
                    Console.WriteLine("Bad argument. Process ID argument needs a process ID. Try '--help'.");
                    return;
                }
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
            
            sleepInterval = UpdateInterval;
            monitor = new Monitor.Windows(ProcessId);
            var t1 = new System.Threading.Thread(() => RunMe()) { IsBackground = true };
            t1.Start();
            while (true)
            {
                var input = Console.ReadLine();
                running = false;
                break;
            }
            
            Console.ReadLine();
        }
        
        static Boolean running = true;
        static Monitor.Windows monitor;
        static Int32 sleepInterval;
        static void RunMe()
        {
            while (running)
            {
                System.Threading.Thread.Sleep(sleepInterval);
                Console.SetCursorPosition(0, 0);
                var result = monitor.GetStatus();
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
            }
        }
    }
}