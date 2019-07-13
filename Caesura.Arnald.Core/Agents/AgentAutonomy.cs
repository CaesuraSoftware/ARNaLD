
using System;

namespace Caesura.Arnald.Core.Agents
{
    
    [Flags]
    public enum AgentAutonomy : Int32
    {
        None                =      0,
        IndependentThread   = 1 << 0,
        SimulateCycle       = 1 << 1,
    }
}
