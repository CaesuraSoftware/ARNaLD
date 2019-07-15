
using System;

namespace Caesura.PerformanceMonitor.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    
    public abstract class BaseCommand
    {
        public String Name { get; set; }
        
        public BaseCommand()
        {
            
        }
        
        public abstract Boolean Verify(String input);
        
        public abstract RequestProgramState Run();
    }
}
