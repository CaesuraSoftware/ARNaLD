
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using Types;
    
    public class Context
    {
        public Stack Stack { get; set; }
        public List<IMelType> Arguments { get; set; }
        
        public Context()
        {
            this.Stack = new Stack();
            this.Arguments = new List<IMelType>();
        }
        
        public Context(Interpreter handle) : this()
        {
            
        }
    }
}
