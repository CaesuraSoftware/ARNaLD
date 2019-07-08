
using System;

namespace Caesura.Arnald.Core.Plugin
{
    public enum PluginKind : Int32
    {
        None            = 1 << 0,
        General         = 1 << 1,
        Scripting       = 1 << 2,
        Communication   = 1 << 3,
    }
}
