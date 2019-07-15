
using System;

namespace Caesura.PerformanceMonitor.Display
{
    using System.Collections.Generic;
    using System.Linq;
    
    public enum RequestProgramState : Int32
    {
        None            =      0,
        Continue        = 1 << 0,
        Exit            = 1 << 1,
        TextInput       = 1 << 2,
        CommandMode     = 1 << 3,
        CommandInput    = 1 << 4,
        EditMode        = 1 << 5,
    }
}
