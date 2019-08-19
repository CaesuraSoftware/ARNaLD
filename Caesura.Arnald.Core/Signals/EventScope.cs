
using System;

namespace Caesura.Arnald.Core.Signals
{
    using System.Collections.Generic;
    using System.Linq;
    using Caesura.Standard;
    
    public class EventScope : IEventScope
    {
        private List<Event> Events { get; set; }
        private Boolean b_UseactivatorPriority;
        public Boolean UseActivatorPriority
        {
            get => this.b_UseactivatorPriority;
            set { this.b_UseactivatorPriority = value; this.Events.Map(ev => ev.UseActivatorPriority = value); }
        }
        private String b_Namespace;
        public String Namespace
        { 
            get => this.b_Namespace;
            set { this.b_Namespace = value; this.Events.Map(ev => ev.Namespace = value); }
        }
        
        public EventScope()
        {
            this.Events                 = new List<Event>();
            this.b_UseactivatorPriority = false;
            this.b_Namespace            = Event.DefaultNamespace;
        }
        
        public Maybe<IEvent> GetEvent(String eventName)
        {
            if (this.IsEventRegistered(eventName))
            {
                var ev = this.Events.Find(x => x.Name == eventName);
                return Maybe<IEvent>.Some(ev);
            }
            return Maybe.None;
        }
        
        public Boolean IsEventRegistered(String eventName)
        {
            return this.Events.Exists(x => x.Name == eventName);
        }
        
        public void Register(String eventName)
        {
            if (this.IsEventRegistered(eventName))
            {
                throw new ElementExistsException(eventName);
            }
            var ev = new Event(eventName)
            {
                UseActivatorPriority    = this.UseActivatorPriority,
                Namespace               = this.Namespace,
            };
            this.Events.Add(ev);
        }
        
        public void Unregister(String eventName)
        {
            if (!this.IsEventRegistered(eventName))
            {
                throw new ElementNotFoundException(eventName);
            }
            var ev = this.GetEvent(eventName).Value as Event;
            this.Events.Remove(ev);
        }
        
        
        public Int32 GetLowestPriorityActivator(String eventName)
        {
            var mev = this.GetEvent(eventName);
            if (mev)
            {
                return mev.Value.GetLowestPriorityActivator();
            }
            throw new ElementNotFoundException(eventName);
        }
        
        public Int32 GetHighestPriorityActivator(String eventName)
        {
            var mev = this.GetEvent(eventName);
            if (mev)
            {
                return mev.Value.GetHighestPriorityActivator();
            }
            throw new ElementNotFoundException(eventName);
        }
        
        public IActivator Subscribe(String eventName, ActivatorCallback callback)
        {
            var mev = this.GetEvent(eventName);
            if (mev)
            {
                return mev.Value.Subscribe(callback);
            }
            throw new ElementNotFoundException(eventName);
        }
        
        public IActivator Subscribe(String eventName, SubscriptionConfiguration config)
        {
            var mev = this.GetEvent(eventName);
            if (mev)
            {
                return mev.Value.Subscribe(config);
            }
            throw new ElementNotFoundException(eventName);
        }
    }
}
