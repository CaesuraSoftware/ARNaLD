
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
        ThreadState AgentThreadState { get; }
        void Start();
        void Stop();
        void Wait();
        void Run();
        void CycleOnce();
        void Learn(IBehavior behavior);
        Boolean Send(IMessage message);
        void Send(IMessage message, CancellationToken token);
        void Dispose(Boolean wait);
    }
}
