
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime.Instructions
{
    using System.Collections.Generic;
    using System.Linq;
    using Types;
    
    public class Ins_Jmp : BaseInstruction
    {
        public override OpCode Code => OpCode.Jmp;
        
        public Ins_Jmp(Interpreter env) : base(env)
        {
            
        }
        
        public override void Execute(Context context)
        {
            var arg = context.PopArgument();
            var argv = arg.Value;
            /**/ if (argv is MelInt64 argm64)
            {
                var argi64 = argm64.InternalRepresentation;
                context.ProgramCounter = argi64;
            }
            else if (argv is MelInt32 argm32)
            {
                var argi32 = argm32.InternalRepresentation;
                context.ProgramCounter = argi32;
            }
        }
    }
}
