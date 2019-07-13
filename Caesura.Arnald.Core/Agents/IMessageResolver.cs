
using System;

namespace Caesura.Arnald.Core.Agents
{
    
    public interface IMessageResolver
    {
        String Name { get; set; }
        IMessageHandler HostHandler { get; set; }
        IMessage Current { get; set; }
        Boolean CheckIfRecipientIsHostName { get; set; }
        MessageResolverResult Check(IMessage message);
        void Execute();
    }
}
