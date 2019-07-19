
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
            if (context.Arguments.Count == 1)
            {
                var arg = context.Arguments.ElementAt(0);
                context.Arguments.Clear();
                context.Stack.Push(arg);
            }
            else
            {
                throw new InvalidOperationException(
                    $"PUSH instruction takes exactly one argument. " + 
                    $"Arguments given: {context.Arguments.Count}"
                );
            }
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
            
            var arg = context.Stack.MainStack.Last();
            var argdex = context.Stack.MainStack.LastIndexOf(arg);
            context.Stack.MainStack.RemoveAt(argdex);
            context.Arguments.Add(arg);
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
            
            var index = context.Arguments.Count;
            var x = context.Arguments.ElementAt(index - 2);
            var y = context.Arguments.ElementAt(index - 1);
            
            var result = NumberHelper.Add(x, y);
            
            context.Arguments.RemoveAt(context.Arguments.LastIndexOf(x));
            context.Arguments.RemoveAt(context.Arguments.LastIndexOf(y));
            
            context.Arguments.Add(result);
            push.Execute(context);
        }
    }
}
