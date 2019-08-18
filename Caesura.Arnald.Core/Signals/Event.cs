
using System;

namespace Caesura.Arnald.Core.Signals
{
    using System.Collections.Generic;
    using System.Linq;
    using Caesura.Standard;
    
    public class Event : IEvent
    {
        public static String DefaultNamespace => "*";
        
        public String Name { get; set; }
        public String Namespace { get; set; }
        public Boolean Blocked { get; private set; }
        
        private IActivator EventBlocker { get; set; }
        private List<IActivator> Activators { get; set; }
        
        public Event()
        {
            this.Namespace = DefaultNamespace;
            
            this.Activators = new List<IActivator>();
        }
        
        public Event(String name) : this()
        {
            this.Name = name;
        }
        
        public IActivator Subscribe()
        {
            var name = Guid.NewGuid().ToString().ToUpper();
            var ver  = new Version(1, 0, 0, 0);
            return this.Subscribe(name, ver);
        }
        
        public IActivator Subscribe(String name, Version version)
        {
            var activator = new Activator(this)
            {
                Name        = name,
                Namespace   = this.Namespace,
                Version     = version,
            };
            this.Activators.Add(activator);
            return activator;
        }
        
        public void Unsubscribe(IActivator activator)
        {
            this.Activators.Remove(activator);
        }
        
        public void Block(IActivator blocker)
        {
            this.EventBlocker = blocker;
            this.Blocked = true;
        }
        
        public void Unblock(IActivator blocker)
        {
            if (!(this.EventBlocker is null) && !Object.ReferenceEquals(blocker, this.EventBlocker))
            {
                throw new InvalidOperationException(
                    $"Attempt to unblock event \"{this.Name}\" by activator \"{blocker.Name}\". " +
                    $"Event was blocked by \"{this.EventBlocker.Name}\" and can only be unblocked by it"
                );
            }
            this.EventBlocker = null;
            this.Blocked = false;
        }
        
        public void Raise(IActivator activator)
        {
            this.Raise(activator, null);
        }
        
        public void Raise(IActivator activator, IDataContainer data)
        {
            var signal = new Signal(this.Name, this.Namespace, activator.Version);
            if (!(data is null))
            {
                signal.Data = data;
            }
            
            /**/ if (this.Blocked)
            {
                this.EventBlocker.Activate(signal);
            }
            else
            {
                this.Activators.ParallelForEach(a => a.Activate(signal));
            }
        }
    }
}
