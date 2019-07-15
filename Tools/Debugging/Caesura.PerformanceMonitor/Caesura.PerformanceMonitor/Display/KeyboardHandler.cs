
using System;

namespace Caesura.PerformanceMonitor.Display
{
    using System.Collections.Generic;
    using System.Linq;
    
    public class KeyboardHandler
    {
        public ConsoleKeyInfo LastKey { get; private set; }
        private Boolean TextInputMode { get; set; }
        private List<ConsoleKeyInfo> InputBuffer { get; set; }
        
        public KeyboardHandler()
        {
            this.TextInputMode = false;
            this.InputBuffer   = new List<ConsoleKeyInfo>(255);
        }
        
        public RequestProgramState Process(ConsoleKeyInfo info)
        {
            if (this.TextInputMode)
            {
                if (info.Key == ConsoleKey.Enter)
                {
                    return RequestProgramState.CommandInput;
                }
                if (info.Key == ConsoleKey.Escape)
                {
                    this.TextInputMode = false;
                    return RequestProgramState.CommandMode;
                }
                
                this.LastKey = info;
                this.InputBuffer.Add(info);
                return RequestProgramState.TextInput;
            }
            else
            {
                if (info.Key == ConsoleKey.Q)
                {
                    return RequestProgramState.Exit;
                }
                if (info.Key == ConsoleKey.I)
                {
                    this.TextInputMode = true;
                    return RequestProgramState.Continue;
                }
            }
            
            return RequestProgramState.None;
        }
        
        public List<ConsoleKeyInfo> GetBuffer()
        {
            var keys = new List<ConsoleKeyInfo>(this.InputBuffer);
            return keys;
        }
        
        public void ClearBuffer()
        {
            this.InputBuffer.Clear();
        }
    }
}
