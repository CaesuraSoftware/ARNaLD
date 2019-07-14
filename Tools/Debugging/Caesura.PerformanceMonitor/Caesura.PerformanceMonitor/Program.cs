using System;

namespace Caesura.PerformanceMonitor
{
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
            Console.WriteLine("Hello World!");
        }
    }
}
