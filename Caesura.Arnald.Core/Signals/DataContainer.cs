
using System;

namespace Caesura.Arnald.Core.Signals
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Caesura.Standard;
    
    // TODO: make sure these can be JSON serialized and back.
    
    /// <summary>
    /// A key/value data store with a String key and an Object value.
    /// </summary>
    public class DataContainer : DataContainer<Object>, IDataContainer
    {
        public DataContainer() : base() { }
        public DataContainer(Int32 capacity) : base(capacity) { }
        public DataContainer(IDictionary<String, Object> dict) : base(dict) { }
        public DataContainer(IDataContainer dc) : base(dc) { }
        public DataContainer(DataContainer<Object> dc) : base(dc) { }
    }
    
    /// <summary>
    /// A key/value data store with a String key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataContainer<T> : IDataContainer<T>
    {
        public Int32 Count => this.internalDictionary.Count;
        
        private Dictionary<String, T> __internalDictionary;
        private Dictionary<String, T> internalDictionary 
        { 
            get
            {
                if (this.__internalDictionary is null)
                {
                    this.__internalDictionary = new Dictionary<String, T>();
                }
                return this.__internalDictionary;
            }
            set => this.__internalDictionary = value;
        }
        
        public DataContainer()
        {
            
        }
        
        public DataContainer(Int32 capacity)
        {
            this.__internalDictionary = new Dictionary<String, T>(capacity);
        }
        
        public DataContainer(IDictionary<String, T> dict)
        {
            this.__internalDictionary = new Dictionary<String, T>(dict);
        }
        
        public DataContainer(IDataContainer dc) : this(dc.Count)
        {
            this.Copy(dc);
        }
        
        public DataContainer(DataContainer<T> dc) : this(dc.Count)
        {
            this.Copy(dc);
        }
        
        public void Copy(Object o)
        {
            if (o is IDataContainer dc)
            {
                foreach (var kvp in dc)
                {
                    var val = kvp.Value;
                    if (val is ICopyable icp)
                    {
                        val = icp.Clone();
                    }
                    this.Set(kvp.Key, (T)val);
                }
            }
        }
        
        public ICopyable Clone()
        {
            var dc = new DataContainer<T>(this);
            return dc;
        }
        
        public void Set(String key, T value)
        {
            if (this.HasValue(key))
            {
                throw new ElementExistsException($"Key \"{key}\" is already present");
            }
            this.Replace(key, value);
        }
        
        public Boolean Replace(String key, T value)
        {
            var replaced = false;
            if (this.HasValue(key))
            {
                this.internalDictionary.Remove(key);
                replaced = true;
            }
            this.internalDictionary.Add(key, value);
            return replaced;
        }
        
        public T this[String key]
        {
            get => this.internalDictionary[key];
            set => this.internalDictionary[key] = value;
        }
        
        public Maybe<T> Get(String key)
        {
            if (!this.HasValue(key))
            {
                return Maybe.None;
            }
            return Maybe<T>.Some(this.internalDictionary[key]);
        }
        
        public Maybe<M> Get<M>(String key)
        {
            if (!this.HasValue(key))
            {
                return Maybe.None;
            }
            var item = this.internalDictionary[key];
            if (item is M mi)
            {
                return Maybe<M>.Some(mi);
            }
            return Maybe.None;
        }
        
        public Boolean HasValue(String key)
        {
            return this.internalDictionary.ContainsKey(key);
        }
        
        public Boolean HasValue<M>(String key)
        {
            if (!this.HasValue(key))
            {
                return false;
            }
            var item = this.internalDictionary[key];
            if (item is M mi)
            {
                return true;
            }
            return false;
        }
        
        public IEnumerator<KeyValuePair<String, T>> GetEnumerator()
        {
            return this.internalDictionary.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        
        public override String ToString()
        {
            var items = this.internalDictionary.ToList();
            
            var sb = new StringBuilder();
            sb.Append("{ ");
            for (var i = 0; i < items.Count; i++)
            {
                var item = items.ElementAt(i);
                sb.Append(item.Key);
                sb.Append(": ");
                sb.Append(item.Value.ToString());
                if (i < items.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(" }");
            
            return sb.ToString();
        }
    }
}
