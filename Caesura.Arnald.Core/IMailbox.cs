
using System;

namespace Caesura.Arnald.Core
{
    using System.Collections.Generic;
    using System.Threading;
    using Caesura.Standard;
    
    public interface IMailbox : IDisposable
    {
        Boolean IsAddingCompleted { get; }
        Boolean TrySend(IMessage message);
        void Send(IMessage message);
        void WaitSend(IMessage message, CancellationToken token);
        IMessage Receive();
        IMessage Receive(CancellationToken token);
        Maybe<IMessage> TryReceive();
    }
}
