
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    
    /// <summary>
    /// Internals of an Agent: The agent's state machine is activated every
    /// time the agent gets a message, in which that message is passed to
    /// the current state. That state then should call a Behavior from the
    /// agent's Personality in order to act out tasks.
    /// </summary>
    public abstract class BaseAgent : IAgent
    {
        public String Name { get; protected set; }
        public Guid Identifier { get; protected set; }
        public IPersonality Personality { get; protected set; }
        public ThreadState AgentThreadState { get; protected set; }
        protected Boolean AgentRunning { get; set; }
        protected CancellationTokenSource CancelToken { get; set; }
        protected IMailbox Messages { get; set; }
        protected IState AgentState { get; set; }
        
        public BaseAgent()
        {
            this.Identifier         = Guid.NewGuid();
            this.Name               = this.Identifier.ToString("N").ToUpper();
            this.AgentThreadState   = ThreadState.Unstarted;
            this.AgentRunning       = false;
            this.Personality        = new Personality();
            this.Messages           = new Mailbox();
            this.AgentState         = State.LoadDefaults(this);
        }
        
        public virtual void Start()
        {
            if (this.AgentRunning)
            {
                throw new InvalidOperationException("Agent is already running.");
            }
            this.AgentRunning = true;
            this.AgentThreadState = ThreadState.Running;
            this.CancelToken = new CancellationTokenSource();
            // TODO: figure out a good way to handle a long-running agent, either make
            // a dedicated thread, make a long-running task or simply make tasks in a
            // loop while waiting for each one to finish (probably a bad idea).
            // At the end of the central loop, set AgentRunning to false.
            throw new NotImplementedException();
        }
        
        public virtual void Stop()
        {
            this.AgentThreadState = ThreadState.StopRequested;
            this.AgentState.TrySetState(StateAtomState.Cancelled);
            this.CancelToken.Cancel(false);
        }
        
        public virtual void Wait()
        {
            while (this.AgentRunning)
            {
                Thread.Sleep(50); // 50 based on experiences from other developers
            }
        }
        
        public virtual void Execute()
        {
            var msg = this.Messages.Receive();
            if (msg)
            {
                this.AgentState.Next(msg.Value);
            }
        }
        
        public virtual void Learn(IBehavior behavior)
        {
            this.Personality.Learn(behavior);
        }
        
        public virtual void Send(IMessage message)
        {
            this.Messages.Send(message);
        }
        
        public virtual void Send(IEnumerable<IMessage> messages)
        {
            this.Messages.Send(messages);
        }
        
        public virtual void Dispose()
        {
            this.AgentState?.Dispose();
            this.Personality?.Dispose();
            this.Stop();
            this.AgentThreadState = ThreadState.WaitSleepJoin;
            this.Wait();
        }
    }
}
