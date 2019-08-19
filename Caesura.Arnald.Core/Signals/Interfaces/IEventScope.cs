
using System;

namespace Caesura.Arnald.Core.Signals
{
    using System.Collections.Generic;
    using Caesura.Standard;
    
    public interface IEventScope
    {
        /// <summary>
        /// Configure whether more than one Activator will get
        /// an event sequentially or in parallel.
        /// </summary>
        /// <value></value>
        Boolean UseActivatorPriority { get; set; }
        
        Maybe<IEvent> GetEvent(String eventName);
        Boolean IsEventRegistered(String eventName);
        /// <summary>
        /// Register a new event.
        /// </summary>
        /// <param name="eventName"></param>
        void Register(String eventName);
        /// <summary>
        /// Unregister an event.
        /// </summary>
        /// <param name="eventName"></param>
        void Unregister(String eventName);
        
        Int32 GetLowestPriorityActivator(String eventName);
        Int32 GetHighestPriorityActivator(String eventName);
        /// <summary>
        /// Subscribe to this event, returning an IActivator.
        /// The name of the IActivator will be a new GUID and
        /// the priority will be set to highest.
        /// </summary>
        /// <returns></returns>
        IActivator Subscribe(String eventName, ActivatorCallback callback);
        /// <summary>
        /// Subscribe to this event, returning an IActivator with the
        /// given name and version.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IActivator Subscribe(String eventName, String name, Version version, Int32 priority, ActivatorCallback callback);
    }
}
