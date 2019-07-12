
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Caesura.Standard;
    
    public interface IPersonality : IDisposable
    {
        Task RunAsync(String name, IMessage message);
        void Run(String name, IMessage message);
        void Learn(IBehavior behavior);
        void Unlearn(IBehavior behavior);
        void Unlearn(Predicate<IBehavior> predicate);
        void UnlearnAll();
        Boolean HasBehavior(IBehavior behavior);
        Boolean HasBehavior(String name);
        Maybe<IBehavior> GetBehavior(Predicate<IBehavior> predicate);
    }
}
