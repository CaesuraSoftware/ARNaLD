
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    public class StateAtom
    {
        // TODO: define one state here (and how it connects with other states)
        public IStateBehavior Parent { get; set; }
        public String Name { get; set; }
        public Action Action { get; set; }
        private List<StateAtom> ComesAfterState { get; set; }
        
        public StateAtom()
        {
            this.ComesAfterState = new List<StateAtom>();
        }
        
        public Boolean AddPreceedingState(StateAtom atom)
        {
            if (this.ComesAfterState.Exists(x => x.Name == atom.Name || x.Name == this.Name))
            {
                return false;
            }
            this.ComesAfterState.Add(atom);
            return true;
        }
        
        public Boolean ComesAfter(String name)
        {
            return this.ComesAfterState.Exists(x => x.Name == name);
        }
    }
}
