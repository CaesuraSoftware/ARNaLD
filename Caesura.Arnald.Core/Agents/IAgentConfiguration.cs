
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Threading;
    
    public interface IAgentConfiguration
    {
        IAgent Owner { get; set; }
        ILocator Location { get; set; }
        String Name { get; set; }
        Guid Identifier { get; set; }
        IPersonality Personality { get; set; }
        IMessageHandler Resolver { get; set; }
        IMailbox Messages { get; set; }
        IState AgentState { get; set; }
        AgentAutonomy Autonomy { get; set; }
        CancellationTokenSource CancelToken { get; set; }
        void Copy(IAgentConfiguration config);
    }
}
