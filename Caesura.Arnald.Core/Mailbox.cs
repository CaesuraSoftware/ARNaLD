
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
        public Boolean IsAddingCompleted => this._inbox.IsAddingCompleted;
        private BlockingCollection<IMessage> _inbox;
        
        public Mailbox()
        {
            this._inbox = new BlockingCollection<IMessage>();
        }
        
        public Mailbox(Int32 capacity)
        {
            this._inbox = new BlockingCollection<IMessage>(capacity);
        }
        
        public Boolean TrySend(IMessage message)
        {
            var success = this._inbox.TryAdd(message);
            return success;
        }
        
        public void Send(IMessage message)
        {
            var success = this.TrySend(message);
            if (!success)
            {
                throw new InvalidOperationException($"{nameof(Mailbox)} is full.");
            }
        }
        
        public void WaitSend(IMessage message, CancellationToken token)
        {
            this._inbox.Add(message, token);
        }
        
        public IMessage Receive()
        {
            return this._inbox.Take();
        }
        
        public IMessage Receive(CancellationToken token)
        {
            return this._inbox.Take(token);
        }
        
        public IEnumerable<IMessage> ReceiveAll()
        {
            var bc = this._inbox;
            this._inbox = new BlockingCollection<IMessage>();
            var array = bc.ToArray();
            bc.Dispose();
            return array;
        }
        
        public void Dispose()
        {
            this._inbox.Dispose();
        }
    }
}
