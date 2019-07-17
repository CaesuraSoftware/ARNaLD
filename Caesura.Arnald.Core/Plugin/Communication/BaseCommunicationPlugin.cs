
using System;

namespace Caesura.Arnald.Core.Plugin.Communication
{
    // TODO: either here or in a custom CommunicationMessage,
    // create an array of Name/Value POCOs called "Contexts",
    // a context is where the location came from or is going
    // to, along with it's sub-locations. A Discord message
    // might be [guild, channel, (user if dm)]
    
    public abstract class BaseCommunicationPlugin : BasePlugin
    {
        public override PluginKind Kind => PluginKind.Communication;
    }
}
