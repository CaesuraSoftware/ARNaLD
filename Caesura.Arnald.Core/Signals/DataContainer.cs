
using System;

namespace Caesura.Arnald.Core.Signals
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Caesura.Standard;
    
    public sealed class DataContainer : IDataContainer
    {
        public Int32 Count => this.internalDictionary.Count;
        
        private Dictionary<String, Object> __internalDictionary;
        private Dictionary<String, Object> internalDictionary 
        { 
            get
            {
                if (this.__internalDictionary is null)
                {
                    this.__internalDictionary = new Dictionary<String, Object>();
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
            this.__internalDictionary = new Dictionary<String, Object>(capacity);
        }
        
        public DataContainer(IDictionary<String, Object> dict)
        {
            this.__internalDictionary = new Dictionary<String, Object>(dict);
        }
        
        public DataContainer(IDataContainer dc) : this(dc.Count)
        {
            this.Copy(dc);
        }
        
        public void Copy(Object o)
        {
            if (o is DataContainer dc)
            {
                foreach (var kvp in dc)
                {
                    if (kvp.Value is ICopyable ic)
                    {
                        var nic = ic.Clone();
                        this.Set(kvp.Key, nic);
                    }
                    else
                    {
                        this.Set(kvp.Key, kvp.Value);
                    }
                }
            }
        }
        
        public ICopyable Clone()
        {
            var dc = new DataContainer(this);
            return dc;
        }
        
        public void Set(String key, Object value)
        {
            if (this.HasValue(key))
            {
                throw new ElementExistsException($"Key \"{key}\" is already present");
            }
            this.Replace(key, value);
        }
        
        public Boolean Replace(String key, Object value)
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
        
        public Object this[String key]
        {
            get => this.internalDictionary[key];
            set => this.internalDictionary[key] = value;
        }
        
        public Maybe<Object> Get(String key)
        {
            if (!this.HasValue(key))
            {
                return Maybe.None;
            }
            return Maybe<Object>.Some(this.internalDictionary[key]);
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
        
        public IEnumerator<KeyValuePair<String, Object>> GetEnumerator()
        {
            return this.internalDictionary.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
