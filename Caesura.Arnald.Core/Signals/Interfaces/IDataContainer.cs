
using System;

namespace Caesura.Arnald.Core.Signals
{
    using System.Collections.Generic;
    using Caesura.Standard;
    
    /// <summary>
    /// A key/value data store with a String key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataContainer<T> : IEnumerable<KeyValuePair<String, T>>, ICopyable<T>
    {
        Int32 Count { get; }
        T this[String key] { get; set; }
        
        /// <summary>
        /// Attempt to add a value with a key. Throw if the key exists.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set(String key, T value);
        /// <summary>
        /// Add or replace an element with a key. Return true if the item was replaced, false if it was added.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>True if the key was replaced, false if it was newly added.</returns>
        Boolean Replace(String key, T value);
        /// <summary>
        /// Get a value using a key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Maybe<T> Get(String key);
        /// <summary>
        /// Check if a key exists.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Boolean HasValue(String key);
    }
    
    /// <summary>
    /// A key/value data store with a String key and an Object value.
    /// </summary>
    public interface IDataContainer : IDataContainer<Object>
    {
        /// <summary>
        /// Get a value using a key, and attempt to cast the Object type to the generic argument.
        /// Return None if the key was not found or if the value cannot be cast to that type.
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="M"></typeparam>
        /// <returns></returns>
        Maybe<M> Get<M>(String key);
        /// <summary>
        /// Check if a key exists or if the value can be cast to the generic argument.
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="M"></typeparam>
        /// <returns></returns>
        Boolean HasValue<M>(String key);
    }
}
