
using System;

namespace Caesura.Arnald.Core
{
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using Caesura.Standard;
    
    public class Mailbox : IMailbox
    {
        private readonly Object indexLock = new Object();
        private ConcurrentQueue<IMessage> _inbox;
        
        public void Send(IMessage message)
        {
            this._inbox.Enqueue(message);
        }
        
        public Maybe<IMessage> Receive()
        {
            var success = this._inbox.TryDequeue(out var msg);
            return success ? Maybe<IMessage>.Some(msg) : Maybe<IMessage>.None;
        }
        
        public IEnumerable<IMessage> ReceiveAll()
        {
            lock (indexLock)
            {
                var msgs = this._inbox.ToList();
                this._inbox = new ConcurrentQueue<IMessage>();
                return msgs;
            }
        }
    }
}
