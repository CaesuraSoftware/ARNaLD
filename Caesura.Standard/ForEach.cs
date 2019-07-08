
using System;

namespace Caesura.Standard
{
    using System.Collections.Generic;
    using System.Linq;
    
    [Flags]
    public enum ForEachResult : Int32
    {
        None        = 1 << 0,
        Success     = 1 << 1,
        Failure     = 1 << 2,
        Halt        = 1 << 3,
    }
    
    public delegate ForEachResult ForEachDelegate<T>(T element);
    public delegate ForEachResult ForEachIteratorDelegate<T>(T element, ref Int32 iterator);
    
    public static class ForEachExtension
    {
        public static void ForEach<T>(this IEnumerable<T> collection, ForEachDelegate<T> callback)
        {
            // TODO: change these to GetEnumerator?
            var size = collection.Count();
            for (var i = 0; i < size; i++)
            {
                var e = collection.ElementAt(i);
                var result = callback.Invoke(e);
                if (result.HasFlag(ForEachResult.Failure | ForEachResult.Halt))
                {
                    break;
                }
            }
        }
        
        public static void ForEach<T>(this IEnumerable<T> collection, ForEachIteratorDelegate<T> callback)
        {
            var size = collection.Count();
            for (var i = 0; i < size; i++)
            {
                var e = collection.ElementAt(i);
                var result = callback.Invoke(e, ref i);
                if (result.HasFlag(ForEachResult.Failure | ForEachResult.Halt))
                {
                    break;
                }
            }
        }
    }
}
