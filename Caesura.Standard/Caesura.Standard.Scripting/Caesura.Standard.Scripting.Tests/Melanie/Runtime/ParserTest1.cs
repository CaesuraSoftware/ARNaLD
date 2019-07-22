
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
        
        // TODO: object implementation:
        // NEW 0                ; create new object, not on the stack, with ID 0
        // PUSH "Age"           ; field name
        // PUSH 10              ; field value
        // STORE 0              ; pop two arguments (key/value) and put them in this object
        // ;...
        // FETCH 0 "Age"        ; push the value of "Age" (10) on the stack (still contained in Object 0)
        // PUSH 1
        // ADD                  ; Add 1 to age (11)
        // PUSH "Age"           ; field name
        // SWAP                 ; swap key/value
        // STORE 0              ; store 11 in "Age", overwriting 10
    }
}
