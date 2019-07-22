
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime.Instructions
{
    using System.Collections.Generic;
    using System.Linq;
    using Types;
    
    public class Ins_New : BaseInstruction
    {
        public override OpCode Code => OpCode.New;
        
        public Ins_New(Interpreter env) : base(env)
        {
            
        }
        
        public override void Execute(Context context)
        {
            var arg = context.PopArgument().Value;
            /**/ if (arg is MelString str)
            {
                if (str.InternalRepresentation == "*")
                {
                    // pop value from the stack and run again
                    var pop = this.GetInstruction(OpCode.Pop, context);
                    pop();
                    this.Execute(context);
                }
                else
                {
                    throw new InvalidOperationException($"Invalid argument for NEW");
                }
            }
            else if (arg is MelInt32 m32)
            {
                var no = new MelObject();
                var id = m32.InternalRepresentation;
                no.Id = id;
                if (context.Environment.Objects.ContainsKey(id))
                {
                    context.Environment.Objects.Remove(id);
                }
                context.Environment.Objects.Add(id, no);
            }
            else
            {
                throw new InvalidOperationException($"Invalid argument for NEW");
            }
        }
    }
    
    public class Ins_Fetch : BaseInstruction
    {
        public override OpCode Code => OpCode.Fetch;
        
        public Ins_Fetch(Interpreter env) : base(env)
        {
            
        }
        
        public override void Execute(Context context)
        {
            var arg = context.PopArgument().Value;
            /**/ if (arg is MelString str)
            {
                if (str.InternalRepresentation == "*")
                {
                    // pop value from the stack and run again
                    var pop = this.GetInstruction(OpCode.Pop, context);
                    pop();
                    this.Execute(context);
                }
                else
                {
                    throw new InvalidOperationException($"Invalid argument for FETCH");
                }
            }
            else if (arg is MelInt32 m32)
            {
                var id = m32.InternalRepresentation;
                if (context.Environment.Objects.ContainsKey(id))
                {
                    var obj = context.Environment.Objects[id];
                    var pop = this.GetInstruction(OpCode.Pop, context);
                    pop();
                    var mname = context.PopArgument();
                    if (mname.HasValue && (mname.Value is MelString mmname))
                    {
                        var name = mmname.InternalRepresentation;
                        if (obj.Fields.ContainsKey(name))
                        {
                            var push = this.GetInstruction(OpCode.Push, context);
                            var item = obj.Fields[name] as IMelType;
                            if (!(item is null))
                            {
                                context.PushArgument(item);
                                push();
                            }
                            else
                            {
                                throw new InvalidOperationException($"Invalid argument for FETCH");
                            }
                        }
                        else
                        {
                            throw new ElementNotFoundException($"Field \"{name}\" of ID {id} does not exist");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException($"Invalid argument for FETCH");
                    }
                }
                else
                {
                    throw new ElementNotFoundException($"Object of ID {id} not found");
                }
            }
            else
            {
                throw new InvalidOperationException($"Invalid argument for FETCH");
            }
        }
    }
    
    public class Ins_Store : BaseInstruction
    {
        public override OpCode Code => OpCode.Store;
        
        public Ins_Store(Interpreter env) : base(env)
        {
            
        }
        
        public override void Execute(Context context)
        {
            var arg = context.PopArgument().Value;
            /**/ if (arg is MelString str)
            {
                if (str.InternalRepresentation == "*")
                {
                    // pop value from the stack and run again
                    var pop = this.GetInstruction(OpCode.Pop, context);
                    pop();
                    this.Execute(context);
                }
                else
                {
                    throw new InvalidOperationException($"Invalid argument for STORE");
                }
            }
            else if (arg is MelInt32 m32)
            {
                var id = m32.InternalRepresentation;
                if (context.Environment.Objects.ContainsKey(id))
                {
                    var obj = context.Environment.Objects[id];
                    var pop = this.GetInstruction(OpCode.Pop, context);
                    pop();
                    pop();
                    var mname = context.PopArgument();
                    var mval  = context.PopArgument();
                    if (mname.HasValue && (mname.Value is MelString mmname) && mval.HasValue)
                    {
                        var name = mmname.InternalRepresentation;
                        if (obj.Fields.ContainsKey(name))
                        {
                            obj.Fields.Remove(name);
                        }
                        obj.Fields.Add(name, mval);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Invalid argument for STORE");
                    }
                }
                else
                {
                    throw new ElementNotFoundException($"Object of ID {id} not found");
                }
            }
            else
            {
                throw new InvalidOperationException($"Invalid argument for STORE");
            }
        }
    }
    
    public class Ins_Delete : BaseInstruction
    {
        public override OpCode Code => OpCode.Delete;
        
        public Ins_Delete(Interpreter env) : base(env)
        {
            
        }
        
        public override void Execute(Context context)
        {
            var arg = context.PopArgument().Value;
            /**/ if (arg is MelString str)
            {
                if (str.InternalRepresentation == "*")
                {
                    // pop value from the stack and run again
                    var pop = this.GetInstruction(OpCode.Pop, context);
                    pop();
                    this.Execute(context);
                }
                else
                {
                    throw new InvalidOperationException($"Invalid argument for DELETE");
                }
            }
            else if (arg is MelInt32 m32)
            {
                var id = m32.InternalRepresentation;
                if (context.Environment.Objects.ContainsKey(id))
                {
                    context.Environment.Objects.Remove(id);
                }
                else
                {
                    throw new ElementNotFoundException($"Object of ID {id} not found");
                }
            }
            else
            {
                throw new InvalidOperationException($"Invalid argument for DELETE");
            }
        }
    }
}
