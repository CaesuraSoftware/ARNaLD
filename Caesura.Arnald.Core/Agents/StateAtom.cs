
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    
    public class StateAtom
    {
        // TODO: define one state here (and how it connects with other states)
        public String Name { get; set; }
        private List<StateAtom> ComesAfterState { get; set; }
        
        public StateAtom()
        {
            this.ComesAfterState = new List<StateAtom>();
        }
        
        public void AddPreceedingState(StateAtom atom)
        {
            if (this.ComesAfterState.Exists(x => x.Name == atom.Name || x.Name == this.Name))
            {
                return;
            }
            this.ComesAfterState.Add(atom);
        }
        
        public Boolean ComesAfter(String name)
        {
            return this.ComesAfterState.Exists(x => x.Name == name);
        }
    }
}
