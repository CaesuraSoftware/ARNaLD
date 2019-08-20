
using System;

namespace Caesura.Arnald.Core.Signals
{
    using System.Collections.Generic;
    using Caesura.Standard;
    
    // TODO: OnX event properties, mainly for logging
    
    public interface IEventScope
    {
        /// <summary>
        /// Configure whether more than one Activator will get
        /// an event sequentially or in parallel.
        /// </summary>
        /// <value></value>
        Boolean UseActivatorPriority { get; set; }
        String Namespace { get; set; }
        
        /// <summary>
        /// Run the main event for this scope.
        /// </summary>
        void Run();
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
        IActivator Subscribe(String eventName, SubscriptionConfiguration config);
        void Raise(String eventName);
        void Raise(String eventName, IDataContainer data);
        void Raise(ISignal signal);
        /// <summary>
        /// Force unblock all events.
        /// </summary>
        void UnblockAll();
        /// <summary>
        /// Block an event from being raised to instead raise another event.
        /// Will automatically unblock when the event is raised again.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventNameToCall"></param>
        /// <returns></returns>
        IActivator Intercept(String eventName, String eventNameToCall);
        /// <summary>
        /// Block an event from being raised to instead raise another event.
        /// Will automatically unblock when the event is raised again.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="signal"></param>
        /// <returns></returns>
        IActivator Intercept(String eventName, ISignal signal);
        /// <summary>
        /// Block an event from being raised and instead run a callback.
        /// Will automatically unblock when the event is raised again.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        IActivator Intercept(String eventName, ActivatorCallback callback);
    }
}
