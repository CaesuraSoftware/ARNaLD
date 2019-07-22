
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
