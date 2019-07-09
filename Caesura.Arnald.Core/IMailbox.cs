
using System;

namespace Caesura.Arnald.Core
{
    using System.Collections.Generic;
    using Caesura.Standard;
    
    public interface IMailbox
    {
        void Send(IMessage message);
        Maybe<IMessage> Receive();
        IEnumerable<IMessage> ReceiveAll();
    }
}
