
using System;

namespace Caesura.Arnald.Tests.Manual
{
    using System.IO;
    using System.Diagnostics;
    
    public static class Program
    {
        public static void Main(String[] args)
        {
            var p = StartMonitor();
            Test(args);
            p.Kill();
            // Console.WriteLine();
            // Console.WriteLine("Test finished. Press any key to continue...");
            // Console.ReadLine();
        }
        
        public static void Test(String[] args)
        {
            var test1 = new Agents.Test1.AgentTest1();
            test1.Run();
        }
        
        public static Process StartMonitor()
        {
            
            var pid         = Process.GetCurrentProcess().Id;
            var path        = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            var directory   = Path.GetDirectoryName(path).Replace("file:\\", "");
            var profpath    = "../../../../Tools/Debugging/Caesura.PerformanceMonitor/Caesura.PerformanceMonitor/bin/Debug/netcoreapp2.2";
            var proffile    = "Caesura.PerformanceMonitor.dll";
            var profargs    = $" --pid {pid}";
            var process     = new Process();
            var si          = new ProcessStartInfo()
            {
                FileName                = "dotnet",
                Arguments               = $"{Path.Combine(directory, profpath, proffile)}{profargs}",
                WorkingDirectory        = Path.Combine(directory, profpath),
                UseShellExecute         = true,
                RedirectStandardOutput  = false,
                RedirectStandardError   = false,
                CreateNoWindow          = true,
            };
            process.StartInfo = si;
            process.Start();
            return process;
        }
    }
}
