
using System;

namespace Caesura.PerformanceMonitor.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using Display;
    
    public class Clear : BaseCommand
    {
        public Clear() : base()
        {
            this.Name = "Clear";
        }
        
        public override Boolean Verify(String input)
        {
            if (String.Equals(input, "Clear", StringComparison.OrdinalIgnoreCase)
            ||  String.Equals(input, "CLS"  , StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
        
        public override RequestProgramState Run(View view)
        {
            view.ClearScreen();
            return RequestProgramState.Continue;
        }
    }
}
