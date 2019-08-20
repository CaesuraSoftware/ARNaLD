
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
            this.AddTest(nameof(ShowAdDemo)         , this.ShowAdDemo);
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
            sub1.SelfActivate = true;
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
            sub1.SelfActivate = true;
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
            sub1.SelfActivate = true;
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
                activated1 = self.Blocking;
            };
            Boolean activated2 = false;
            ActivatorCallback onactivate2 = (self, signal) =>
            {
                activated2 = true;
            };
            
            // Test proper
            IActivator sub1 = ev.Subscribe(onactivate1);
            IActivator sub2 = ev.Subscribe(onactivate2);
            sub1.SelfActivate = false; // sub1 blocking should always make sub1 get the signal.
            sub1.Block();
            sub1.Raise();
            
            // Assertions
            Assert.NotEqual(sub1.Name, sub2.Name);
            Assert.True(activated1);
            Assert.False(activated2);
        }
        
        [Fact]
        public void ShowAdDemo()
        {
            // Inspired by the Luca Matteis "B-Threads" talks.
            // This function is split up as three "iterations" to represent
            // incremental versions of a program. In this case, we're simulating an ATM.
            // In the first version of the program, we log the user in and show them
            // their account information. In the second version, we show an ad before
            // showing the account details. In the third version, we don't show an ad
            // for people with premium accounts. We do all this without changing the old
            // program in any way, only adding on to it.
            
            // Setup
            IEventScope scope = new EventScope("ATM");
            
            // --- Iteration 1 --- //
            this.WriteLine("--- Iteration 1 (log in and show account) ---");
            
            var eventName_login = "login";
            var eventName_showAccount = "showAccount";
            scope.Register(eventName_login);
            scope.Register(eventName_showAccount);
            
            scope.Subscribe(eventName_login, (self, signal) =>
            {
                this.WriteLine("Logging in...");
            });
            scope.Subscribe(eventName_showAccount, (self, signal) =>
            {
                this.WriteLine("Showing account! The end!");
            });
            
            scope.EventStack.SetStack(new String[]
            {
                eventName_login,
                eventName_showAccount,
            });
            
            // Start iteration 1
            scope.Run(false);
            
            // --- Iteration 2 --- //
            this.WriteLine("--- Iteration 2 (show an ad) ---");
            
            var eventName_showAd = "showAd";
            scope.Register(eventName_showAd);
            
            scope.Subscribe(eventName_showAd, (self, signal) =>
            {
                this.WriteLine("Showing ad!");
            });
            
            scope.EventStack.Insert(eventName_showAd, eventName_showAccount);
            
            // Start iteration 2
            scope.Run(false);
            
            // --- Iteration 3 --- //
            this.WriteLine("--- Iteration 3 (don't show ads for premium user) ---");
            
            Boolean premiumUser = true;
            
            scope.Intercept(eventName_showAd, (self, signal) =>
            {
                if (premiumUser)
                {
                    this.WriteLine("Premium user! No ads here.");
                }
                else
                {
                    self.Unblock();
                }
            });
            
            scope.Run(false);
            
            this.WriteLine("--- Iteration 3.1 (premium user again) ---");
            
            premiumUser = true;
            scope.Run(false);
            
            this.WriteLine("--- Iteration 3.2 (not premium user) ---");
            
            premiumUser = false;
            scope.Run(false);
            
            this.WriteLine("--- Iteration 3.3 (ditto) ---");
            
            premiumUser = false;
            scope.Run(false);
            
            this.WriteLine("--- Iteration 3.4 (premium user) ---");
            
            premiumUser = true;
            scope.Run(false);
            
            // --- Iteration 4 --- //
            this.WriteLine("--- Iteration 4 (security checking) ---");
            
            var eventName_securityCheck = "securityCheck";
            scope.Register(eventName_securityCheck);
            
            scope.Subscribe(eventName_securityCheck, (self, signal) =>
            {
                this.WriteLine("Extra security checking!");
            });
            
            scope.EventStack.Insert(eventName_securityCheck, eventName_showAd);
            
            // Start iteration 4
            scope.Run(false);
        }
    }
}
