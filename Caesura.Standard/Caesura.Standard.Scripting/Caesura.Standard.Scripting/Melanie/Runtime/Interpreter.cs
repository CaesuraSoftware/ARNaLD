
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
                
            };
            this.Types        = new Dictionary<TypeIndicator, IMelType>()
            {
                { TypeIndicator.Int8    , new MelInt8    () },
                { TypeIndicator.Int16   , new MelInt16   () },
                { TypeIndicator.Int32   , new MelInt32   () },
                { TypeIndicator.Int64   , new MelInt64   () },
                { TypeIndicator.Single  , new MelSingle  () },
                { TypeIndicator.Double  , new MelDouble  () },
                { TypeIndicator.Boolean , new MelBoolean () },
                { TypeIndicator.Object  , new MelObject  () },
                { TypeIndicator.Pointer , new MelPointer () },
                { TypeIndicator.Func    , new MelFunc    () },
            };
            this.Contexts     = new List<Context>();
            this.MainContext  = new Context(this);
        }
        
        public Interpreter(RuntimeConfiguration rtc) : this()
        {
            
        }
        
        public void Execute(OpCode code)
        {
            if (code == OpCode.NoOp)
            {
                return;
            }
            if (!this.Instructions.ContainsKey(code))
            {
                throw new UnrecognizedOpcodeException(code);
            }
            var inst = this.Instructions[code];
            inst.Execute();
        }
    }
}
