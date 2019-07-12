
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    
    public class State : IState
    {
        public IAgent Agent { get; set; }
        private List<IStateAtom> Atoms { get; set; }
        private IStateAtom Current { get; set; }
        
        public State()
        {
            this.Atoms = new List<IStateAtom>();
        }
        
        public void Next()
        {
            
        }
    }
}
