
using System;

namespace Caesura.Arnald.Core.Agents
{
    
    public delegate MessageResolverResult CheckCallback(MessageResolver resolver, IMessage message);
    
    public delegate void ExecuteCallback(MessageResolver resolver, IMessage message);
    
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
            this.CheckIfRecipientIsHostName     = true;
            this.ResolverState                  = new State();
            this.CheckCallback                  = (resolver, message) => MessageResolverResult.Continue;
            this.ExecuteCallback                = (resolver, message) => this.ResolverState.Next(message);
        }
        
        public MessageResolver(String name) : this()
        {
            this.Name = name;
        }
        
        public MessageResolver(CheckCallback checker) : this()
        {
            this.CheckCallback = checker;
        }
        
        public MessageResolver(ExecuteCallback execute) : this()
        {
            this.ExecuteCallback = execute;
        }
        
        public MessageResolver(String name, CheckCallback checker) : this(checker)
        {
            this.Name = name;
        }
        
        public MessageResolver(String name, ExecuteCallback execute) : this(execute)
        {
            this.Name = name;
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
            this.ExecuteCallback?.Invoke(this, this.Current);
        }
    }
}
