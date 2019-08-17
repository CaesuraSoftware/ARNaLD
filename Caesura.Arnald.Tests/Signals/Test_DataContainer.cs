
using System;

namespace Caesura.Arnald.Tests.Signals
{
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;
    using Caesura.Arnald.Core.Signals;
    
    /// <summary>
    /// This test case is for the Caesura.Arnald.Core.Signals.DataContainer class.
    /// </summary>
    public class Test_DataContainer : BaseTest
    {
        public Test_DataContainer(ITestOutputHelper output) : base(output)
        {
            this.AddTest(nameof(AddToDataContainer_1), this.AddToDataContainer_1);
            this.AddTest(nameof(AddToDataContainer_2), this.AddToDataContainer_2);
        }
        
        /// <summary>
        /// Test that elements can be added to and retrieved from a DataContainer.
        /// </summary>
        [Fact]
        public void AddToDataContainer_1()
        {
            // Setup and constants
            IDataContainer dc = new DataContainer();
            String key = "Key";
            Int32 value = 10;
            
            // Test proper
            dc.Set(key, value);
            var item = dc.Get<Int32>(key);
            
            // Assertions
            Assert.True(item.HasValue);
            Assert.Equal(item.Value, value);
        }
        
        /// <summary>
        /// Test that elements cannot be retrieved from the wrong type cast.
        /// </summary>
        [Fact]
        public void AddToDataContainer_2()
        {
            // Setup and constants
            IDataContainer dc = new DataContainer();
            String key = "Key";
            Int32 value = 10;
            
            // Test proper
            dc.Set(key, value);
            var item = dc.Get<String>(key);
            
            // Assertions
            Assert.False(item.HasValue);
        }
    }
}
