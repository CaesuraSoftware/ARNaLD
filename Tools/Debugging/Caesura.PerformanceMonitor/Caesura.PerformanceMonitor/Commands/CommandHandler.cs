
using System;

namespace Caesura.PerformanceMonitor.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    
    public class CommandHandler
    {
        private List<BaseCommand> Commands { get; set; }
        
        public CommandHandler()
        {
            this.Commands = new List<BaseCommand>();
        }
        
        public void Add(BaseCommand command)
        {
            this.Commands.Add(command);
        }
        
        public RequestProgramState Run(String input)
        {
            foreach (var command in this.Commands)
            {
                if (command.Verify(input))
                {
                    return command.Run();
                }
            }
            return RequestProgramState.None;
        }
    }
}
