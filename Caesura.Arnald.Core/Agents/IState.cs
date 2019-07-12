
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    
    public interface IState
    {
        IAgent Agent { get; set; }
        void Next();
    }
}
