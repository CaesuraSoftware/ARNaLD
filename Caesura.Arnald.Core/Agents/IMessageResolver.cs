
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    
    public interface IMessageResolver
    {
        String Name { get; set; }
        IMessageHandler Owner { get; set; }
        IMessage Current { get; set; }
        MessageResolverResult Check(IMessage message);
        void Execute();
    }
}
