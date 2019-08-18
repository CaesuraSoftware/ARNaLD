
using System;

namespace Caesura.Arnald.Core.Signals
{
    using System.Collections.Generic;
    using System.Threading;
    
    // TODO: we shouldn't block the thread for only one event, need to rethink this
    // instead, have ONE Activator that blocks until it gets Raised
    
    public interface IActivator : IDisposable
    {
        String Name { get; }
        String Namespace { get; }
        Version Version { get; }
        
        /// <summary>
        /// Run when a signal is raised.
        /// </summary>
        void Activate(ISignal signal);
        /// <summary>
        /// Block this signal from being raised to any Activator
        /// except this one.
        /// </summary>
        void Block();
        /// <summary>
        /// Unblock this signal from being raised.
        /// Only the blocking Activator can unblock it.
        /// </summary>
        void Unblock();
        /// <summary>
        /// Raise a signal to this activator's host Event.
        /// </summary>
        void Raise();
        /// <summary>
        /// Raise a signal to this activator's host Event.
        /// </summary>
        /// <param name="data"></param>
        void Raise(IDataContainer data);
        /// <summary>
        /// Unsubscribe this Activator from the IEvent that created it.
        /// </summary>
        void Unsubscribe();
    }
}
