
using System;

namespace Caesura.Arnald.Core.Signals
{
    using System.Collections.Generic;
    using System.Linq;
    using Caesura.Standard;
    
    // TODO: make sure these are JSON serializable and back
    
    /// <summary>
    /// A Signal reprsents a system event requesting a subsystem to activate.
    /// </summary>
    public class Signal : Signal<Object>, ISignal
    {
        public static String GlobalNamespace => "*";
        
        public Signal() : base() { }
        public Signal(String name, String nameSpace, Version ver) : base(name, nameSpace, ver) { }
        public Signal(String name) : base(name) { }
        public Signal(String name, String nameSpace, IDataContainer dc) : base(name, nameSpace, dc) { }
        public Signal(ISignal s) : base(s) { }
        public Signal(Signal<Object> s) : base(s) { }
    }
    
    /// <summary>
    /// A Signal reprsents a system event requesting a subsystem to activate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Signal<T> : ISignal<T>
    {
        public String Name { get; set; }
        public String Namespace { get; set; }
        public Version Version { get; set; }
        public String Sender { get; set; }
        public IDataContainer<T> Data { get; set; }
        
        public Signal()
        {
            this.Data = new DataContainer<T>();
        }
        
        public Signal(String name, String nameSpace, Version ver) : this()
        {
            this.Name       = name;
            this.Namespace  = nameSpace;
            this.Version    = ver;
        }
        
        public Signal(String name) : this(name, Signal.GlobalNamespace, new Version(1, 0, 0, 0))
        {
            
        }
        
        public Signal(String name, String nameSpace, IDataContainer<T> dc) : this(name, nameSpace, new Version(1, 0, 0, 0))
        {
            this.Data = dc.Clone() as IDataContainer<T>;
        }
        
        public Signal(ISignal s)
        {
            this.Copy(s);
        }
        
        public Signal(Signal<T> s)
        {
            this.Copy(s);
        }
        
        public void Copy(Object o)
        {
            if (o is ISignal s)
            {
                this.Name       = s.Name;
                this.Namespace  = s.Namespace;
                this.Version    = s.Version;
                this.Data       = s.Data.Clone() as IDataContainer<T>;
            }
        }
        
        public ICopyable Clone()
        {
            var s = new Signal<T>(this);
            return s;
        }
        
        public Boolean HasData(String key)
        {
            return this.Data.HasValue(key);
        }
        
        public Maybe<T> GetData(String key)
        {
            return this.Data.Get(key);
        }
        
        public Boolean HasData<M>(String key)
        {
            return this.Data.HasValue<M>(key);
        }
        
        public Maybe<M> GetData<M>(String key)
        {
            return this.Data.Get<M>(key);
        }
        
        public void AddData(String key, T value)
        {
            this.Data.Replace(key, value);
        }
        
        public void AssertData(String key)
        {
            if (!this.HasData(key))
            {
                throw new ElementNotFoundException(key);
            }
        }
        
        public void AssertData<M>(String key)
        {
            if (!this.HasData<M>(key))
            {
                throw new ElementNotFoundException(key);
            }
        }
    }
}
