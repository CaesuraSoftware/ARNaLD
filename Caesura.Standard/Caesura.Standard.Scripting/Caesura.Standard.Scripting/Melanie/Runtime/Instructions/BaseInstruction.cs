
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
        
        protected void RunOp(OpCode code, Context context)
        {
            var oc = this.Environment.Instructions[code];
            oc.Execute(context);
        }
        
        protected BaseInstruction GetInstruction(OpCode code)
        {
            return this.Environment.Instructions[code];
        }
        
        protected Action<Context> GetInstructionFunc(OpCode code)
        {
            return this.Environment.Instructions[code].Execute;
        }
        
        protected Action GetInstruction(OpCode code, Context context)
        {
            var oc = this.GetInstruction(code);
            return () => oc.Execute(context);
        }
    }
}
