
using System;

namespace Caesura.Standard.Scripting.Tests.Melanie.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using Caesura.Standard.Scripting.Melanie.AsmParser;
    using Caesura.Standard.Scripting.Melanie.Runtime;
    using Caesura.Standard.Scripting.Melanie.Runtime.Types;
    using Caesura.Standard.Scripting.Melanie.Runtime.Instructions;
    using Xunit;
    using Xunit.Abstractions;
    
    public class ParserTest1
    {
        protected ITestOutputHelper TestOutput { get; }
        
        public ParserTest1(ITestOutputHelper output)
        {
            this.TestOutput = output;
        }
        
        public void WriteLine(String str)
        {
            Console.WriteLine(str);
            this.TestOutput?.WriteLine(str);
        }
        
        [Fact]
        public void AddTest1()
        {
            var interp = new Interpreter();
            interp.Run(@"
            001: PUSH 1_000
            002: PUSH 43
            003: ADD
            ");
            var rm = interp.MainContext.Stack.Peek();
            var r = rm.Value as MelInt32;
            Assert.True(r.InternalRepresentation == 1_043);
        }
        
        // FIXME: negative numbers don't work
        [Fact]
        public void AddTest2()
        {
            var interp = new Interpreter();
            interp.Run(@"
            001: PUSH 1_050
            002: PUSH -40
            003: ADD
            ");
            var rm = interp.MainContext.Stack.Peek();
            var r = rm.Value as MelInt32;
            Assert.True(r.InternalRepresentation == 1_010);
        }
        
        [Fact]
        public void StrTest1()
        {
            var interp = new Interpreter();
            interp.Run(@"
            001: PUSH ""Hello, \\ \"" world!""
            ");
            var rm = interp.MainContext.Stack.Peek();
            var r = rm.Value as MelString;
            Assert.True(r.InternalRepresentation == "Hello, \\ \" world!");
        }
        
        [Fact]
        public void JumpTest1()
        {
            var interp = new Interpreter();
            interp.Run(@"
            001: PUSH 1_000
            002: PUSH 2_000
            003: JMP 6
            004: PUSH 43
            005:
            006: ADD
            ");
            var rm = interp.MainContext.Stack.Peek();
            var r = rm.Value as MelInt32;
            Assert.True(r.InternalRepresentation == 3_000);
        }
        
        [Fact]
        public void JumpTest2()
        {
            var interp = new Interpreter();
            interp.Run(@"
            001: PUSH 1_000
            002: PUSH 2_000
            003: PUSH 6
            004: JMP *
            005: PUSH 43
            006:
            007: ADD
            ");
            var rm = interp.MainContext.Stack.Peek();
            var r = rm.Value as MelInt32;
            Assert.True(r.InternalRepresentation == 3_000);
        }
        
        [Fact]
        public void SyntaxTest1()
        {
            var interp = new Interpreter();
            interp.Run(@"
            _0010: PUSH 1_000 ; 1000
            _0020: PUSH 43 ;; 43
            __0030: ADD;test
            0045: PUSH 1_000;*test
            __0050: SUB ;test;;
            ");
            var rm = interp.MainContext.Stack.Peek();
            var r = rm.Value as MelInt32;
            Assert.True(r.InternalRepresentation == 43);
        }
        
        [Fact]
        public void OrderTest1()
        {
            var interp = new Interpreter();
            Assert.Throws(typeof(InvalidOperationException), () =>
            {
                interp.Run(@"
                001: PUSH 1_000
                002: PUSH 43
                006: ADD
                004: PUSH 1_000
                005: SUB
                ");
            });
        }
        
        [Fact]
        public void OrderTest2()
        {
            var interp = new Interpreter();
            Assert.Throws(typeof(ArgumentException), () =>
            {
                interp.Run(@"
                001: PUSH 1_000
                002: PUSH 43
                003: ADD
                003: PUSH 1_000
                005: SUB
                ");
            });
        }
        
        [Fact]
        public void BadInstructionTest1()
        {
            var interp = new Interpreter();
            Assert.Throws(typeof(InvalidOperationException), () =>
            {
                interp.Run(@"
                001: PUSH 1_000
                002: PUSH 43
                003: ADD
                004: PUSH 1_000
                005: SUB
                006: SUB
                ");
            });
        }
        
        [Fact]
        public void FuncTest1()
        {
            var interp = new Interpreter();
            interp.Run(@"
            001: JMP 70 ; Start of program
            
            ; Mul(x,y)
            020: MUL
            030: RET
            
            ; Sub(x,y)
            040: SUB
            050: RET
            
            ; Main
            070: PUSH 3
            080: PUSH 20
            090: CALL 20 ; Mul
            100: PUSH 10
            110: CALL 40 ; Sub
            120: RET
            
            ");
            var rm = interp.MainContext.Stack.Peek();
            var r = rm.Value as MelInt32;
            Assert.True(r.InternalRepresentation == 50);
        }
        
        [Fact]
        public void FuncTest2()
        {
            var interp = new Interpreter();
            interp.MainContext.ExternalCallSites.Add(new ExtCallSite()
            {
                Name = "Console.WriteLine(String)",
                Caller = (context) => 
                {
                    var pop = context.Environment.Instructions[OpCode.Pop];
                    pop.Execute(context);
                    var marg = context.PopArgument();
                    var arg = marg.Value as MelString;
                    this.WriteLine(arg.InternalRepresentation);
                },
            });
            interp.Run(@"
            
            010: PUSH ""Hello, world! From Melanie!""
            020: CALL [Console.WriteLine(String)]
            030: RET
            
            ");
        }
        
        [Fact]
        public void ObjectTest1()
        {
            var interp = new Interpreter();
            interp.MainContext.ExternalCallSites.Add(new ExtCallSite()
            {
                Name = "Console.WriteLine(String/Int32)",
                Caller = (context) => 
                {
                    var pop = context.Environment.Instructions[OpCode.Pop];
                    pop.Execute(context);
                    var marg = context.PopArgument();
                    /**/ if (marg.Value is MelString ms)
                    {
                        this.WriteLine(ms.InternalRepresentation);
                    }
                    else if (marg.Value is MelInt32 m32)
                    {
                        this.WriteLine(m32.InternalRepresentation.ToString());
                    }
                },
            });
            interp.Run(@"
            
            010: NEW 0
            020: PUSH ""Age""
            030: PUSH 10
            040: STORE 0
            050: PUSH ""Age""
            060: FETCH 0
            070: PUSH 1
            080: ADD
            090: PUSH ""Age""
            100: SWAP
            110: STORE 0
            120: PUSH ""Age""
            130: FETCH 0
            140: CALL [Console.WriteLine(String/Int32)]
            150: DELETE 0
            
            ");
        }
        
        [Fact]
        public void ObjectTest2()
        {
            var interp = new Interpreter();
            interp.MainContext.ExternalCallSites.Add(new ExtCallSite()
            {
                Name = "Console.WriteLine(String/Int32)",
                Caller = (context) => 
                {
                    var pop = context.Environment.Instructions[OpCode.Pop];
                    pop.Execute(context);
                    var marg = context.PopArgument();
                    /**/ if (marg.Value is MelString ms)
                    {
                        this.WriteLine(ms.InternalRepresentation);
                    }
                    else if (marg.Value is MelInt32 m32)
                    {
                        this.WriteLine(m32.InternalRepresentation.ToString());
                    }
                },
            });
            interp.Run(@"
            
            0001: JMP 1000      ; Go to main
            
            ;; Object Constructor
            0200: DUP           ; Duplicate object ID
            0210: NEW *         ; Create a new object with ID
            0220: PUSH ""Age""  ; Push Age
            0230: SWAP          ; Swap so Age is above ID
            0240: PUSH 10       
            0250: SWAP          ; Swap so 10 is above ID
            0260: STORE *       ; Take ID (object), key (Age), and value (10)
            0270: RET
            
            ;; Age-Up
            0300: DUP           ; Duplicate Object ID
            0310: PUSH ""Age""
            0320: SWAP          ; Swap Age and Object ID
            0330: FETCH *       ; Get value of Age
            0340: PUSH 1
            0350: ADD           : Age + 1
            0360: SWAP          ; ID is below Age's value
            0370: PUSH ""Age""
            0380: SWAP          ; ID is below Age (is below Age's value)
            0390: SWAP 1        ; swap Age and Age's value
            0400: STORE *
            0410: RET
            
            ;; Print Age
            0500: PUSH ""Age""
            0510: SWAP
            0520: FETCH *
            0530: CALL [Console.WriteLine(String/Int32)]
            0540: RET
            
            ;; Main
            1000:
            1010: PUSH 1
            1011: PUSH 5
            1012: DUP *         ; Duplicate the object's ID for the other methods
            1020: CALL 0200     ; Construct new object
            1030: CALL 0300     ; Age up 1
            1040: CALL 0500     ; Print age
            1050: CALL 0300
            1060: CALL 0300     ; Age up twice
            1070: CALL 500      ; Print again
            1080: RET
            
            ");
        }
    }
}
