
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
        public Int32 Count => this._inbox.Count;
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
        
        public Maybe<IMessage> TryReceive()
        {
            var success = this._inbox.TryTake(out var msg);
            if (success)
            {
                return Maybe<IMessage>.Some(msg);
            }
            return Maybe.None;
        }
        
        public void Dispose()
        {
            this._inbox.Dispose();
        }
    }
}
