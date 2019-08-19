
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
        public Boolean UseActivatorPriority { get; set; }
        
        private IActivator EventBlocker { get; set; }
        private List<IActivator> Activators { get; set; }
        
        public Event()
        {
            this.Namespace              = DefaultNamespace;
            this.Blocked                = false;
            this.UseActivatorPriority   = false;
            
            this.Activators             = new List<IActivator>();
        }
        
        public Event(String name) : this()
        {
            this.Name = name;
        }
        
        public Int32 GetLowestPriorityActivator()
        {
            var priority = 0;
            if (this.Activators.Count > 0)
            {
                var first = this.Activators.First();
                priority = first.Priority;
            }
            return priority;
        }
        
        public Int32 GetHighestPriorityActivator()
        {
            var priority = 0;
            if (this.Activators.Count > 0)
            {
                var last = this.Activators.Last();
                priority = last.Priority;
            }
            return priority;
        }
        
        public IActivator Subscribe(ActivatorCallback callback)
        {
            var name     = Guid.NewGuid().ToString().ToUpper();
            var ver      = new Version(1, 0, 0, 0);
            var priority = this.GetHighestPriorityActivator();
            var config = new SubscriptionConfiguration()
            {
                Name        = name,
                Version     = ver,
                Priority    = priority,
                OnActivate  = callback,
            };
            return this.Subscribe(config);
        }
        
        public IActivator Subscribe(SubscriptionConfiguration config)
        {
            var activator = new Activator(this)
            {
                Name            = config.Name,
                Namespace       = this.Namespace,
                Version         = config.Version,
                Priority        = config.Priority,
                OnActivate      = config.OnActivate,
                OnUnsubscribe   = config.OnUnsubscribe,
            };
            this.Activators.Add(activator);
            this.Activators.Sort((x, y) => x.Priority.CompareTo(y.Priority));
            return activator;
        }
        
        public void Unsubscribe(IActivator activator)
        {
            if (activator is null)
            {
                throw new ArgumentNullException(nameof(activator));
            }
            this.Activators.Remove(activator);
        }
        
        public void Block(IActivator blocker)
        {
            if (blocker is null)
            {
                throw new ArgumentNullException(nameof(blocker));
            }
            this.EventBlocker = blocker;
            this.Blocked = true;
        }
        
        public void Unblock(IActivator blocker)
        {
            if (blocker is null)
            {
                throw new ArgumentNullException(nameof(blocker));
            }
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
            // TODO: maybe put raising in it's own thread or something, so it doesn't block?
            // actually maybe just do an async variant
            this.GetRaisedEvents(activator, data);
        }
        
        private void GetRaisedEvents(IActivator activator, IDataContainer data)
        {
            if (activator is null)
            {
                throw new ArgumentNullException(nameof(activator));
            }
            
            var signal = new Signal(this.Name, this.Namespace, activator.Version);
            if (!(data is null))
            {
                signal.Data = data;
            }
            
            /**/ if (this.Blocked)
            {
                this.EventBlocker.Activate(signal);
            }
            else if (this.UseActivatorPriority)
            {
                foreach (var a in this.Activators)
                {
                    if (this.Blocked)
                    {
                        // if a previously-activated activator blocked this event,
                        // then do not activate any further activators.
                        break;
                    }
                    a.Activate(signal);
                }
            }
            else
            {
                this.Activators.ParallelForEach(a => 
                {
                    // same philosophy as the previous comment, except this is more
                    // of wishful thinking than hard logic given this is multithreaded.
                    if (!this.Blocked)
                    {
                        a.Activate(signal);
                    }
                });
            }
        }
    }
}
