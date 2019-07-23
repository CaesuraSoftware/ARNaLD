
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
            
            ;; Object Constructor
            0200: DEF ""Child..ctor""
            0210: DUP           ; Duplicate object ID
            0220: NEW *         ; Create a new object with ID
            0230: PUSH ""Age""  ; Push Age
            0240: SWAP          ; Swap so Age is above ID
            0250: PUSH 10       
            0260: SWAP          ; Swap so 10 is above ID
            0270: STORE *       ; Take ID (object), key (Age), and value (10)
            0280: RET
            
            ;; Age-Up
            0300: DEF ""Child.Age (this)""
            0310: DUP           ; Duplicate Object ID
            0320: PUSH ""Age""
            0330: SWAP          ; Swap Age and Object ID
            0340: FETCH *       ; Get value of Age
            0350: PUSH 1
            0360: ADD           : Age + 1
            0370: SWAP          ; ID is below Age's value
            0380: PUSH ""Age""
            0390: SWAP          ; ID is below Age (is below Age's value)
            0400: SWAP 1        ; swap Age and Age's value
            0410: STORE *
            0420: RET
            
            ;; Print Age
            0500: DEF ""Child.Print (this)""
            0510: PUSH ""Age""
            0520: SWAP
            0530: FETCH *
            0540: CALL [Console.WriteLine(String/Int32)]
            0550: RET
            
            ;; Main
            1000: DEF ""Main""
            1010: PUSH 1
            1011: PUSH 5
            1012: DUP *         ; Duplicate the object's ID for the other methods
            1020: CALL ""Child..ctor""
            1030: CALL ""Child.Age   (this)""
            1040: CALL ""Child.Print (this)""
            1050: CALL ""Child.Age   (this)""
            1060: CALL ""Child.Age   (this)""
            1070: CALL ""Child.Print (this)""
            1080: RET
            
            ");
        }
    }
}
