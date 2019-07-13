
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    
    public interface IAgent : IDisposable
    {
        ILocator HostLocator { get; set; }
        String Name { get; }
        Guid Identifier { get; }
        IPersonality Personality { get; }
        IMessageHandler Resolver { get; set; }
        IState AgentState { get; set; }
        ThreadState AgentThreadState { get; }
        AgentAutonomy Autonomy { get; set; }
        void Setup(IAgentConfiguration config);
        void Start();
        void Stop();
        void Wait();
        void Run();
        void CycleOnce();
        void CycleOnce(CancellationToken token);
        void CycleOnceNoBlock();
        Boolean Send(IMessage message);
        void Send(IMessage message, CancellationToken token);
        void Dispose(Boolean wait);
    }
}
