
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    
    public interface IPersonality : IDisposable
    {
        IEnumerable<Task> Execute(IEnumerable<IMessage> messages);
        void Learn(IBehavior behavior);
    }
}