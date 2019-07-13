
using System;

namespace Caesura.Arnald.Core.Plugin
{
    using Agents;
    
    public interface IPlugin : IDisposable
    {
        PluginKind Kind { get; }
        String Name { get; }
        IAgent Operator { get; }
    }
}
