
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
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
        protected IMailbox Messages { get; set; }
        protected IState AgentState { get; set; }
        
        public BaseAgent()
        {
            this.Identifier     = Guid.NewGuid();
            this.Name           = this.Identifier.ToString("N").ToUpper();
            this.Personality    = new Personality();
            this.Messages       = new Mailbox();
            this.AgentState     = State.LoadDefaults(this);
        }
        
        public virtual void Execute()
        {
            var msg = this.Messages.Receive();
            if (msg.HasValue)
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
        }
    }
}
