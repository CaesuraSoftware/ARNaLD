
using System;

namespace Caesura.Arnald.Core.Agents
{
    
    public interface IState : IDisposable
    {
        IAgent HostAgent { get; set; }
        IStateAtom InitialState { get; set; }
        IStateAtom Current { get; }
        Boolean TryAdd(IStateAtom atom);
        void Add(IStateAtom atom);
        void Add(String name, IStateAtomCallback callback);
        Boolean Remove(IStateAtom atom);
        IStateAtom Find(Predicate<IStateAtom> predicate);
        Boolean TrySetState(String name);
        Boolean TrySetInitialState(String name);
        void SetState(String name);
        void Next();
        void Next(IMessage message);
        void Dispose(IMessage message);
    }
}
