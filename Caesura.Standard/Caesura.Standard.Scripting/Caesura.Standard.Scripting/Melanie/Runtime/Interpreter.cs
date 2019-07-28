
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using Instructions;
    using Types;
    using AsmParser;
    
    // TODO:
    // - THIS instruction that pushes the current object ID on to
    //   the stack (each function needs a local variable object for this)
    // - make all types inherit from MelObject
    // - give functions a local stack that is indexable.
    //   working with this global stack using swap instructions
    //   is pretty much impossible, even for a compiler.
    // - implement exceptions
    // - implement a FUNC instruction to describe arguments and
    //   return value. It will check/verify the stack and also
    //   make sure a matching RET instruction terminates it.
    //   Additionally, have Main work as the main function.
    // - have FUNC reset the line numbers so we can reuse line.
    //   numbers in functions.
    // - debugging states and breakpoints.
    // - proper error/expection output with line numbers and stacktrace
    // - header metadata that the compiler can fill in.
    // - better internal exceptions.
    // - Some kind of global object ID counter so constructors can
    //   pass the increment of that counter or something.
    // FIXME:
    // - Nothing so far!
    
    public class Interpreter
    {
        public Dictionary<OpCode, BaseInstruction> Instructions { get; set; }
        public Dictionary<TypeIndicator, IMelType> Types { get; set; }
        public Dictionary<Int32, MelObject> Objects { get; set; }
        public List<Context> Contexts { get; set; }
        public Context MainContext { get; set; }
        public Parser Parser { get; set; }
        
        public Interpreter()
        {
            this.Instructions = new Dictionary<OpCode, BaseInstruction>()
            {
                // Stack
                { OpCode.Push           , new Ins_Push       (this) },
                { OpCode.Pop            , new Ins_Pop        (this) },
                { OpCode.Swap           , new Ins_Swap       (this) },
                { OpCode.Dup            , new Ins_Dup        (this) },
                // Arithmetic
                { OpCode.Add            , new Ins_Add        (this) },
                { OpCode.Sub            , new Ins_Sub        (this) },
                { OpCode.Div            , new Ins_Div        (this) },
                { OpCode.Mul            , new Ins_Mul        (this) },
                { OpCode.Rem            , new Ins_Rem        (this) },
                // Subroutines and Jumping
                { OpCode.Def            , new Ins_Def        (this) },
                { OpCode.Jmp            , new Ins_Jmp        (this) },
                { OpCode.Call           , new Ins_Call       (this) },
                { OpCode.Ret            , new Ins_Ret        (this) },
                // Objects
                { OpCode.New            , new Ins_New        (this) },
                { OpCode.Fetch          , new Ins_Fetch      (this) },
                { OpCode.Store          , new Ins_Store      (this) },
                { OpCode.Delete         , new Ins_Delete     (this) },
            };
            this.Types        = new Dictionary<TypeIndicator, IMelType>()
            {
                { TypeIndicator.Int8    , new MelInt8        ()     },
                { TypeIndicator.Int16   , new MelInt16       ()     },
                { TypeIndicator.Int32   , new MelInt32       ()     },
                { TypeIndicator.Int64   , new MelInt64       ()     },
                { TypeIndicator.UInt64  , new MelUInt64      ()     },
                { TypeIndicator.Single  , new MelSingle      ()     },
                { TypeIndicator.Double  , new MelDouble      ()     },
                { TypeIndicator.Boolean , new MelBoolean     ()     },
                { TypeIndicator.Object  , new MelObject      ()     },
                { TypeIndicator.Array   , new MelObject      ()     },
                { TypeIndicator.String  , new MelString      ()     },
                { TypeIndicator.Pointer , new MelPointer     ()     },
                { TypeIndicator.Func    , new MelFunc        ()     },
            };
            this.Objects      = new Dictionary<Int32, MelObject>();
            this.Contexts     = new List<Context>();
            this.MainContext  = new Context(this);
            this.Parser       = new Parser(this.MainContext);
        }
        
        public Interpreter(RuntimeConfiguration rtc) : this()
        {
            
        }
        
        public void Parse(String lines)
        {
            this.Parser.ContextParse(lines);
        }
        
        public void Run(String lines)
        {
            this.Parse(lines);
            this.Run();
        }
        
        public void Run()
        {
            this.MainContext.Run();
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
