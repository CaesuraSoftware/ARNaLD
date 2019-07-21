
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
    
    public class Ins_Add : BaseInstruction
    {
        public override OpCode Code => OpCode.Add;
        
        public Ins_Add(Interpreter env) : base(env)
        {
            
        }
        
        public override void Execute(Context context)
        {
            var pop  = this.GetInstruction(OpCode.Pop , context);
            var push = this.GetInstruction(OpCode.Push, context);
            
            pop();
            pop();
            
            var x = context.PopArgument();
            var y = context.PopArgument();
            var result = NumberHelper.Add(x.Value, y.Value);
            
            context.PushArgument(result);
            push();
        }
    }
    
    public class Ins_Sub : BaseInstruction
    {
        public override OpCode Code => OpCode.Sub;
        
        public Ins_Sub(Interpreter env) : base(env)
        {
            
        }
        
        public override void Execute(Context context)
        {
            var pop  = this.GetInstruction(OpCode.Pop , context);
            var push = this.GetInstruction(OpCode.Push, context);
            
            pop();
            pop();
            
            var x = context.PopArgument();
            var y = context.PopArgument();
            var result = NumberHelper.Sub(x.Value, y.Value);
            
            context.PushArgument(result);
            push();
        }
    }
}
