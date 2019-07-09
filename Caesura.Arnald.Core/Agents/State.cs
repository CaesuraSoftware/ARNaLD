
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    
    public class State : IState
    {
        // TODO: a state machine for state machines
        public IAgent Owner { get; set; }
        private List<StateBehavior> Behaviors { get; set; }
    }
}
