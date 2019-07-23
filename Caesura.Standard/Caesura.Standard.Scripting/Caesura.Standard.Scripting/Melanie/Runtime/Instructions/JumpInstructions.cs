
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime.Instructions
{
    using System.Collections.Generic;
    using System.Linq;
    using Types;
    
    public class Ins_Def : BaseInstruction
    {
        public override OpCode Code => OpCode.Def;
        
        public Ins_Def(Interpreter env) : base(env)
        {
            
        }
        
        public override void Execute(Context context)
        {
            // TODO: CACHE RESULTS
            
            var marg = context.PopArgument();
            /**/ if (marg.NoValue)
            {
                throw new InvalidOperationException("DEF requires an argument");
            }
            else if (!(marg.Value is MelString))
            {
                throw new InvalidOperationException("DEF requires a String argument");
            }
            var line = (marg.Value as MelString).InternalRepresentation;
            
            // Parse arguments
            var args = new List<String>();
            var name = this.GetNameAndArgs(line, args);
            
            // Register function name
            var cs = context.Listing[context.ProgramCounter];
            if (String.IsNullOrEmpty(cs.FunctionName))
            {
                cs.FunctionName = name;
            }
            
            // Verify stack
            if (context.Stack.Count < args.Count)
            {
                throw new RuntimeException("Stack does not have enough arguments for this function call");
            }
        }
        
        public Boolean FunctionRegistered(String line, Context context)
        {
            var name = line.Split(' ', '(')[0].Trim();
            foreach (var item in context.Listing)
            {
                if (item.Value.FunctionName == name)
                {
                    return true;
                }
            }
            return false;
        }
        
        public String GetNameAndArgs(String line, List<String> args)
        {
            var name = String.Empty;
            var working = String.Empty;
            var inargs = false;
            foreach (var c in line)
            {
                /**/ if (c == ' ' && String.IsNullOrEmpty(name))
                {
                    name = working;
                    working = String.Empty;
                }
                else if (c == ' ' && !String.IsNullOrEmpty(name))
                {
                    continue;
                }
                else if (c =='(' && !inargs)
                {
                    inargs = true;
                }
                else if (c =='(' && inargs)
                {
                    throw new InvalidOperationException("Duplicate open parenthesis in function definition");
                }
                else if (c == ')' && inargs)
                {
                    args.Add(working);
                    working = String.Empty;
                }
                else if (c == ')' && !inargs)
                {
                    throw new InvalidOperationException("Unmatched close parenthesis in function definition");
                }
                else if (c == ',' && inargs)
                {
                    args.Add(working);
                    working = String.Empty;
                }
                else
                {
                    working += c;
                }
            }
            if (String.IsNullOrEmpty(name))
            {
                name = working.TrimEnd();
            }
            
            return name;
        }
    }
    
    public class Ins_Call : BaseInstruction
    {
        public override OpCode Code => OpCode.Call;
        
        public Ins_Call(Interpreter env) : base(env)
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
                else if (argms.StartsWith("[") && argms.EndsWith("]"))
                {
                    var extarg = argms.Trim('[', ']', ' ');
                    context.ExternalCall(extarg);
                }
                else
                {
                    CallSite<IMelType> cs = default;
                    Int64 pc = 0;
                    var arg1 = argms.Replace(" ", String.Empty);
                    foreach (var item in context.Listing)
                    {
                        if (String.IsNullOrEmpty(item.Value.FunctionDef))
                        {
                            continue;
                        }
                        
                        var arg2 = item.Value.FunctionDef.Replace(" ", String.Empty);
                        if (arg1 == arg2)
                        {
                            cs = item.Value;
                            pc = item.Key;
                            break;
                        }
                    }
                    
                    if (cs == default)
                    {
                        throw new RuntimeException($"Function definition \"{argms}\" not found");
                    }
                    
                    context.Call(pc);
                }
            }
            else if (argv is MelInt64 argm64)
            {
                var argi64 = argm64.InternalRepresentation;
                context.Call(argi64);
            }
            else if (argv is MelInt32 argm32)
            {
                var argi32 = argm32.InternalRepresentation;
                context.Call(argi32);
            }
        }
    }
    
    public class Ins_Ret : BaseInstruction
    {
        public override OpCode Code => OpCode.Ret;
        
        public Ins_Ret(Interpreter env) : base(env)
        {
            
        }
        
        public override void Execute(Context context)
        {
            context.ReturnFromCall();
        }
    }
    
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
