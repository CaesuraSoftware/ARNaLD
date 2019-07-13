
using System;

namespace Caesura.Arnald.Core.Plugin.Communication
{
    
    public abstract class BaseCommunicationPlugin : BasePlugin
    {
        public override PluginKind Kind => PluginKind.Communication;
    }
}
