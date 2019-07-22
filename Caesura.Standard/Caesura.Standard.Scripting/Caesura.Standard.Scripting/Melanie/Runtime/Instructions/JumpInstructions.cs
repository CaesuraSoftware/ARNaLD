
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
            /**/ if (argv is MelString ms)
            {
                var argms = ms.InternalRepresentation;
                if (argms == "*")
                {
                    var pop = this.GetInstruction(OpCode.Pop, context);
                    pop();
                    this.Execute(context); // recurse
                }
                else
                {
                    throw new InvalidOperationException("Invalid JMP argument");
                }
            }
            else if (argv is MelInt64 argm64)
            {
                var argi64 = argm64.InternalRepresentation;
                context.ProgramCounter = argi64 - 1; // minus 1 so it doesn't skip the instruction it jumps to
            }
            else if (argv is MelInt32 argm32)
            {
                var argi32 = argm32.InternalRepresentation;
                context.ProgramCounter = argi32 - 1;
            }
        }
    }
}
