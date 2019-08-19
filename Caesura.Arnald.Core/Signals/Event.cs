
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
        public Int32 SubscriberCount => this.Activators.Count;
        public Boolean Blocked { get; private set; }
        public Boolean UseActivatorPriority { get; set; }
        public IActivator EventBlocker { get; private set; }
        
        private List<IActivator> Activators { get; set; }
        
        public Event()
        {
            this.Namespace              = DefaultNamespace;
            this.Blocked                = false;
            this.UseActivatorPriority   = true;
            
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
            var config = new SubscriptionConfiguration()
            {
                OnActivate  = callback,
            };
            return this.Subscribe(config);
        }
        
        public IActivator Subscribe(SubscriptionConfiguration config)
        {
            var priority = config.Priority;
            switch (config.PreferredPriority)
            {
                case SubscriptionConfigurationPriority.Highest:
                    priority = this.GetHighestPriorityActivator() + 1;
                    break;
                case SubscriptionConfigurationPriority.Lowest:
                    priority = this.GetLowestPriorityActivator() -1;
                    break;
                case SubscriptionConfigurationPriority.Midrange:
                    priority = this.GetHighestPriorityActivator() / 2;
                    break;
            }
            var activator = new Activator(this)
            {
                Name            = config.Name,
                Namespace       = this.Namespace,
                Version         = config.Version,
                Priority        = priority,
                SelfActivate    = config.SelfActivate,
                OnActivate      = config.OnActivate,
                OnUnsubscribe   = config.OnUnsubscribe,
            };
            this.Activators.Add(activator);
            this.Activators.Sort((x, y) => -1 * x.Priority.CompareTo(y.Priority));
            
            // TODO: log subscription
            
            return activator;
        }
        
        public void Unsubscribe(IActivator activator)
        {
            if (activator is null)
            {
                throw new ArgumentNullException(nameof(activator));
            }
            this.Activators.Remove(activator);
            if (Object.ReferenceEquals(this.EventBlocker, activator))
            {
                this.Unblock(activator);
            }
            
            // TODO: log unsub
        }
        
        public void Block(IActivator blocker)
        {
            if (blocker is null)
            {
                throw new ArgumentNullException(nameof(blocker));
            }
            if (this.Blocked)
            {
                throw new InvalidOperationException($"Event is already blocked by \"{this.EventBlocker.Name}\"");
            }
            this.EventBlocker = blocker;
            this.Blocked = true;
            
            // TODO: log blocking
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
            
            // TODO: log unblocking
        }
        
        public void Raise(IActivator activator)
        {
            this.Raise(activator, null);
        }
        
        public void Raise(IActivator activator, IDataContainer data)
        {
            // TODO: async variant?
            // TODO: log raising
            this.GetRaisedEvents(activator, data);
        }
        
        public void Raise(ISignal signal)
        {
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
                    if (a.Name == signal.Sender && !a.SelfActivate)
                    {
                        continue;
                    }
                    a.Activate(signal);
                }
            }
            else
            {
                this.Activators.ParallelMap(a => 
                {
                    // same philosophy as the previous comment, except this is more
                    // of wishful thinking than hard logic given this is multithreaded.
                    if (!this.Blocked)
                    {
                        if (a.Name == signal.Sender && !a.SelfActivate)
                        {
                            return;
                        }
                        a.Activate(signal);
                    }
                });
            }
        }
        
        private void GetRaisedEvents(IActivator activator, IDataContainer data)
        {
            if (activator is null)
            {
                throw new ArgumentNullException(nameof(activator));
            }
            
            var signal = new Signal(this.Name, this.Namespace, activator.Version)
            {
                Sender = activator.Name,
            };
            if (!(data is null))
            {
                signal.Data = data;
            }
            
            this.Raise(signal);
        }
    }
}
