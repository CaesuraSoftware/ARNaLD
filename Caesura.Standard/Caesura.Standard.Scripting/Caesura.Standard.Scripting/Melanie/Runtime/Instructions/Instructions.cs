
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
            context.Push(arg);
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
            context.PushArgument(arg);
        }
    }
    
    public class Ins_Add : BaseInstruction
    {
        public override OpCode Code => OpCode.Add;
        
        public Ins_Add(Interpreter env) : base(env)
        {
            
        }
        
        public override void Execute(Context context)
        {
            var pop = this.Environment.Instructions[OpCode.Pop];
            var push = this.Environment.Instructions[OpCode.Push];
            
            pop.Execute(context);
            pop.Execute(context);
            
            var x = context.PopArgument();
            var y = context.PopArgument();
            var result = NumberHelper.Add(x, y);
            
            context.Push(result);
            push.Execute(context);
        }
    }
}
