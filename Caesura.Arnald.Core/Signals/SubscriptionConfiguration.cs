
using System;

namespace Caesura.Arnald.Core.Signals
{
    using System.Collections.Generic;
    using System.Linq;
    
    public enum SubscriptionConfigurationPriority
    {
        None        =      0,
        Lowest      = 1 << 0,
        Highest     = 1 << 1,
        Midrange    = 1 << 2,
    }
    
    public class SubscriptionConfiguration
    {
        public String Name { get; set; }
        public String Namespace { get; set; }
        public Version Version { get; set; }
        public Int32 Priority { get; set; }
        public ActivatorCallback OnActivate { get; set; }
        public Action<IActivator> OnUnsubscribe { get; set; }
        
        public SubscriptionConfigurationPriority PreferredPriority { get; set; }
        
        public SubscriptionConfiguration()
        {
            this.Name               = Guid.NewGuid().ToString().ToUpper();
            this.Namespace          = Event.DefaultNamespace;
            this.Version            = new Version(1, 0, 0, 0);
            
            this.PreferredPriority  = SubscriptionConfigurationPriority.Highest;
        }
    }
}
