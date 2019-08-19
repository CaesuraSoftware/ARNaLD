
using System;

namespace Caesura.Arnald.Core.Signals
{
    using System.Collections.Generic;
    using System.Linq;
    
    public class SubscriptionConfiguration
    {
        public String Name { get; set; }
        public String Namespace { get; set; }
        public Version Version { get; set; }
        public Int32 Priority { get; set; }
        public ActivatorCallback OnActivate { get; set; }
        public Action<IActivator> OnUnsubscribe { get; set; }
        
        public SubscriptionConfiguration()
        {
            
        }
    }
}
