
using System;

namespace Caesura.Arnald.Core.Signals
{
    using System.Collections.Generic;
    using Caesura.Standard;
    
    public interface ISignal : ICopyable
    {
        String Name { get; }
        String Namespace { get; }
        Version Version { get; }
        IDataContainer Data { get; }
    }
}
