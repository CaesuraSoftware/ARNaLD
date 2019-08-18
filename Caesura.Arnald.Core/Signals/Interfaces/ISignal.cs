
using System;

namespace Caesura.Arnald.Core.Signals
{
    using System.Collections.Generic;
    using Caesura.Standard;
    
    /// <summary>
    /// A Signal reprsents a system event requesting a subsystem to activate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISignal<T> : ICopyable
    {
        /// <summary>
        /// The name of the signal
        /// </summary>
        String Name { get; }
        /// <summary>
        /// The namespace of the signal, which is usually the name of the subsystem firing the signal.
        /// </summary>
        String Namespace { get; }
        /// <summary>
        /// The version of the signal, so subsystems can look for specific versions of newer or older signals.
        /// </summary>
        Version Version { get; }
        /// <summary>
        /// The data of the signal, which is essentially the arguments for which ever subsystem gets the signal.
        /// </summary>
        IDataContainer<T> Data { get; }
        
        Boolean HasData(String key);
        Maybe<T> GetData(String key);
        Boolean HasData<M>(String key);
        Maybe<M> GetData<M>(String key);
        void AddData(String key, T value);
    }
    
    /// <summary>
    /// A Signal reprsents a system event requesting a subsystem to activate.
    /// </summary>
    public interface ISignal : ISignal<Object>
    {
        
    }
}
