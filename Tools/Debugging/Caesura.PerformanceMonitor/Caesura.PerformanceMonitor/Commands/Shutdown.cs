
using System;

namespace Caesura.PerformanceMonitor.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
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
            ||  String.Equals(input, "Leave"   , StringComparison.OrdinalIgnoreCase)
            ||  String.Equals(input, "Quit"    , StringComparison.OrdinalIgnoreCase)
            ||  String.Equals(input, "End"     , StringComparison.OrdinalIgnoreCase)
            ||  String.Equals(input, "Die"     , StringComparison.OrdinalIgnoreCase)
            ||  String.Equals(input, "."       , StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
        
        public override RequestProgramState Run(View view)
        {
            view.SetInput($"{new String(' ', view.Prompt.Length)}bai bai! :D", ConsoleColor.Cyan);
            Thread.Sleep(1500);
            return RequestProgramState.Exit;
        }
    }
}
