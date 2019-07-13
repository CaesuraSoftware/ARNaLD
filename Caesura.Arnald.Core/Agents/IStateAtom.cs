
using System;

namespace Caesura.Arnald.Core.Agents
{
    using Caesura.Standard;
    
    public delegate String IStateAtomCallback(StateAtom atom, IMessage message);
    
    public interface IStateAtom
    {
        String Name { get; set; }
        Maybe<String> Call(IMessage message);
    }
}
