
using System;

namespace Caesura.Arnald.Core.Signals
{
    using System.Collections.Generic;
    
    public interface ISignal
    {
        String Name { get; }
        String Namespace { get; }
        Version Version { get; }
        IDataContainer Data { get; }
    }
}
