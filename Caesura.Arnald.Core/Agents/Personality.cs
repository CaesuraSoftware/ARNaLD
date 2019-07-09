
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    
    public class Personality
    {
        private List<IBehavior> Behaviors { get; set; }
        
        public Personality()
        {
            this.Behaviors = new List<IBehavior>();
        }
        
        public void Learn(IBehavior behavior)
        {
            
        }
    }
}
