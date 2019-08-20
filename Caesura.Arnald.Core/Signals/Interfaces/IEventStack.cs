
using System;

namespace Caesura.Arnald.Core.Signals
{
    using System.Collections.Generic;
    
    [Flags]
    public enum EventStackInsertOptions
    {
        None                =      0,
        InsertBefore        = 1 << 0,
        InsertAfter         = 1 << 1,
        InsertAtEarliest    = 1 << 2,
        InsertAtLatest      = 1 << 3,
    }
    
    public interface IEventStack
    {
        Int32 Index { get; set; }
        Int32 Count { get; }
        Boolean Repeat { get; set; }
        String this[Int32 index] { get; set; }
        
        void SetStack(IEnumerable<String> stack);
        void Push(String name);
        String Next();
        String Peek();
        void Reset();
        void Swap();
        void BlockNext();
        void Replace(String item, String where, EventStackInsertOptions options);
        void Insert(String item, String where);
        void Insert(String item, String where, EventStackInsertOptions options);
    }
}
