
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    
    public class StateBehavior : IStateBehavior
    {
        // TODO: a state machine
        public IState Parent { get; set; }
        private List<StateAtom> Atoms { get; set; }
    }
}
