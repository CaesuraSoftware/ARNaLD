
using System;

namespace Caesura.Standard
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    [Flags]
    public enum MapResult : Int32
    {
        None        =      0,
        Success     = 1 << 0,
        Failure     = 1 << 1,
        Halt        = 1 << 2,
    }
    
    public delegate MapResult MapCallback<T>(T element);
    public delegate MapResult MapIteratorCallback<T>(T element, ref Int32 iterator);
    
    public static class InteratorExtensions
    {
        public static void Map<T>(this IEnumerable<T> collection, Action<T> callback)
        {
            collection.Map<T>(item =>
            {
                callback.Invoke(item);
                return MapResult.Success;
            });
        }
        
        public static void Map<T>(this IEnumerable<T> collection, MapCallback<T> callback)
        {
            var enumerator = collection.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    var result = callback.Invoke(current);
                    if (result.HasFlag(MapResult.Failure | MapResult.Halt))
                    {
                        break;
                    }
                }
            }
            finally
            {
                enumerator?.Dispose();
            }
        }
        
        public static void Map<T>(this IEnumerable<T> collection, MapIteratorCallback<T> callback)
        {
            var size = collection.Count();
            for (var i = 0; i < size; i++)
            {
                var e = collection.ElementAt(i);
                var result = callback.Invoke(e, ref i);
                if (result.HasFlag(MapResult.Failure | MapResult.Halt))
                {
                    break;
                }
            }
        }
        
        /// <summary>
        /// Run a callback on all elements of a collection in parallel. If any of the
        /// calls to the callback throw an exception, they are added to an AggregateException
        /// and thrown at the end of execution.
        /// No parallelization will occur if the collection contains 1 or less items.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="callback"></param>
        /// <typeparam name="T"></typeparam>
        public static void ParallelMap<T>(this IEnumerable<T> collection, Action<T> callback)
        {
            if (collection.Count() == 0)
            {
                return;
            }
            if (collection.Count() == 1)
            {
                var item = collection.First();
                callback.Invoke(item);
                return;
            }
            
            var exceptions = new List<Exception>();
            Parallel.ForEach(collection, (item) =>
            {
                try
                {
                    callback.Invoke(item);
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            });
            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }
    }
}
