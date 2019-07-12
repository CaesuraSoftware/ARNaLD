
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    
    public interface IState : IDisposable
    {
        IAgent Owner { get; set; }
        IStateAtom InitialState { get; set; }
        Boolean TryAdd(IStateAtom atom);
        void Add(IStateAtom atom);
        void Add(String name, IStateAtomCallback callback);
        Boolean Remove(IStateAtom atom);
        IStateAtom Find(Predicate<IStateAtom> predicate);
        Boolean TrySetState(String name);
        void SetState(String name);
        void Next(IMessage message);
    }
}
