
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    
    public interface IAgent : IDisposable
    {
        String Name { get; }
        Guid Identifier { get; }
        IPersonality Personality { get; }
        void Execute();
        void Learn(IBehavior behavior);
        void Send(IMessage message);
        void Send(IEnumerable<IMessage> messages);
    }
}
