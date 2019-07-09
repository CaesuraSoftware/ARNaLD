
using System;

namespace Caesura.Arnald.Core.Plugin
{
    using System.Collections.Generic;
    using System.Linq;
    
    public abstract class BasePlugin : IPlugin
    {
        public abstract PluginKind Kind { get; }
        public abstract String Name { get; protected set; }
        
        public BasePlugin()
        {
            
        }
    }
}
