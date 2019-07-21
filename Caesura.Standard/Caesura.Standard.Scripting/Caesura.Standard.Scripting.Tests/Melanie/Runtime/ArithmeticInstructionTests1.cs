
using System;

namespace Caesura.Standard.Scripting.Tests.Melanie.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using Caesura.Standard.Scripting.Melanie.Runtime;
    using Caesura.Standard.Scripting.Melanie.Runtime.Types;
    using Caesura.Standard.Scripting.Melanie.Runtime.Instructions;
    using Xunit;
    
    public class ArithmeticInstructionTest1
    {
        [Fact]
        public void AddTest1()
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
        
        [Fact]
        public void SubTest1()
        {
            var interp = new Interpreter();
            var a = new MelInt32(6);
            var b = new MelInt32(2);
            interp.ParseInstruction(OpCode.Push, a);
            interp.ParseInstruction(OpCode.Push, b);
            interp.ParseInstruction(OpCode.Sub);
            var rm = interp.MainContext.Stack.Peek();
            var r = rm.Value as MelInt32;
            Assert.True(r.InternalRepresentation == 4);
        }
        
        [Fact]
        public void MulTest1()
        {
            var interp = new Interpreter();
            var a = new MelInt32(6);
            var b = new MelInt32(3);
            interp.ParseInstruction(OpCode.Push, a);
            interp.ParseInstruction(OpCode.Push, b);
            interp.ParseInstruction(OpCode.Mul);
            var rm = interp.MainContext.Stack.Peek();
            var r = rm.Value as MelInt32;
            Assert.True(r.InternalRepresentation == 18);
        }
        
        [Fact]
        public void DivTest1()
        {
            var interp = new Interpreter();
            var a = new MelInt32(6);
            var b = new MelInt32(2);
            interp.ParseInstruction(OpCode.Push, a);
            interp.ParseInstruction(OpCode.Push, b);
            interp.ParseInstruction(OpCode.Div);
            var rm = interp.MainContext.Stack.Peek();
            var r = rm.Value as MelInt32;
            Assert.True(r.InternalRepresentation == 3);
        }
        
        [Fact]
        public void RemTest1()
        {
            var interp = new Interpreter();
            var a = new MelInt32(5);
            var b = new MelInt32(4);
            interp.ParseInstruction(OpCode.Push, a);
            interp.ParseInstruction(OpCode.Push, b);
            interp.ParseInstruction(OpCode.Rem);
            var rm = interp.MainContext.Stack.Peek();
            var r = rm.Value as MelInt32;
            Assert.True(r.InternalRepresentation == 1);
        }
    }
}
