
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using Types;
    
    public class Stack
    {
        public List<IMelType> MainStack { get; set; }
        public List<String> CallStack { get; set; }
        
        public Stack()
        {
            this.MainStack = new List<IMelType>();
            this.CallStack = new List<String>();
        }
        
        
    }
}
