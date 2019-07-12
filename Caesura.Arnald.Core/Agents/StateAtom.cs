
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Caesura.Standard;
    
    public class StateAtom : IStateAtom
    {
        public IState Environment { get; set; }
        public String Name { get; set; }
        public Func<StateAtom, String> Callback { get; set; }
        
        public StateAtom()
        {
            
        }
        
        public StateAtom(IState parent) : this()
        {
            this.Environment = parent;
        }
        
        public StateAtom(IState parent, Func<StateAtom, String> callback) : this(parent)
        {
            this.Callback = callback;
        }
        
        public Maybe<String> Call()
        {
            var result = this.Callback?.Invoke(this);
            if (!String.IsNullOrEmpty(result))
            {
                return result;
            }
            return Maybe.Unit;
        }
    }
}
