
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    
    public interface IAgentConfiguration
    {
        IAgent Owner { get; set; }
        String Name { get; set; }
        Guid Identifier { get; set; }
        IPersonality Personality { get; set; }
        IMailbox Messages { get; set; }
        IState AgentState { get; set; }
        void Copy(IAgentConfiguration config);
    }
}
