using System;

namespace Caesura.Arnald.Tests.Manual
{
    public static class Program
    {
        public static void Main(String[] args)
        {
            Test(args);
            Console.WriteLine();
            Console.WriteLine("Test finished. Press any key to continue...");
            Console.ReadLine();
        }
        
        public static void Test(String[] args)
        {
            var test1 = new Agents.Test1.AgentTest1();
            test1.Run();
        }
    }
}
