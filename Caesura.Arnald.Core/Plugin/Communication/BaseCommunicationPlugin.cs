
using System;

namespace Caesura.Arnald.Core.Plugin.Communication
{
    using System.Collections.Generic;
    using System.Linq;
    
    public abstract class BaseCommunicationPlugin : BasePlugin
    {
        public override PluginKind Kind => PluginKind.Communication;
    }
}
