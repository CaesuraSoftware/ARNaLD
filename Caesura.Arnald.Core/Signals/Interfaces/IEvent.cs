
using System;

namespace Caesura.Arnald.Core.Signals
{
    using System.Collections.Generic;
    
    public interface IEvent
    {
        String Name { get; }
        String Namespace { get; }
        Boolean Blocked { get; }
        
        /// <summary>
        /// Subscribe to this event, returning an IActivator.
        /// The name of the IActivator will be a new GUID.
        /// </summary>
        /// <returns></returns>
        IActivator Subscribe();
        /// <summary>
        /// Subscribe to this event, returning an IActivator with the
        /// given name and version.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IActivator Subscribe(String name, Version version);
    }
}
