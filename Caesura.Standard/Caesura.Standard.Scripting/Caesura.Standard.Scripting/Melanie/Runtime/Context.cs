
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using Types;
    
    public class Context
    {
        public Stack Stack { get; set; }
        public List<IMelType> Arguments { get; set; }
        
        public Context()
        {
            this.Stack = new Stack();
            this.Arguments = new List<IMelType>();
        }
        
        public Context(Interpreter handle) : this()
        {
            
        }
        
        public void PushArgument(IMelType item)
        {
            this.Arguments.Add(item);
        }
        
        public IMelType PopArgument()
        {
            if (this.Arguments.Count == 0)
            {
                throw new InvalidOperationException("Arguments are empty");
            }
            
            var index = this.Arguments.Count - 1;
            var item = this.Arguments.ElementAt(index);
            this.Arguments.RemoveAt(index);
            return item;
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
