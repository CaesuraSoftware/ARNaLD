
using System;

namespace Caesura.Arnald.Core.Plugin
{
    using Agents;
    
    public abstract class BasePlugin : IPlugin
    {
        public abstract PluginKind Kind { get; }
        public abstract String Name { get; protected set; }
        public IAgent Operator { get; set; }
        
        public BasePlugin()
        {
            
        }
        
        public virtual void Dispose()
        {
            this.Operator?.Dispose();
        }
    }
}
