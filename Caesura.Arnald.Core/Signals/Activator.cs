
using System;

namespace Caesura.Arnald.Core.Signals
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    
    public delegate void ActivatorCallback(IActivator self, ISignal signal);
    
    public class Activator : IActivator
    {
        public String Name { get; set; }
        public String Namespace { get; set; }
        public Version Version { get; set; }
        public ActivatorCallback OnReceive { get; set; }
        
        private Event HostEvent { get; set; }
        
        public Activator()
        {
            
        }
        
        public Activator(Event ev)
        {
            this.HostEvent = ev;
        }
        
        public virtual void Activate(ISignal signal)
        {
            this.OnReceive?.Invoke(this, signal);
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
        }
        
        public void Dispose()
        {
            this.internalUnsubscribe();
        }
    }
}
