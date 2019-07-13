
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    
    [Flags]
    public enum MessageResolverResult : Int32
    {
        None            =      0,
        Stop            = 1 << 1,
        Continue        = 1 << 2,
        ContinueAsync   = 1 << 3,
    }
}
