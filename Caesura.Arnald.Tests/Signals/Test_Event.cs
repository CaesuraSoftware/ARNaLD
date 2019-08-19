
using System;

namespace Caesura.Arnald.Tests.Signals
{
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;
    using Caesura.Arnald.Core.Signals;
    
    public class Test_Event : BaseTest
    {
        public Test_Event(ITestOutputHelper output) : base(output)
        {
            this.AddTest(nameof(SubscriptionTest_1) , this.SubscriptionTest_1);
            this.AddTest(nameof(RaiseTest_1)        , this.RaiseTest_1);
            this.AddTest(nameof(RaiseTest_2)        , this.RaiseTest_2);
            this.AddTest(nameof(BlockTest_1)        , this.BlockTest_1);
        }
        
        [Fact]
        public void SubscriptionTest_1()
        {
            // Setup and constants
            String name = "event0"; // no relation to the video game
            IEvent ev = new Event(name);
            
            Boolean activated1 = false;
            Boolean disposed1 = false;
            ActivatorCallback onactivate1 = (self, signal) =>
            {
                activated1 = true;
            };
            Action<IActivator> onunsub1 = (self) =>
            {
                disposed1 = true;
            };
            
            // Test proper
            IActivator sub1 = ev.Subscribe(onactivate1);
            sub1.OnUnsubscribe = onunsub1;
            sub1.Raise();
            sub1.Unsubscribe();
            
            // Assertions
            Assert.True(activated1);
            Assert.True(disposed1);
        }
        
        [Fact]
        public void RaiseTest_1()
        {
            // Setup and constants
            String name = "event0";
            IEvent ev = new Event(name);
            
            Boolean activated1 = false;
            ActivatorCallback onactivate1 = (self, signal) =>
            {
                activated1 = true;
            };
            Boolean activated2 = false;
            ActivatorCallback onactivate2 = (self, signal) =>
            {
                activated2 = true;
            };
            
            // Test proper
            IActivator sub1 = ev.Subscribe(onactivate1);
            IActivator sub2 = ev.Subscribe(onactivate2);
            sub1.Raise();
            
            // Assertions
            Assert.NotEqual(sub1.Name, sub2.Name);
            Assert.True(activated1);
            Assert.True(activated2);
        }
        
        [Fact]
        public void RaiseTest_2()
        {
            // Setup and constants
            String name = "event0";
            IEvent ev = new Event(name);
            
            String message_name = "message";
            Boolean message_content = true;
            IDataContainer data = new DataContainer();
            data.Set(message_name, message_content);
            
            Boolean activated1 = false;
            ActivatorCallback onactivate1 = (self, signal) =>
            {
                activated1 = true;
            };
            Boolean activated2 = false;
            ActivatorCallback onactivate2 = (self, signal) =>
            {
                signal.AssertData<Boolean>(message_name);
                
                activated2 = signal.GetData<Boolean>(message_name).Value;
            };
            
            // Test proper
            IActivator sub1 = ev.Subscribe(onactivate1);
            IActivator sub2 = ev.Subscribe(onactivate2);
            sub1.Raise(data);
            
            // Assertions
            Assert.NotEqual(sub1.Name, sub2.Name);
            Assert.True(activated1);
            Assert.True(activated2);
        }
        
        [Fact]
        public void BlockTest_1()
        {
            // Setup and constants
            String name = "event0";
            IEvent ev = new Event(name);
            
            Boolean activated1 = false;
            ActivatorCallback onactivate1 = (self, signal) =>
            {
                activated1 = true;
            };
            Boolean activated2 = false;
            ActivatorCallback onactivate2 = (self, signal) =>
            {
                activated2 = true;
            };
            
            // Test proper
            IActivator sub1 = ev.Subscribe(onactivate1);
            IActivator sub2 = ev.Subscribe(onactivate2);
            sub1.Block();
            sub1.Raise();
            
            // Assertions
            Assert.NotEqual(sub1.Name, sub2.Name);
            Assert.True(activated1);
            Assert.False(activated2);
        }
    }
}
