
using System;

namespace Caesura.Arnald.Core.Plugin
{
    public interface IPlugin : IDisposable
    {
        PluginKind Kind { get; }
        String Name { get; }
        BasePluginAgent Operator { get; }
    }
}
