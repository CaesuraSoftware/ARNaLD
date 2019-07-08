
using System;

namespace Caesura.Arnald.Core.Plugin
{
    public enum PluginKind : Int32
    {
        None            = 1 << 0,
        General         = 1 << 1,
        Behavior        = 1 << 2,
        Scripting       = 1 << 3,
        Communication   = 1 << 4,
    }
}
