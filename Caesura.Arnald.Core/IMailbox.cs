
using System;

namespace Caesura.Arnald.Core
{
    using System.Collections.Generic;
    using Caesura.Standard;
    
    public interface IMailbox
    {
        void Send(IMessage message);
        void Send(IEnumerable<IMessage> messages);
        Maybe<IMessage> Peek();
        Maybe<IMessage> Receive();
        IEnumerable<IMessage> ReceiveAll();
        IEnumerable<IMessage> PeekAll();
    }
}
