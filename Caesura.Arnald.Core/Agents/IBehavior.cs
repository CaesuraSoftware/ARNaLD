
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    
    public interface IBehavior : IComparable, IDisposable
    {
        IAgent Owner { get; set; }
        String Name { get; }
        Int32 Priority { get; set; }
        void Execute(IMessage message);
    }
}
