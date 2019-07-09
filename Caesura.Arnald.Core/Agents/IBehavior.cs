
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    
    public interface IBehavior : IComparable, IDisposable
    {
        String Name { get; }
        Int32 Priority { get; set; }
        Task<IMessage> Execute(IEnumerable<IMessage> messages);
    }
}
