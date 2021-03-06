
using System;

namespace Caesura.PerformanceMonitor.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using Display;
    
    public class ViewCmd : BaseCommand
    {
        public ViewCmd() : base()
        {
            this.Name = "View";
        }
        
        public override Boolean Verify(String input)
        {
            if (String.Equals(input, "Help", StringComparison.OrdinalIgnoreCase))
            {
                this.Input = "Help";
                return true;
            }
            if (String.Equals(input, "Back", StringComparison.OrdinalIgnoreCase))
            {
                this.Input = "Main";
                return true;
            }
            
            var strs = input.Split(' ');
            if (strs.Length < 2)
            {
                return false;
            }
            
            this.Input = strs[1];
            if (String.Equals(strs[0], "View", StringComparison.OrdinalIgnoreCase)
            ||  String.Equals(strs[0], "See" , StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
        
        public override RequestProgramState Run(View view)
        {
            var vf = view.GetView(this.Input);
            if (vf is null)
            {
                view.SetInput($"{new String(' ', view.Prompt.Length)}Page not found.", ConsoleColor.Yellow);
                return RequestProgramState.Continue;
            }
            if (vf.Name != "Main")
            {
                view.SetInput($"{new String(' ', view.Prompt.Length)}Viewing page: {vf.Name}. To go back, use 'view main'.", ConsoleColor.Cyan);
            }
            view.SetView(vf.Name);
            return RequestProgramState.Continue;
        }
    }
}
