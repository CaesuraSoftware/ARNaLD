
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
    
    public class ParserTest1
    {
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
        public void AddTest2()
        {
            var interp = new Interpreter();
            interp.Run(@"
            _0010: PUSH 1_000 ; 1000
            _0020: PUSH 43 ;; 43
            __0030: ADD;test
            0045: PUSH 1_000;test
            __0050: SUB
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
    }
}
