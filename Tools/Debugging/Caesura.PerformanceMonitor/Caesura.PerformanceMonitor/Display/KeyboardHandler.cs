
using System;

namespace Caesura.PerformanceMonitor.Display
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    public class KeyboardHandler
    {
        public ConsoleKeyInfo LastKey { get; private set; }
        private Boolean TextInputMode { get; set; }
        private List<String> History { get; set; }
        private Int32 HistoryPosition { get; set; }
        private StringBuilder Editor { get; set; }
        private Int32 EditDistance { get; set; }
        
        public KeyboardHandler()
        {
            this.TextInputMode  = false;
            this.History        = new List<String>() { "" };
            this.Editor         = new StringBuilder();
        }
        
        public RequestProgramState Process(ConsoleKeyInfo info)
        {
            if (this.TextInputMode)
            {
                if (info.Key == ConsoleKey.Enter)
                {
                    this.EditDistance = 0;
                    this.HistoryPosition = this.History.Count - 1;
                    var hi = this.Editor.ToString();
                    if ((!String.IsNullOrEmpty(hi)) && (!String.IsNullOrWhiteSpace(hi)))
                    {
                        if (this.History.Contains(hi))
                        {
                            this.History.Remove(hi);
                        }
                        this.History.Add(hi);
                        if (this.History.Count > 1000)
                        {
                            this.History.RemoveAt(1); // 0 is our empty string
                        }
                        this.HistoryPosition = this.History.Count;
                    }
                    return RequestProgramState.CommandInput;
                }
                if (info.Key == ConsoleKey.Escape)
                {
                    this.TextInputMode = false;
                    return RequestProgramState.CommandMode;
                }
                
                this.LastKey = info;
                return RequestProgramState.TextInput;
            }
            else
            {
                if (info.Key == ConsoleKey.Q
                ||  info.Key == ConsoleKey.Escape)
                {
                    return RequestProgramState.Exit;
                }
                if (info.Key == ConsoleKey.I)
                {
                    this.TextInputMode = true;
                    return RequestProgramState.EditMode;
                }
            }
            
            return RequestProgramState.None;
        }
        
        public String ProcessText()
        {
            var key = this.LastKey;
            
            /**/ if (key.Key == ConsoleKey.Backspace)
            {
                if (this.Editor.Length > 0)
                {
                    this.Editor.Remove(this.Editor.Length - 1, 1);
                    this.EditDistance--;
                    if (this.EditDistance < 0)
                    {
                        this.EditDistance = 0;
                    }
                }
            }
            else if (key.Key == ConsoleKey.LeftArrow)
            {
                this.EditDistance--;
                if (this.EditDistance < 0)
                {
                    this.EditDistance = 0;
                }
            }
            else if (key.Key == ConsoleKey.RightArrow)
            {
                this.EditDistance++;
                if (this.EditDistance > this.Editor.Length - 1)
                {
                    this.EditDistance = this.Editor.Length - 1;
                }
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                this.HistoryPosition--;
                if (this.HistoryPosition < 0)
                {
                    this.HistoryPosition = 0;
                }
                if (this.History.Count > 0 && this.History.Count > this.HistoryPosition)
                {
                    this.ClearBuffer();
                    var input = this.History[this.HistoryPosition];
                    this.Editor.Append(input);
                    this.EditDistance = this.Editor.Length;
                }
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                this.HistoryPosition++;
                if (this.HistoryPosition >= this.History.Count)
                {
                    this.HistoryPosition = this.History.Count - 1;
                }
                if (this.History.Count > 0 && this.History.Count > this.HistoryPosition)
                {
                    this.ClearBuffer();
                    var input = this.History[this.HistoryPosition];
                    this.Editor.Append(input);
                    this.EditDistance = this.Editor.Length;
                }
            }
            else
            {
                this.Editor.Insert(this.EditDistance, key.KeyChar);
                this.EditDistance++;
            }
            
            return this.Editor.ToString();
        }
        
        public void ClearBuffer()
        {
            this.Editor.Clear();
            this.EditDistance = 0;
        }
    }
}
