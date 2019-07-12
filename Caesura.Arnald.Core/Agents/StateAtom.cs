
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Caesura.Standard;
    
    public delegate String IStateAtomCallback(StateAtom atom, IMessage message);
    
    public class StateAtom : IStateAtom
    {
        public IState Environment { get; set; }
        public String Name { get; set; }
        public IStateAtomCallback Callback { get; set; }
        
        public StateAtom()
        {
            
        }
        
        public StateAtom(IState parent) : this()
        {
            this.Environment = parent;
        }
        
        public StateAtom(IState parent, IStateAtomCallback callback) : this(parent)
        {
            this.Callback = callback;
        }
        
        public Maybe<String> Call(IMessage message)
        {
            var result = this.Callback?.Invoke(this, message);
            if (!String.IsNullOrEmpty(result))
            {
                return result;
            }
            return Maybe.None;
        }
    }
}
