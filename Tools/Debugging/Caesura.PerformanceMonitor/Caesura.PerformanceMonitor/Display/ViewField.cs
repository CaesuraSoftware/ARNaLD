
using System;

namespace Caesura.PerformanceMonitor.Display
{
    using System.Collections.Generic;
    using System.Linq;
    using Monitor;
    
    public class ViewField
    {
        public String Name { get; set; }
        public Action<View, MonitorResult> Run { get; set; }
    }
}
