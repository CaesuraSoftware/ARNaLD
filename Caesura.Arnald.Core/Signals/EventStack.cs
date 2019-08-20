
using System;

namespace Caesura.Arnald.Core.Signals
{
    using System.Collections.Generic;
    using System.Linq;
    using Caesura.Standard;
    
    public class EventStack : IEventStack
    {
        public Int32 Index { get; set; }
        public Int32 Count => this.Stack.Count;
        public Boolean Repeat { get; set; }
        private List<String> Stack { get; set; }
        
        public EventStack()
        {
            this.Stack = new List<String>();
            this.Repeat = false;
            this.Reset();
        }
        
        public String this[Int32 index] 
        {
            get => this.Stack[index];
            set => this.Stack[index] = value;
        }
        
        public void SetStack(IEnumerable<String> stack)
        {
            this.Stack = new List<String>(stack);
        }
        
        public void Push(String name)
        {
            this.Stack.Add(name);
        }
        
        public String Next()
        {
            var item = this.Peek();
            this.Index++;
            if (this.Index >= this.Count)
            {
                if (this.Repeat)
                {
                    this.Reset();
                }
                else
                {
                    this.Index = -1;
                }
            }
            return item;
        }
        
        public String Peek()
        {
            return this.Stack.ElementAt(this.Index);
        }
        
        public void Reset()
        {
            this.Index = 0;
        }
        
        public void Swap()
        {
            if (this.Count < 2)
            {
                throw new InvalidOperationException("Stack must contain at least two elements to swap.");
            }
            var item1 = this.Stack.ElementAt(this.Stack.Count - 1);
            var item2 = this.Stack.ElementAt(this.Stack.Count - 2);
            this.Stack[this.Stack.Count - 1] = item2;
            this.Stack[this.Stack.Count - 2] = item1;
        }
        
        public void BlockNext()
        {
            throw new NotImplementedException();
        }
        
        // TODO: replace/insert for specific indexes? foreach that could loop
        // through the stack, finding each instance of "where" until it found
        // the number the user requested, then get it's index.
        
        public void Replace(String item, String where, EventStackInsertOptions options)
        {
            var index = 0;
            if (options.HasFlag(EventStackInsertOptions.InsertAtLatest))
            {
                index = this.Stack.LastIndexOf(where);
            }
            else
            {
                index = this.Stack.IndexOf(where);
            }
            
            if (index == -1)
            {
                throw new ElementNotFoundException(where);
            }
            
            this.Stack[index] = item;
        }
        
        public void Insert(String item, String where)
        {
            this.Insert(item, where, EventStackInsertOptions.InsertBefore | EventStackInsertOptions.InsertAtLatest);
        }
        
        public void Insert(String item, String where, EventStackInsertOptions options)
        {
            var index = this.Stack.LastIndexOf(where);
            if (options.HasFlag(EventStackInsertOptions.InsertAtEarliest))
            {
                index = this.Stack.IndexOf(where);
            }
            if (index == -1)
            {
                throw new ElementNotFoundException(where);
            }
            
            if (options.HasFlag(EventStackInsertOptions.InsertAfter))
            {
                if (index + 1 == this.Count)
                {
                    this.Push(item);
                    return;
                }
                this.Stack.Insert(index + 1, item);
            }
            else
            {
                this.Stack.Insert(index, item);
            }
        }
    }
}
