
using System;

namespace Caesura.Arnald.Core.Signals
{
    using System.Collections.Generic;
    
    public interface IEvent
    {
        String Name { get; }
        String Namespace { get; }
        Int32 SubscriberCount { get; }
        /// <summary>
        /// Get or set whether Activators can get this event or not.
        /// </summary>
        /// <value></value>
        Boolean Blocked { get; }
        /// <summary>
        /// Configure whether more than one Activator will get
        /// an event sequentially or in parallel.
        /// </summary>
        /// <value></value>
        Boolean UseActivatorPriority { get; set; }
        
        Int32 GetLowestPriorityActivator();
        Int32 GetHighestPriorityActivator();
        /// <summary>
        /// Subscribe to this event, returning an IActivator.
        /// The name of the IActivator will be a new GUID and
        /// the priority will be set to highest.
        /// </summary>
        /// <returns></returns>
        IActivator Subscribe(ActivatorCallback callback);
        /// <summary>
        /// Subscribe to this event, returning an IActivator with the
        /// given name and version.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IActivator Subscribe(SubscriptionConfiguration config);
    }
}
