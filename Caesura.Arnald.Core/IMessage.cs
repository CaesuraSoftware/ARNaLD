
using System;

namespace Caesura.Arnald.Core
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Caesura.Standard;
    
    public interface IMessage<T> : IMessage
    {
        T Data { get; set; }
    }
    
    public interface IMessage
    {
        String Sender { get; set; }
        String Recipient { get; set; }
        String Information { get; set; }
        Dictionary<String, Object> Items { get; set; }
        T Get<T>(String name);
        Maybe<T> TryGet<T>(String name);
        Boolean Set<T>(String name, T item);
        Boolean Set<T>(String name, T item, Boolean force);
        IMessage SwapSender();
        IMessage CreateResponse(String sender);
        void Copy(IMessage msg);
    }
}
