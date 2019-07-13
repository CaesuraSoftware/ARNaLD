
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
        public Boolean CheckIfRecipientIsHostName { get; set; }
        public State ResolverState { get; set; }
        public CheckCallback CheckCallback { get; set; }
        public ExecuteCallback ExecuteCallback { get; set; }
        
        public MessageResolver()
        {
            var name                            = this.HostHandler?.HostAgent?.Name;
            this.Name                           = name is null ? null : name + "Resolver";
            this.CheckIfRecipientIsHostName     = true;
            this.ResolverState                  = new State();
            this.ExecuteCallback                = (resolver) => this.ResolverState.Next(this.Current);
        }
        
        public MessageResolver(String name) : this()
        {
            this.Name = name;
        }
        
        public MessageResolver(String name, CheckCallback checker) : this(name)
        {
            this.CheckCallback = checker;
        }
        
        public MessageResolver(String name, CheckCallback checker, ExecuteCallback execute) : this(name, checker)
        {
            this.ExecuteCallback = execute;
        }
        
        public MessageResolverResult Check(IMessage message)
        {
            this.Current = message;
            if (this.CheckIfRecipientIsHostName)
            {
                if (message.Recipient != this.HostHandler.HostAgent.Name)
                {
                    return MessageResolverResult.Pass;
                }
            }
            if (this.CheckCallback is null)
            {
                return MessageResolverResult.Pass;
            }
            return this.CheckCallback.Invoke(this, message);
        }
        
        public void Execute()
        {
            this.ExecuteCallback?.Invoke(this);
        }
    }
}
