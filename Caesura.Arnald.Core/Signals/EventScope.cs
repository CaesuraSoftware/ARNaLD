
using System;

namespace Caesura.Arnald.Core.Signals
{
    using System.Collections.Generic;
    using System.Linq;
    using Caesura.Standard;
    
    public class EventScope : IEventScope
    {
        public static String MainEventName => "__main";
        
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
        public IEventStack EventStack { get; set; }
        
        public EventScope()
        {
            this.Events                 = new List<Event>();
            this.b_UseactivatorPriority = true;
            this.b_Namespace            = Event.DefaultNamespace;
            this.EventStack             = new EventStack();
            
            this.Register(EventScope.MainEventName);
        }
        
        public EventScope(String nameSpace) : this()
        {
            this.Namespace = nameSpace;
        }
        
        public void Run()
        {
            this.UnblockAll();
            this.Raise(EventScope.MainEventName);
        }
        
        public void Run(Boolean repeat)
        {
            this.UnblockAll();
            this.EventStack.Reset();
            this.EventStack.Repeat = repeat;
            while (this.EventStack.Index != -1)
            {
                var item = this.EventStack.Next();
                this.Raise(item);
            }
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
        
        public void Raise(String eventName)
        {
            this.Raise(eventName, null);
        }
        
        public void Raise(String eventName, IDataContainer data)
        {
            var signal = new Signal()
            {
                Name        = eventName,
                Namespace   = this.Namespace,
            };
            if (data != null)
            {
                signal.Data = data;
            }
            this.Raise(signal);
        }
        
        public void Raise(ISignal signal)
        {
            var ev = this.Events.Find(x => x.Name == signal.Name);
            if (ev is null)
            {
                throw new ElementNotFoundException($"No event registered with name of \"{signal.Name}\"");
            }
            ev.Raise(signal);
        }
        
        public void UnblockAll()
        {
            this.Events.Map(ev => ev.Unblock());
        }
        
        public IActivator Intercept(String eventName, String eventNameToCall)
        {
            var signal = new Signal(eventNameToCall)
            {
                Namespace = this.Namespace,
            };
            return this.Intercept(eventName, signal);
        }
        
        public IActivator Intercept(String eventName, ISignal signal)
        {
            return this.Intercept(eventName, (self, s) =>
            {
                this.Raise(signal);
            });
        }
        
        public IActivator Intercept(String eventName, ActivatorCallback callback)
        {
            var ev = this.Events.Find(x => x.Name == eventName);
            if (ev is null)
            {
                throw new ElementNotFoundException($"No event registered with name of \"{eventName}\"");
            }
            return ev.Intercept(callback);
        }
        
        public IActivator Intercept(String eventName, String callNext, ActivatorCallback callback)
        {
            var ev = this.Events.Find(x => x.Name == eventName);
            if (ev is null)
            {
                throw new ElementNotFoundException($"No event registered with name of \"{eventName}\"");
            }
            return ev.Intercept(this, callNext, callback);
        }
    }
}
