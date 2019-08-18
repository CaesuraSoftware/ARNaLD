
using System;

namespace Caesura.Arnald.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;
    using Caesura.Arnald.Core.Signals;
    
    public class Test_Template : BaseTest
    {
        public Test_Template(ITestOutputHelper output) : base(output)
        {
            this.AddTest(nameof(SampleTest_1), this.SampleTest_1);
        }
        
        [Fact]
        public void SampleTest_1()
        {
            // Setup and constants
            Boolean a = false;
            
            // Test proper
            a = true;
            
            // Assertions
            Assert.True(a);
        }
    }
}
