
using System;

namespace Caesura.Arnald.Core.Plugin
{
    using System.Collections.Generic;
    
    public interface IPlugin
    {
        PluginKind Kind { get; }
        String Name { get; }
    }
}
