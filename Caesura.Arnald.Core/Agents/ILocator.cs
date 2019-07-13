
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using Caesura.Standard;
    
    public interface ILocator : IDisposable
    {
        String Name { get; set; }
        Guid Identifier { get; set; }
        Boolean DisposeAgentsOnDispose { get; set; }
        event Action<ILocator, IAgent> OnAdd;
        event Action<ILocator, IAgent> OnRemove;
        event Action<ILocator, IAgent> OnDisposeAgent;
        event Action<ILocator> OnDispose;
        
        Maybe<IAgent> Find(Predicate<IAgent> predicate);
        Maybe<IAgent> Find(String name);
        IEnumerable<IAgent> FindAll(Predicate<IAgent> predicate);
        void Add(IAgent agent);
        Boolean TryAdd(IAgent agent);
        Boolean Remove(Predicate<IAgent> predicate);
        Boolean Remove(String name);
        Boolean Remove(IAgent agent);
        void Clear();
        void Clear(Boolean disposeAgents);
        
    }
}
