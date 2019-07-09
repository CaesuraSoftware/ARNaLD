
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    
    public class StateBehavior : IStateBehavior
    {
        // TODO: a state machine
        private List<StateAtom> Atoms { get; set; }
    }
}
