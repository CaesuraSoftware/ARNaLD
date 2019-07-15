
using System;

namespace Caesura.PerformanceMonitor.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using Display;
    
    public class Echo : BaseCommand
    {
        public Echo() : base()
        {
            this.Name = "Echo";
        }
        
        public override Boolean Verify(String input)
        {
            var strs = input.Split(' ');
            if (strs.Length < 2)
            {
                return false;
            }
            
            this.Input = strs[1];
            if (String.Equals(strs[0], "Echo", StringComparison.OrdinalIgnoreCase)
            ||  String.Equals(strs[0], "Say" , StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
        
        public override RequestProgramState Run(View view)
        {
            view.SetInput($"{new String(' ', view.Prompt.Length)}\"{this.Input}\" to you too!", ConsoleColor.Cyan);
            return RequestProgramState.Continue;
        }
    }
}
