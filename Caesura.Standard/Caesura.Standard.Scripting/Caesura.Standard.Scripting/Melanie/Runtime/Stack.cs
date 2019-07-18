
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using Types;
    
    public class Stack
    {
        public List<BaseType> MainStack { get; set; }
        public List<String> CallStack { get; set; }
        
        public Stack()
        {
            this.MainStack = new List<BaseType>();
            this.CallStack = new List<String>();
        }
        
        
    }
}
