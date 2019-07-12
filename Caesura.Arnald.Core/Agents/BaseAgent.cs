
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
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
        
        public virtual IEnumerable<Task<IMessage>> Execute()
        {
            var msgs = this.Messages.ReceiveAll();
            return this.Personality.Execute(msgs);
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
