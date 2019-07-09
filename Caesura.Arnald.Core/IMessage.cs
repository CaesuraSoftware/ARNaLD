
using System;

namespace Caesura.Arnald.Core
{
    using System.Collections.Generic;
    
    public interface IMessage
    {
        String Sender { get; set; }
        String Recipient { get; set; }
        String Message { get; set; }
        Dictionary<String, Object> Items { get; set; }
        T Get<T>(String name);
        Boolean TryGet<T>(String name, out T item);
        Boolean Set<T>(String name, T item);
        Boolean Set<T>(String name, T item, Boolean force);
        void Copy(IMessage msg);
    }
}