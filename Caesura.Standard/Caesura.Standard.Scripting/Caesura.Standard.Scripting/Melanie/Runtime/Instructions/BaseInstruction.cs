
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime.Instructions
{
    using System.Collections.Generic;
    using System.Linq;
    
    public abstract class BaseInstruction
    {
        protected Interpreter Environment { get; set; }
        public abstract OpCode Code { get; }
        
        public BaseInstruction()
        {
            
        }
        
        public BaseInstruction(Interpreter handle) : this()
        {
            this.Environment = handle;
        }
        
        public abstract void Execute(Context context);
    }
}
