
using System;

namespace Caesura.Arnald.Core.Agents
{
    
    [Flags]
    public enum MessageResolverResult : Int32
    {
        None            =      0,
        Stop            = 1 << 0,
        StopAsync       = 1 << 1,
        Continue        = 1 << 2,
        ContinueAsync   = 1 << 3,
    }
}
