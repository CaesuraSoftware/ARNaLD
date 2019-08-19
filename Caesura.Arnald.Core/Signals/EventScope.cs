
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
            set => this.Events.Map(ev => ev.UseActivatorPriority = value);
        }
        
        public EventScope()
        {
            this.Events                 = new List<Event>();
            this.b_UseactivatorPriority = false;
        }
        
        public IEvent GetEvent(String eventName)
        {
            throw new NotImplementedException();
        }
        
        public Boolean IsEventRegistered(String eventName)
        {
            throw new NotImplementedException();
        }
        
        public void Register(String eventName)
        {
            throw new NotImplementedException();
        }
        
        public void Unregister(String eventName)
        {
            throw new NotImplementedException();
        }
        
        
        public Int32 GetLowestPriorityActivator(String eventName)
        {
            throw new NotImplementedException();
        }
        
        public Int32 GetHighestPriorityActivator(String eventName)
        {
            throw new NotImplementedException();
        }
        
        public IActivator Subscribe(String eventName, ActivatorCallback callback)
        {
            throw new NotImplementedException();
        }
        
        public IActivator Subscribe(String eventName, String name, Version version, Int32 priority, ActivatorCallback callback)
        {
            throw new NotImplementedException();
        }
        
        private void SetUseActivatorPriority(Boolean value)
        {
            foreach (var ev in this.Events)
            {
                ev.UseActivatorPriority = value;
            }
        }
    }
}
