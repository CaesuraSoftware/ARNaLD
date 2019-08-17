
using System;

namespace Caesura.Arnald.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;
    
    public abstract class BaseTest
    {
        private readonly ITestOutputHelper output;
        public String TestName { get; protected set; }
        public Dictionary<String, Action> Tests { get; set; }
        
        public BaseTest(ITestOutputHelper output)
        {
            this.output = output;
            this.Tests = new Dictionary<String, Action>();
            this.TestName = this.GetType().Name;
            this.PreTest();
            this.WriteLine($"Running test class {this.TestName}...");
        }
        
        protected virtual void PreTest()
        {
            // Do nothing
        }
        
        public void AddTest(String name, Action a)
        {
            this.Tests.Add(name, a);
        }
        
        public void WriteLine(String str, params Object[] args)
        {
            this.output.WriteLine(str, args);
            Console.WriteLine(str, args);
        }
        
        public void WriteLine()
        {
            this.WriteLine(String.Empty);
        }
    }
}
