
using System;

namespace Caesura.Arnald.Core.Agents
{
    
    public interface IBehavior : IComparable, IDisposable
    {
        IAgent Owner { get; set; }
        String Name { get; }
        Int32 Priority { get; set; }
        void Execute(IMessage message);
    }
}
