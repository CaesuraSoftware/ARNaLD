
using System;

namespace Caesura.Standard
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    public static class CollectionsExtensions
    {
        
        /// <summary>
        /// Start all tasks in a collection, but do not wait for them.
        /// </summary>
        /// <param name="tasks"></param>
        public static void StartAll(IEnumerable<Task> tasks)
        {
            foreach (var task in tasks)
            {
                try
                {
                    task.Start();
                }
                catch (ObjectDisposedException)
                {
                    // do nothing
                }
                catch (InvalidOperationException)
                {
                    // do nothing
                }
            }
        }
    }
}
