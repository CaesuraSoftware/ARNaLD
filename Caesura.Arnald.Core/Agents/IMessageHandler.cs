
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using Caesura.Standard;
    
    public interface IMessageHandler
    {
        IAgent HostAgent { get; set; }
        void AddResolver(IMessageResolver resolver);
        Boolean RemoveResolver(IMessageResolver resolver);
        Maybe<IMessageResolver> GetResolver(Predicate<IMessageResolver> predicate);
        void Process(IMessage message);
    }
}
