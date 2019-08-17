
using System;

namespace Caesura.Arnald.Tests.Manual
{
    using System.Threading;
    using System.IO;
    using System.Diagnostics;
    using System.Reflection;
    using System.ComponentModel;
    
    public static class Program
    {
        public static void Main(String[] args)
        {
            Console.Title = "Caesura.ARNaLD Manual Test Platform";
            
            Process monitor_process = null;
            var use_monitor = PromptMonitor(defaultAnswer: true);
            
            if (use_monitor)
            {
                monitor_process = StartMonitor();
                if (monitor_process is null)
                {
                    Console.ReadLine();
                    return;
                }
            }
            
            RunTest(args);
            
            if (use_monitor)
            {
                try
                {
                    if (!monitor_process.HasExited)
                    {
                        monitor_process.Kill();
                        monitor_process.WaitForExit(3000);
                        if (!monitor_process.HasExited)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Gave up waiting to kill monitor process.");
                            Console.ResetColor();
                        }
                    }
                }
                catch (Win32Exception)
                {
                    // Nothing, the process is exiting right now.
                }
                catch (InvalidOperationException)
                {
                    // Nothing, the process has already exited.
                }
            }
            
            Console.WriteLine();
            Console.WriteLine("Test finished. Press any key to continue...");
            Console.ReadLine();
        }
        
        public static void RunTest(String[] args)
        {
            // TODO: test categories followed by test numbers.
            // e.g.:
            // Run category:
            // [0] Agents
            // [1] Plugins
            // > 0
            // Run test:
            // [0] Agent creation
            // [1] Agent signal creation
            // [2] Agent signal receiving
            // etc....
            // Pressing enter (default) just goes to the last test.
        }
        
        public static Boolean PromptMonitor(Boolean defaultAnswer = true)
        {
            Console.Write("Start monitor? ");
            Console.Write(defaultAnswer ? "Y/n" : "y/N");
            Console.WriteLine();
            var response = String.Empty;
            while (true)
            {
                Console.Write(">");
                response = Console.ReadLine().ToLower();
                
                if (String.IsNullOrEmpty(response))
                {
                    return defaultAnswer;
                }
                
                if (response == "y"
                ||  response == "yes")
                {
                    return true;
                }
                
                if (response == "n"
                ||  response == "no")
                {
                    return false;
                }
                
                Console.WriteLine("Unrecognized response.");
            }
        }
        
        public static Process StartMonitor()
        {
            var pid         = Process.GetCurrentProcess().Id;
            var path        = Assembly.GetExecutingAssembly().CodeBase;
            var directory   = Path.GetDirectoryName(path).Replace("file:\\", "");
            var profpath    = "../../../../Tools/Debugging/Caesura.PerformanceMonitor/Caesura.PerformanceMonitor/bin/Debug/netcoreapp2.2";
            var proffile    = "Caesura.PerformanceMonitor.dll";
            var profargs    = $"--pid {pid}";
            var process     = new Process();
            var psi         = new ProcessStartInfo()
            {
                FileName                = "dotnet",
                Arguments               = $"{Path.Combine(directory, profpath, proffile)} {profargs}",
                WorkingDirectory        = Path.Combine(directory, profpath),
                UseShellExecute         = true,
                RedirectStandardOutput  = false,
                RedirectStandardError   = false,
                CreateNoWindow          = true,
            };
            process.StartInfo = psi;
            try
            {
                if (!Directory.Exists(Path.Combine(directory, profpath))
                ||  !File.Exists(Path.Combine(directory, profpath, proffile)))
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Could not find monitor assembly! Make sure to compile it first!");
                    Console.ResetColor();
                }
                else
                {
                    process.Start();
                    return process;
                }
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Could not start monitor process! {e}");
                Console.ResetColor();
            }
            return null;
        }
    }
}
