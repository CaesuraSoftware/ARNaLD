
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    
    public interface IMessageHandler
    {
        IAgent Owner { get; set; }
        void Process(IMessage message);
    }
}
