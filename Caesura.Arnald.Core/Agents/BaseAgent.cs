
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
        public Personality Personality { get; protected set; }
        protected Mailbox Messages { get; set; }
        
        public BaseAgent()
        {
            this.Identifier = Guid.NewGuid();
            this.Personality = new Personality();
        }
        
        public virtual IEnumerable<Task> Execute()
        {
            return this.Personality.Execute();
        }
        
        public virtual void Learn(IBehavior behavior)
        {
            this.Personality.Learn(behavior);
        }
        
        public virtual void Send(IMessage message)
        {
            throw new NotImplementedException();
        }
        
        public virtual void Send(IEnumerable<IMessage> messages)
        {
            throw new NotImplementedException();
        }
        
        public virtual void Dispose()
        {
            this.Personality?.Dispose();
        }
    }
}
