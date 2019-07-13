
using System;

namespace Caesura.Arnald.Core.Agents
{
    
    public delegate MessageResolverResult CheckCallback(MessageResolver resolver, IMessage message);
    
    public delegate void ExecuteCallback(MessageResolver resolver);
    
    public class MessageResolver : IMessageResolver
    {
        public String Name { get; set; }
        public IMessageHandler HostHandler { get; set; }
        public IMessage Current { get; set; }
        public State ResolverState { get; set; }
        public CheckCallback CheckCallback { get; set; }
        public ExecuteCallback ExecuteCallback { get; set; }
        
        public MessageResolver()
        {
            this.ResolverState      = new State();
            this.ExecuteCallback    = (resolver) => this.ResolverState.Next(this.Current);
        }
        
        public MessageResolver(String name, CheckCallback checker) : this()
        {
            this.Name               = name;
            this.CheckCallback      = checker;
        }
        
        public MessageResolver(String name, CheckCallback checker, ExecuteCallback execute) : this(name, checker)
        {
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
