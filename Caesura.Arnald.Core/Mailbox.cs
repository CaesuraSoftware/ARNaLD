
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
        private Queue<IMessage> _inbox;
        
        public Mailbox()
        {
            this._inbox = new Queue<IMessage>();
        }
        
        public Mailbox(IMailbox mailbox)
        {
            this.Copy(mailbox);
        }
        
        public void Send(IMessage message)
        {
            lock (this.indexLock)
            {
                this._inbox.Enqueue(message);
            }
        }
        
        public void Send(IEnumerable<IMessage> messages)
        {
            lock (this.indexLock)
            {
                foreach (var message in messages)
                {
                    this._inbox.Enqueue(message);
                }
            }
        }
        
        public Maybe<IMessage> Peek()
        {
            lock (this.indexLock)
            {
                if (this._inbox.Count > 0)
                {
                    var msg = this._inbox.Peek();
                    return Maybe<IMessage>.Some(msg);
                }
            }
            return Maybe.None;
        }
        
        public Maybe<IMessage> Receive()
        {
            lock (this.indexLock)
            {
                var mmsg = this.Peek();
                if (mmsg.HasValue)
                {
                    this._inbox.Dequeue();
                    return mmsg;
                }
                return mmsg;
            }
        }
        
        public IEnumerable<IMessage> ReceiveAll()
        {
            lock (this.indexLock)
            {
                var msgs = this._inbox.ToList();
                this._inbox.Clear();
                return msgs;
            }
        }
        
        public IEnumerable<IMessage> PeekAll()
        {
            lock (this.indexLock)
            {
                var msgs = this._inbox.ToList();
                return msgs;
            }
        }
        
        public void Copy(IMailbox mailbox)
        {
            if (mailbox is Mailbox mb)
            {
                this._inbox = mb._inbox ?? new Queue<IMessage>();
            }
            else
            {
                this._inbox = new Queue<IMessage>(mailbox.PeekAll());
            }
        }
    }
}
