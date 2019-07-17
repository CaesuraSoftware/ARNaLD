
using System;

namespace Caesura.Arnald.Core.Plugin
{
    [Flags]
    public enum PluginKind : Int32
    {
        None            =      0,
        General         = 1 << 0,
        Behavior        = 1 << 1,
        Scripting       = 1 << 2,
        Communication   = 1 << 3,
        Storage         = 1 << 4,
    }
}
