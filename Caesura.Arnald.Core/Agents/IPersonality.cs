
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    
    public interface IPersonality : IDisposable
    {
        void Learn(IBehavior behavior);
    }
}
