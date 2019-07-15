
using System;

namespace Caesura.PerformanceMonitor.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using Display;
    
    public class Shutdown : BaseCommand
    {
        public Shutdown() : base()
        {
            this.Name = "Shutdown";
        }
        
        public override Boolean Verify(String input)
        {
            if (String.Equals(input, "Shutdown", StringComparison.OrdinalIgnoreCase)
            ||  String.Equals(input, "Quit"    , StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
        
        public override RequestProgramState Run(View view)
        {
            view.Stop();
            view.Wait();
            view.ClearScreen();
            return RequestProgramState.Exit;
        }
    }
}
