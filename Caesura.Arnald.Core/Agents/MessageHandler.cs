
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    
    public class MessageHandler : IMessageHandler
    {
        public IAgent Owner { get; set; }
        private List<IMessageResolver> Resolvers { get; set; }
        
        public MessageHandler()
        {
            this.Resolvers = new List<IMessageResolver>();
        }
        
        public void Process(IMessage message)
        {
            Boolean execAsync = true;
            var resolvers = new List<IMessageResolver>();
            
            foreach (var resolver in this.Resolvers)
            {
                resolvers.Add(resolver);
                
                var result = resolver.Check(message);
                if (result.HasFlag(MessageResolverResult.Stop))
                {
                    break;
                }
                if (result.HasFlag(MessageResolverResult.Continue))
                {
                    // if a resolver returns Continue instead of ContinueAsync,
                    // then the agreement between all active resolvers to work
                    // in parallel is broken and they will all be processed
                    // syncronously.
                    execAsync = false;
                }
            }
            
            this.Execute(resolvers, execAsync);
        }
        
        private void Execute(List<IMessageResolver> resolvers, Boolean execAsync)
        {
            if (execAsync) // execute all resolvers in parallel
            {
                if (resolvers.Count == 1) // no need to do parallel on one resolver
                {
                    resolvers.First().Execute();
                }
                else
                {
                    var exceptions = new ConcurrentQueue<Exception>();
                    Parallel.ForEach(resolvers, (resolver) =>
                    {
                        try
                        {
                            resolver.Execute();
                        }
                        catch (Exception e)
                        {
                            exceptions.Enqueue(e);
                        }
                    });
                    
                    if (exceptions.Count > 0) 
                    {
                        throw new AggregateException(exceptions);
                    }
                }
            }
            else // execute resolvers in sequence
            {
                foreach (var resolver in resolvers)
                {
                    resolver.Execute();
                }
            }
        }
    }
}
