
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
        
        public Maybe<IMelType> Pop()
        {
            if (this.Count == 0)
            {
                throw new ElementNotFoundException();
            }
            
            var index = this.MainStack.Count - 1;
            var item = this.MainStack.ElementAt(index);
            this.MainStack.RemoveAt(index);
            return Maybe<IMelType>.Some(item);
        }
        
        public Maybe<IMelType> Peek(Int32 index)
        {
            if (this.Count == 0 || index >= this.Count)
            {
                return Maybe.None;
            }
            
            var item = this.MainStack.ElementAt(index);
            return Maybe<IMelType>.Some(item);
        }
        
        public Maybe<IMelType> Peek()
        {
            if (this.Count == 0)
            {
                return Maybe.None;
            }
            
            var index = this.MainStack.Count - 1;
            var item = this.MainStack.ElementAt(index);
            return Maybe<IMelType>.Some(item);
        }
        
        public void Clear()
        {
            this.MainStack.Clear();
        }
        
        public void Swap()
        {
            var item1 = this.Pop();
            var item2 = this.Pop();
            this.Push(item1.Value);
            this.Push(item2.Value);
        }
        
        public void Swap(Int32 index1, Int32 index2)
        {
            var item1 = this.MainStack[index1];
            var item2 = this.MainStack[index2];
            this.MainStack[index2] = item1;
            this.MainStack[index1] = item2;
        }
        
        public void Swap(Int32 index)
        {
            this.Swap(index, index - 1);
        }
        
        public void Dup()
        {
            var item = this.Pop();
            this.Push(item.Value);
            this.Push(item.Value);
        }
        
        public void Dup(Int32 times)
        {
            if (times <= 0)
            {
                throw new ArgumentException();
            }
            for (var i = 0; i < times; i++)
            {
                this.Dup();
            }
        }
    }
}
