
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    
    public delegate MessageResolverResult CheckCallback(MessageResolver resolver, IMessage message);
    
    public delegate void ExecuteCallback(MessageResolver resolver);
    
    public class MessageResolver : IMessageResolver
    {
        public String Name { get; set; }
        public IMessageHandler Owner { get; set; }
        public IMessage Current { get; set; }
        public CheckCallback CheckCallback { get; set; }
        public ExecuteCallback ExecuteCallback { get; set; }
        
        public MessageResolver()
        {
            
        }
        
        public MessageResolver(IMessageHandler owner, String name, CheckCallback checker, ExecuteCallback execute)
        {
            this.Owner              = owner;
            this.Name               = name;
            this.CheckCallback      = checker;
            this.ExecuteCallback    = execute;
        }
        
        public MessageResolverResult Check(IMessage message)
        {
            this.Current = message;
            if (this.CheckCallback is null)
            {
                return MessageResolverResult.None;
            }
            return this.CheckCallback.Invoke(this, message);
        }
        
        public void Execute()
        {
            this.ExecuteCallback?.Invoke(this);
        }
    }
}
