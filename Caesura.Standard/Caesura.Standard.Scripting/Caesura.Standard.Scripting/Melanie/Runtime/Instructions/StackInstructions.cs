
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime.Instructions
{
    using System.Collections.Generic;
    using System.Linq;
    
    public class Ins_Push : BaseInstruction
    {
        public override OpCode Code => OpCode.Push;
        
        public Ins_Push(Interpreter env) : base(env)
        {
            
        }
        
        public override void Execute(Context context)
        {
            var arg = context.PopArgument();
            context.Push(arg.Value);
        }
    }
    
    public class Ins_Pop : BaseInstruction
    {
        public override OpCode Code => OpCode.Pop;
        
        public Ins_Pop(Interpreter env) : base(env)
        {
            
        }
        
        public override void Execute(Context context)
        {
            if (context.Stack.MainStack.Count == 0)
            {
                throw new InvalidOperationException("Stack is empty");
            }
            
            var arg = context.Pop();
            context.PushArgument(arg.Value);
        }
    }
}
