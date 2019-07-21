
using System;

namespace Caesura.Standard.Scripting.Tests.Melanie.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using Caesura.Standard.Scripting.Melanie.Runtime;
    using Caesura.Standard.Scripting.Melanie.Runtime.Types;
    using Caesura.Standard.Scripting.Melanie.Runtime.Instructions;
    using Xunit;
    
    public class RuntimeTest1
    {
        [Fact]
        public void Test1()
        {
            var interp = new Interpreter();
            var a = new MelInt32(5);
            var b = new MelInt32(6);
            interp.ParseInstruction(OpCode.Push, a);
            interp.ParseInstruction(OpCode.Push, b);
            interp.ParseInstruction(OpCode.Add);
            var rm = interp.MainContext.Stack.Peek();
            var r = rm.Value as MelInt32;
            Assert.True(r.InternalRepresentation == 11);
        }
    }
}
