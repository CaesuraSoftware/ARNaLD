
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    
    [Flags]
    public enum AgentAutonomy : Int32
    {
        None                =      0,
        IndependentThread   = 1 << 0,
        SimulateCycle       = 1 << 1,
    }
}
