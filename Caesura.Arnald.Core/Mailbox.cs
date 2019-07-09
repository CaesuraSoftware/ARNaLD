
using System;

namespace Caesura.Arnald.Core
{
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Linq;
    
    public class Mailbox
    {
        private ConcurrentQueue<IMessage> Inbox { get; set; }
        
    }
}
