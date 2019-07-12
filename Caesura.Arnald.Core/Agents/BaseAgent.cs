
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
        protected IMailbox Messages { get; set; }
        protected IState AgentState { get; set; }
        
        public BaseAgent()
        {
            this.Identifier         = Guid.NewGuid();
            this.Name               = this.Identifier.ToString("N").ToUpper();
            this.AgentThreadState   = ThreadState.Unstarted;
            this.Personality        = new Personality();
            this.Messages           = new Mailbox();
            this.AgentState         = State.LoadDefaults(this);
        }
        
        public virtual void Start()
        {
            this.AgentThreadState = ThreadState.Running;
            // TODO: create a long-running task here, store the cancellationtoken in this class
            // call Execute in a loop
            throw new NotImplementedException();
        }
        
        public virtual void Stop()
        {
            this.AgentThreadState = ThreadState.StopRequested;
            // TODO: call the cancellationtoken's cancel here
            throw new NotImplementedException();
        }
        
        public virtual void StopAndWait()
        {
            this.Stop();
            this.AgentThreadState = ThreadState.WaitSleepJoin;
            throw new NotImplementedException();
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
            this.StopAndWait();
        }
    }
}
