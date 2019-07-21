
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime.Instructions
{
    using System.Collections.Generic;
    using System.Linq;
    
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
    
    public class Ins_Div : BaseInstruction
    {
        public override OpCode Code => OpCode.Div;
        
        public Ins_Div(Interpreter env) : base(env)
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
            var result = NumberHelper.Div(x.Value, y.Value);
            
            context.PushArgument(result);
            push();
        }
    }
    
    public class Ins_Mul : BaseInstruction
    {
        public override OpCode Code => OpCode.Mul;
        
        public Ins_Mul(Interpreter env) : base(env)
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
            var result = NumberHelper.Mul(x.Value, y.Value);
            
            context.PushArgument(result);
            push();
        }
    }
    
    public class Ins_Rem : BaseInstruction
    {
        public override OpCode Code => OpCode.Rem;
        
        public Ins_Rem(Interpreter env) : base(env)
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
            var result = NumberHelper.Rem(x.Value, y.Value);
            
            context.PushArgument(result);
            push();
        }
    }
}
