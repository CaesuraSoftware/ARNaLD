
using System;

namespace Caesura.PerformanceMonitor.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using Display;
    
    public class Refresh : BaseCommand
    {
        public Refresh() : base()
        {
            this.Name = "Refresh";
        }
        
        public override Boolean Verify(String input)
        {
            var strs = input.Split(' ');
            if (strs.Length < 2)
            {
                return false;
            }
            
            this.Input = strs[1];
            if (String.Equals(strs[0], "Refresh", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
        
        public override RequestProgramState Run(View view)
        {
            var success = Int32.TryParse(this.Input, out var rr);
            if (!success || rr < 0)
            {
                view.SetInput($"{view.Prompt}\"{this.Input}\" is not a valid refresh rate.");
                return RequestProgramState.Continue;
            }
            view.ClearScreen();
            view.SetInput($"{view.Prompt}Refresh rate set to {rr}");
            view.RefreshRate = rr;
            return RequestProgramState.Continue;
        }
    }
}
