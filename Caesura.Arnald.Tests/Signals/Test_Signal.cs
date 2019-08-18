
using System;

namespace Caesura.Arnald.Tests.Signals
{
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;
    using Caesura.Arnald.Core.Signals;
    
    public class Test_Signal : BaseTest
    {
        public Test_Signal(ITestOutputHelper output) : base(output)
        {
            this.AddTest(nameof(DataAccess_1), this.DataAccess_1);
        }
        
        [Fact]
        public void DataAccess_1()
        {
            // Setup and constants
            ISignal s = new Signal();
            String key = "Key";
            Int32 value = 10;
            
            // Test proper
            s.Data.Set(key, value);
            var item = s.GetData<Int32>(key);
            
            // Assertions
            Assert.True(item.HasValue);
            Assert.Equal(item.Value, value);
        }
    }
}
