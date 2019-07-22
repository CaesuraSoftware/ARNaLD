
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using Instructions;
    using Types;
    
    public class Interpreter
    {
        public Dictionary<OpCode, BaseInstruction> Instructions { get; set; }
        public Dictionary<TypeIndicator, IMelType> Types { get; set; }
        public List<Context> Contexts { get; set; }
        public Context MainContext { get; set; }
        
        public Interpreter()
        {
            this.Instructions = new Dictionary<OpCode, BaseInstruction>()
            {
                { OpCode.Push           , new Ins_Push       (this) },
                { OpCode.Pop            , new Ins_Pop        (this) },
                { OpCode.Add            , new Ins_Add        (this) },
                { OpCode.Sub            , new Ins_Sub        (this) },
                { OpCode.Div            , new Ins_Div        (this) },
                { OpCode.Mul            , new Ins_Mul        (this) },
                { OpCode.Rem            , new Ins_Rem        (this) },
            };
            this.Types        = new Dictionary<TypeIndicator, IMelType>()
            {
                { TypeIndicator.Int8    , new MelInt8        ()     },
                { TypeIndicator.Int16   , new MelInt16       ()     },
                { TypeIndicator.Int32   , new MelInt32       ()     },
                { TypeIndicator.Int64   , new MelInt64       ()     },
                { TypeIndicator.Single  , new MelSingle      ()     },
                { TypeIndicator.Double  , new MelDouble      ()     },
                { TypeIndicator.Boolean , new MelBoolean     ()     },
                { TypeIndicator.Object  , new MelObject      ()     },
                { TypeIndicator.Array   , new MelObject      ()     },
                { TypeIndicator.String  , new MelString      ()     },
                { TypeIndicator.Pointer , new MelPointer     ()     },
                { TypeIndicator.Func    , new MelFunc        ()     },
            };
            this.Contexts     = new List<Context>();
            this.MainContext  = new Context(this);
        }
        
        public Interpreter(RuntimeConfiguration rtc) : this()
        {
            
        }
        
        public void ParseInstruction(OpCode code, IEnumerable<IMelType> args, Context context)
        {
            if (code == OpCode.Nop)
            {
                return;
            }
            if (!this.Instructions.ContainsKey(code))
            {
                throw new UnrecognizedOpcodeException(code);
            }
            if (args != null)
            {
                foreach (var arg in args)
                {
                    context.PushArgument(arg);
                }
            }
            var inst = this.Instructions[code];
            inst.Execute(context);
        }
        
        public void ParseInstruction(OpCode code, IEnumerable<IMelType> args)
        {
            this.ParseInstruction(code, args, this.MainContext);
        }
        
        public void ParseInstruction(OpCode code, params IMelType[] args)
        {
            this.ParseInstruction(code, args, this.MainContext);
        }
        
        public void ParseInstruction(OpCode code)
        {
            this.ParseInstruction(code, null, this.MainContext);
        }
    }
}
