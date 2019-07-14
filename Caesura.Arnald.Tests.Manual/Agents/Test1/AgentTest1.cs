
using System;

namespace Caesura.Arnald.Tests.Manual.Agents.Test1
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Caesura.Arnald.Core;
    using Caesura.Arnald.Core.Agents;
    using Caesura.Standard;
    
    /// <summary>
    /// Test 1: A simple console-based app where one agent listens for console
    /// input and sends a message to another agent who sends all messages
    /// addressed to it to the console output.
    /// </summary>
    public class AgentTest1
    {
        public void Run()
        {
            Console.WriteLine("Initializing Test 1...");
            var locator = new Locator();
            var config = AgentConfiguration.Default;
            var input = new ConsoleInput(config);
            var output = new ConsoleOutput(config);
            locator.Add(input);
            locator.Add(output);
            locator.Run();
        }
    }
    
    public class ConsoleInput : BaseAgent
    {
        private Thread ConsoleInputThread { get; set; }
        private Boolean ConsoleInputThreadRunning { get; set; }
        
        public ConsoleInput() : base()
        {
            
        }
        
        public ConsoleInput(IAgentConfiguration config) : base(config)
        {
            
        }
        
        public override void Setup(IAgentConfiguration config)
        {
            base.Setup(config);
            
            this.ConsoleInputThreadRunning = false;
            this.ConsoleInputThread = new Thread(this.HandleConsoleInput);
            this.ConsoleInputThread.IsBackground = true;
            
            this.AgentState.Add("ShowCursor", (atom, message) => "HideCursor");
            this.AgentState.Add("HideCursor", (atom, message) => "ShowCursor");
            this.AgentState.TrySetInitialState("ShowCursor");
            this.AgentState.TrySetState("ShowCursor");
            
            this.Resolver.AddResolver(
                new MessageResolver((resolver, message) =>
                {
                    if (message.Information == "ShowCursor")
                    {
                        this.AgentState.TrySetState("ShowCursor");
                    }
                })
            );
        }
        
        public override void Start()
        {
            if (!this.ConsoleInputThreadRunning)
            {
                try
                {
                    this.ConsoleInputThread.Start();
                }
                catch (ThreadStartException)
                {
                    // no-op
                }
            }
            base.Start();
        }
        
        private void HandleConsoleInput()
        {
            this.ConsoleInputThreadRunning = true;
            Console.WriteLine("Test 1: Agent Console Handler. Type 'quit' to end the session.");
            while (!this.CancelToken.IsCancellationRequested)
            {
                if (this.AgentState.Current.Name == "HideCursor")
                {
                    Thread.Sleep(50);
                    continue;
                }
                
                Console.Write("> ");
                var input = Console.ReadLine();
                var msg = new Message()
                {
                    Sender = this.Name,
                    Recipient = nameof(ConsoleOutput),
                    Information = input,
                };
                this.AgentState.Next();
                this.HostLocator.Send(msg);
            }
            this.ConsoleInputThreadRunning = false;
        }
    }
    
    public class ConsoleOutput : BaseAgent
    {
        public ConsoleOutput() : base()
        {
            
        }
        
        public ConsoleOutput(IAgentConfiguration config) : base(config)
        {
            
        }
        
        public override void Setup(IAgentConfiguration config)
        {
            base.Setup(config);
            
            this.Resolver.AddResolver(
                new MessageResolver((resolver, message) =>
                {
                    Console.WriteLine($"I got a message! {message.Information}");
                    
                    if (String.Equals(message.Information, "quit", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("bai bai!");
                        this.HostLocator.Stop();
                        return;
                    }
                    
                    var response = message.SwapSender();
                    response.Information = "ShowCursor";
                    this.HostLocator.Send(response);
                })
            );
        }
    }
}
