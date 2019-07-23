
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime.Instructions
{
    using System.Collections.Generic;
    using System.Linq;
    using Types;
    
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
    
    public class Ins_Swap : BaseInstruction
    {
        public override OpCode Code => OpCode.Swap;
        
        public Ins_Swap(Interpreter env) : base(env)
        {
            
        }
        
        public override void Execute(Context context)
        {
            /**/ if (context.Stack.MainStack.Count == 0)
            {
                throw new InvalidOperationException("Stack is empty");
            }
            else if (context.Stack.MainStack.Count <= 1)
            {
                throw new InvalidOperationException("Not enough arguments on stack");
            }
            
            if (context.Arguments.Count > 0)
            {
                var marg = context.Arguments.Pop().Value;
                /**/ if (marg is MelInt32 m32)
                {
                    var num = m32.InternalRepresentation;
                    /**/ if (num == 0) // normal swap
                    {
                        context.Stack.Swap();
                    }
                    else if (num >= context.Stack.Count)
                    {
                        throw new InvalidOperationException($"SWAP argument ({num}) cannot be more than the current stack ({context.Stack.Count})");
                    }
                    else if (num < 0)
                    {
                        throw new InvalidOperationException($"SWAP argument ({num}) cannot be negative");
                    }
                    else
                    {
                        var item1 = context.Stack.MainStack.ElementAt(context.Stack.Count - num - 1);
                        var item2 = context.Stack.MainStack.ElementAt(context.Stack.Count - num - 2);
                        var index1 = context.Stack.MainStack.LastIndexOf(item1);
                        var index2 = context.Stack.MainStack.LastIndexOf(item2);
                        context.Stack.MainStack[index1] = item2;
                        context.Stack.MainStack[index2] = item1;
                    }
                }
                else if (marg is MelInt64 m64)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    throw new InvalidOperationException("Invalid argument for SWAP");
                }
            }
            else
            {
                context.Stack.Swap();
            }
        }
    }
    
    public class Ins_Dup : BaseInstruction
    {
        public override OpCode Code => OpCode.Dup;
        
        public Ins_Dup(Interpreter env) : base(env)
        {
            
        }
        
        public override void Execute(Context context)
        {
            if (context.Stack.MainStack.Count == 0)
            {
                throw new InvalidOperationException("Stack is empty");
            }
            
            if (context.Arguments.Count > 0)
            {
                var marg = context.Arguments.Pop().Value;
                /**/ if (marg is MelString ms)
                {
                    if (ms.InternalRepresentation == "*")
                    {
                        // pop value from the stack and run again
                        var pop = this.GetInstruction(OpCode.Pop, context);
                        pop();
                        this.Execute(context);
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid argument for DUP");
                    }
                }
                else if (marg is MelInt32 m32)
                {
                    var num = m32.InternalRepresentation;
                    /**/ if (num == 0 || num == 1) // normal dup
                    {
                        context.Stack.Dup();
                    }
                    else if (num < 0)
                    {
                        throw new InvalidOperationException($"DUP argument ({num}) cannot be negative");
                    }
                    else
                    {
                        for (var i = 0; i < num; i++)
                        {
                            context.Stack.Dup();
                        }
                    }
                }
                else if (marg is MelInt64 m64)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    throw new InvalidOperationException("Invalid argument for DUP");
                }
            }
            else
            {
                context.Stack.Dup();
            }
        }
    }
}
