
using System;

namespace Caesura.Arnald.Core.Signals
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    
    public class Activator : IActivator
    {
        public String Name { get; set; }
        public String Namespace { get; set; }
        public Version Version { get; set; }
        public Int32 Priority { get; set; }
        public Boolean SelfActivate { get; set; }
        public ActivatorCallback OnActivate { get; set; }
        public Action<IActivator> OnUnsubscribe { get; set; }
        
        private Event HostEvent { get; set; }
        
        public Activator()
        {
            this.SelfActivate = false;
        }
        
        public Activator(Event ev) : this()
        {
            this.HostEvent = ev;
        }
        
        public virtual void Activate(ISignal signal)
        {
            this.OnActivate?.Invoke(this, signal);
        }
        
        public void Block()
        {
            this.HostEvent.Block(this);
        }
        
        public void Unblock()
        {
            this.HostEvent.Unblock(this);
        }
        
        public void Raise()
        {
            this.HostEvent.Raise(this);
        }
        
        public void Raise(IDataContainer data)
        {
            this.HostEvent.Raise(this, data);
        }
        
        public void Unsubscribe()
        {
            this.Dispose();
        }
        
        private void internalUnsubscribe()
        {
            this.HostEvent.Unsubscribe(this);
            this.OnUnsubscribe?.Invoke(this);
        }
        
        public void Dispose()
        {
            this.internalUnsubscribe();
        }
    }
}
