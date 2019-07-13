
using System;

namespace Caesura.Arnald.Core.Agents
{
    
    [Flags]
    public enum MessageResolverResult : Int32
    {
        None            =      0,
        Pass            = 1 << 0,
        Stop            = 1 << 1,
        StopAsync       = 1 << 2,
        Continue        = 1 << 3,
        ContinueAsync   = 1 << 4,
    }
}
