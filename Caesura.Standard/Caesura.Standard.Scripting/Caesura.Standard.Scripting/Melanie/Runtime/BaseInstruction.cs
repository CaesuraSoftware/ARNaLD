
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    
    public abstract class BaseInstruction
    {
        protected Interpreter Environment { get; set; }
        public OpCode Code { get; set; }
        
        public BaseInstruction()
        {
            
        }
        
        public BaseInstruction(Interpreter handle) : this()
        {
            this.Environment = handle;
        }
        
        public abstract void Execute();
    }
}
