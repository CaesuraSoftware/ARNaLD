
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using Types;
    
    public class Context
    {
        public Interpreter Environment { get; set; }
        public Stack Stack { get; set; }
        public Stack Arguments { get; set; }
        
        public Context()
        {
            this.Stack = new Stack();
            this.Arguments = new Stack(3);
        }
        
        public Context(Interpreter handle) : this()
        {
            this.Environment = handle;
        }
        
        public void PushArgument(IMelType item)
        {
            this.Arguments.Push(item);
        }
        
        public IMelType PopArgument()
        {
            return this.Arguments.Pop();
        }
        
        public void Push(IMelType item)
        {
            this.Stack.Push(item);
        }
        
        public IMelType Pop()
        {
            return this.Stack.Pop();
        }
    }
}
