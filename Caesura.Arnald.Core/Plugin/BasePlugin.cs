
using System;

namespace Caesura.Arnald.Core.Plugin
{
    using System.Collections.Generic;
    using System.Linq;
    
    public abstract class BasePlugin : IPlugin
    {
        public abstract PluginKind Kind { get; }
    }
}
