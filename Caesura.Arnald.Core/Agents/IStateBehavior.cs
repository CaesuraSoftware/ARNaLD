
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    
    public interface IStateBehavior
    {
        IState Parent { get; set; }
    }
}
