
using System;

namespace Caesura.Arnald.Core.Agents
{
    using Caesura.Standard;
    
    public delegate String IStateAtomCallback(StateAtom atom, IMessage message);
    
    public static class StateAtomState
    {
        public static String Begin = "BEGIN";
        public static String End = "END";
        public static String Disposing = "DISPOSING";
        public static String Cancelled = "CANCELLED";
    }
    
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
        
        public StateAtom(IState parent, String name) : this()
        {
            this.Name = name;
        }
        
        public StateAtom(IState parent, String name, IStateAtomCallback callback) : this(parent, name)
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
