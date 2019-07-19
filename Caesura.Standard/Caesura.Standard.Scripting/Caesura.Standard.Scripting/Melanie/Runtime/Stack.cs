
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using Types;
    
    public class Stack
    {
        public Int32 Count => this.MainStack.Count;
        public List<IMelType> MainStack { get; set; }
        
        public Stack()
        {
            this.MainStack = new List<IMelType>();
        }
        
        public Stack(Int32 count)
        {
            this.MainStack = new List<IMelType>(count);
        }
        
        public void Push(IMelType item)
        {
            this.MainStack.Add(item);
        }
        
        public IMelType Pop()
        {
            var index = this.MainStack.Count - 1;
            var item = this.MainStack.ElementAt(index);
            this.MainStack.RemoveAt(index);
            return item;
        }
        
        public IMelType Peek()
        {
            if (this.Count == 0)
            {
                return null;
            }
            
            var index = this.MainStack.Count - 1;
            var item = this.MainStack.ElementAt(index);
            return item;
        }
        
        public void Clear()
        {
            this.MainStack.Clear();
        }
        
        public void Swap()
        {
            var item1 = this.Pop();
            var item2 = this.Pop();
            this.Push(item1);
            this.Push(item2);
        }
        
        public void Dup()
        {
            var item = this.Pop();
            this.Push(item);
            this.Push(item);
        }
    }
}
