
using System;

namespace Caesura.Arnald.Core.Agents
{
    
    [Flags]
    public enum MessageResolverResult : Int32
    {
        None            =      0,
        Stop            = 1 << 0,
        Continue        = 1 << 1,
        ContinueAsync   = 1 << 2,
    }
}
