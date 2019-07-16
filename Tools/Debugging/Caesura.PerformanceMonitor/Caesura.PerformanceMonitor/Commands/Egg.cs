
using System;

namespace Caesura.PerformanceMonitor.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using Display;
    
    public class Egg : BaseCommand
    {
        public Egg() : base()
        {
            
        }
        
        public override Boolean Verify(String input)
        {
            /**/ if (String.Equals(input, "Paulman", StringComparison.OrdinalIgnoreCase))
            {
                this.Input = "ANSELMUS";
                return true;
            }
            else if (String.Equals(input, "Anselmus", StringComparison.OrdinalIgnoreCase))
            {
                this.Input = "PAULMAN";
                return true;
            }
            else if (String.Equals(input, "Fuck you", StringComparison.OrdinalIgnoreCase))
            {
                this.Input = "No, fuck you, leather man!";
                return true;
            }
            else if (String.Equals(input, "Fuck", StringComparison.OrdinalIgnoreCase))
            {
                this.Input = "DON'T KNOW HOW TO FUCK";
                return true;
            }
            else if (String.Equals(input, "Oh shit I'm sorry", StringComparison.OrdinalIgnoreCase))
            {
                this.Input = "Sorry for what? Our daddy taught us not to be ashamed of our dicks.";
                return true;
            }
            return false;
        }
        
        public override RequestProgramState Run(View view)
        {
            view.SetInput($"{new String(' ', view.Prompt.Length)}{this.Input}", ConsoleColor.Cyan);
            return RequestProgramState.Continue;
        }
    }
}
