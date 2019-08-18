
using System;

namespace Caesura.Arnald.Tests.Signals
{
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;
    using Caesura.Arnald.Core.Signals;
    
    public class Test_Activator : BaseTest
    {
        public Test_Activator(ITestOutputHelper output) : base(output)
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
