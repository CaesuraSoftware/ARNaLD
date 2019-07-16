
using System;

namespace Caesura.PerformanceMonitor.Display.Views
{
    using System.Collections.Generic;
    using System.Linq;
    using Display;
    using Monitor;
    
    public class View2
    {
        private Int32 b_ThreadPage;
        public Int32 ThreadPage { get => this.b_ThreadPage; set => this.b_ThreadPage = value < 1 ? 1 : value; }
        private Int32 b_HelpIndex;
        public Int32 HelpIndex 
        { 
            get => this.b_HelpIndex; 
            set => this.b_HelpIndex = value <= 0 ? 0 : (value >= this.MaxHelpIndex ? this.MaxHelpIndex : value);
        }
        public Int32 MaxThreadsPerPage { get; set; }
        private MonitorResult LastResult { get; set; }
        private String HelpString { get; set; }
        private List<String> HelpPage { get; set; }
        private Int32 HelpWidth { get; set; }
        private Int32 MaxHelpIndex { get; set; }
        
        public View2()
        {
            this.b_ThreadPage = 1;
            this.MaxThreadsPerPage = 30;
            
            this.HelpPage = new List<String>();
            this.HelpIndex = 0;
            this.MaxHelpIndex = 0;
            
            this.HelpString = @"
Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec a diam vitae turpis accumsan sodales. Suspendisse hendrerit dapibus consectetur. Donec nibh neque, accumsan non nisi id, tristique facilisis mauris. Nulla facilisi. Cras facilisis id felis luctus viverra. Ut luctus fringilla posuere. Morbi laoreet consectetur dolor, ut iaculis libero vulputate quis. Nunc nulla leo, ultrices eu neque et, elementum tempus massa. Nulla vitae nisl vel nulla vehicula rhoncus. Aliquam ut mauris in velit venenatis elementum ut ut eros. Mauris accumsan tellus in scelerisque placerat.

Nulla consequat, erat vitae dignissim cursus, urna velit feugiat elit, nec tincidunt diam turpis varius sapien. Duis dignissim turpis at molestie commodo. Pellentesque nibh mi, porttitor nec tincidunt non, euismod quis ligula. In semper tempus lacus a ornare. Phasellus fermentum sem sed sapien tincidunt, eget malesuada dui ultrices. Phasellus elementum fringilla interdum. Nam congue finibus feugiat.

Aliquam erat volutpat. Proin scelerisque at nisl a congue. Nulla vestibulum sapien quis consequat fermentum. Donec porttitor iaculis ipsum. Integer tortor diam, congue et ipsum ut, mollis tempus sem. Proin ultrices dolor et odio dapibus, et tempus est iaculis. Nulla sodales lacinia posuere. Aliquam mi felis, ultrices vel euismod ac, pellentesque eu libero. Ut tellus nulla, lobortis ac pulvinar vel, rutrum ut orci. Nullam pulvinar pellentesque condimentum. In mattis justo lectus, venenatis rutrum neque accumsan ut. Proin vel nisi a sem consectetur dapibus.

Aliquam maximus lectus ac quam faucibus, et vestibulum ante porttitor. Maecenas massa libero, posuere ac fermentum sed, malesuada a nisi. Praesent nec ligula imperdiet eros finibus luctus. Nunc laoreet nulla vitae maximus tristique. Aliquam at condimentum ex. Vivamus porttitor metus eget neque efficitur aliquet. Donec accumsan nisi sit amet ipsum posuere, eu interdum ex maximus. Ut hendrerit ut leo non consequat. Sed ornare aliquam mollis. Vestibulum viverra enim vitae rhoncus iaculis. Sed ultricies mattis magna eu volutpat. Phasellus eleifend erat at enim semper, eu malesuada turpis consequat. Donec hendrerit tellus quis magna dictum blandit. Nunc quis lorem varius, bibendum leo aliquet, sollicitudin turpis.

Duis lobortis, ex at blandit mollis, turpis quam tristique urna, eget tincidunt sapien nisl ut quam. Nullam felis lacus, pretium sed blandit eget, mollis tempor neque. Donec pulvinar lorem at ex pulvinar, quis ornare ante congue. Phasellus luctus euismod ligula, sed ultrices dui. Aliquam ac augue lectus. Cras semper, velit eu pulvinar congue, lorem nunc maximus leo, sit amet gravida urna eros sit amet risus. Quisque ut velit malesuada, maximus eros eu, venenatis augue. Curabitur auctor ultrices erat, eget placerat justo posuere sit amet. Ut scelerisque tincidunt auctor. Etiam nec aliquam lacus. Aenean pharetra ultricies dui non consectetur. Sed leo risus, condimentum eget finibus ut, congue at mauris. Mauris eget tristique elit. In laoreet nisi tempor arcu convallis faucibus.
";
        }
        
        public void OnChange(View self)
        {
            this.HelpIndex = 0;
        }
        
        public void Init(View self, MonitorResult result)
        {
            if (this.LastResult is null)
            {
                this.LastResult = result;
            }
            Console.Title = $"Caesura Performance Monitor";
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
        }
        
        public void CmdArea(View self, MonitorResult result)
        {
            Console.SetCursorPosition(0, Console.WindowHeight - 3);
            Console.Write(new String('-', Console.WindowWidth - 1));
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.Write(new String(' ', Console.WindowWidth - 1));
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.ForegroundColor = self.TextAreaColor;
            Console.Write(self.TextArea);
            Console.ResetColor();
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write(new String('-', Console.WindowWidth - 1));
        }
        
        public void MainView(View self, MonitorResult result)
        {
            this.Init(self, result);
            
            // --- TITLE BAR --- //
            
            var prebar = "--- ";
            Console.Write(prebar);
            var title = $"Monitoring \"{result.WindowTitle}\" ({result.Name})";
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(title);
            Console.ResetColor();
            Console.Write(" ");
            Console.WriteLine(new String('-', Console.WindowWidth - (prebar.Length + title.Length) - 2));
            
            // --- SPLIT SCREEN CPU+RAM / THREADS --- //
            
            this.MaxThreadsPerPage = Console.WindowHeight - 8;
            var threadPos = this.MaxThreadsPerPage * (this.ThreadPage - 1);
            for (var i = 0; i < (Console.WindowHeight - 4); i++)
            {
                // CPU + RAM VIEW:
                var indicator = String.Empty;
                switch (i)
                {
                    case 0:
                        {
                            indicator = $"Process ID: {result.ProcessId}";
                            indicator = " " + indicator;
                        } break;
                    case 1:
                        {
                            indicator = $"Threads: {result.ThreadCount}";
                            /**/ if (result.ThreadCount > this.LastResult.ThreadCount)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                            }
                            else if (result.ThreadCount < this.LastResult.ThreadCount)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGreen;
                            }
                            indicator = " " + indicator;
                        } break;
                    case 2:
                        {
                            indicator = $"CPU: {result.ProcessorUsagePercent} %";
                            /**/ if (Math.Abs(result.ProcessorUsage - this.LastResult.ProcessorUsage) < 0.001)
                            {
                                // Equal, do nothing.
                            }
                            else if (result.ProcessorUsage > this.LastResult.ProcessorUsage)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                            }
                            else if (result.ProcessorUsage < this.LastResult.ProcessorUsage)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGreen;
                            }
                            indicator = " " + indicator;
                        } break;
                    case 3:
                        {
                            indicator = $"RAM: {(result.MemoryBytesWorkingSet / 1024).ToString("N0")} K ({result.MemoryMegabytesWorkingSet.ToString("N0")} MB)";
                            /**/ if (result.MemoryBytesWorkingSet  > this.LastResult.MemoryBytesWorkingSet)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                            }
                            else if (result.MemoryBytesWorkingSet  < this.LastResult.MemoryBytesWorkingSet)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGreen;
                            }
                            indicator = " " + indicator;
                        } break;
                    case 5:
                        {
                            indicator = new String('-', (((Console.WindowWidth - 5) / 2)));
                        } break;
                    default:
                        indicator = String.Empty;
                        break;
                }
                Console.Write(indicator);
                var size = ((Console.WindowWidth - 4) / 2) - indicator.Length;
                if (size >= 0)
                {
                    Console.Write(new String(' ', size));
                }
                Console.ResetColor();
                Console.Write("| ");
                
                // THREAD VIEW:
                /**/ if (i < Console.WindowHeight - 11)
                {
                    for (var j = 0; j < 2; j++)
                    {
                        if (threadPos < result.Threads.Count)
                        {
                            var t = result.Threads.ElementAt(threadPos);
                            var ti = $"Thread {t.ThreadId.ToString().PadRight(6)} % {t.ProcessorUsagePercent.PadRight(6)}";
                            if (!this.LastResult.Threads.Exists(x => x.ThreadId == t.ThreadId))
                            {
                                Console.BackgroundColor = ConsoleColor.DarkCyan;
                            }
                            else
                            {
                                var nt = this.LastResult.Threads.Find(x => x.ThreadId == t.ThreadId);
                                /**/ if (Math.Abs(t.ProcessorUsage - nt.ProcessorUsage) < 0.001)
                                {
                                    // Equal, do nothing.
                                }
                                else if (t.ProcessorUsage > nt.ProcessorUsage)
                                {
                                    Console.BackgroundColor = ConsoleColor.DarkRed;
                                }
                                else if (t.ProcessorUsage < nt.ProcessorUsage)
                                {
                                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                                }
                            }
                            Console.Write(ti);
                            Console.ResetColor();
                            Console.Write(new String(' ', 3));
                            threadPos++;
                        }
                        else
                        {
                            Console.Write(new String(' ', 26));
                        }
                    }
                }
                else if (i == Console.WindowHeight - 11)
                {
                    var pagenum = (result.ThreadCount / this.MaxThreadsPerPage) + 1;
                    var pageguide = $"--- Page {this.ThreadPage} of {pagenum} (▲/◄ = Back ▼/► = Forward) ";
                    var cutoff = ((Console.WindowWidth - 4) / 2) - pageguide.Length;
                    if (cutoff >= 0)
                    {
                        Console.Write(pageguide);
                        Console.Write(new String('-', cutoff));
                    }
                }
                Console.WriteLine();
            }
            
            this.CmdArea(self, result);
            
            this.LastResult = result;
        }
        
        public void HelpView(View self, MonitorResult result)
        {
            this.Init(self, result);
            
            if (this.HelpWidth != Console.WindowWidth)
            {
                this.HelpPage.Clear();
                
                var helplen = Console.WindowWidth - 6;
                var words1 = this.HelpString.Split(' ').ToList();
                var words2 = new System.Text.StringBuilder();
                foreach (var word1 in words1)
                {
                    var nword = word1
                        .Replace("\r", "")
                        .Replace("\n", "\n ");
                    words2.Append(" " + nword);
                }
                words1 = words2.ToString().Split(' ').ToList();
                var newline = String.Empty;
                foreach (var word1 in words1)
                {
                    var nword = word1
                        .Replace("\r", String.Empty);
                    /**/ if (nword.Contains("\n"))
                    {
                        nword = nword.Replace('\n', ' ');
                        if (newline.Length + nword.Length > helplen)
                        {
                            this.HelpPage.Add(newline.TrimStart());
                            newline = nword;
                        }
                        else
                        {
                            newline = $"{newline} {nword}";
                            this.HelpPage.Add(newline.TrimStart());
                            newline = String.Empty;
                        }
                    }
                    else if (newline.Length + nword.Length > helplen)
                    {
                        this.HelpPage.Add(newline.TrimStart());
                        newline = nword;
                        newline = $"{newline} {nword}";
                    }
                    else
                    {
                        newline = $"{newline} {nword}";
                    }
                }
                if (this.HelpPage.Count > 0 && this.HelpPage.ElementAt(this.HelpPage.Count - 1) == newline)
                {
                    this.HelpPage.Add(newline.TrimStart());
                }
                this.HelpWidth = Console.WindowWidth;
                this.MaxHelpIndex = this.HelpPage.Count - 1;
            }
            
            for (var i = 0; i < Console.WindowHeight - 4; i++)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.ForegroundColor = ConsoleColor.White;
                var line = "   ";
                /**/ if (i == 0)
                {
                    line = "▲  ";
                }
                else if (i == Console.WindowHeight - 5)
                {
                    line = "▼  ";
                }
                else if (((i - 1) + this.HelpIndex) >= 0 && ((i - 1) + this.HelpIndex) < this.HelpPage.Count)
                {
                    line = line + this.HelpPage.ElementAt((i - 1) + this.HelpIndex);
                }
                Console.Write(line);
                Console.WriteLine(new String(' ', Console.WindowWidth - line.Length - 1));
            }
            var lastline = $"--- Viewing HELP --- Navigation: ▲/◄ = Page Up ▼/► = Page Down ";
            Console.Write(lastline);
            Console.WriteLine(new String('-', Console.WindowWidth - lastline.Length - 1));
            Console.ResetColor();
            
            this.CmdArea(self, result);
        }
    }
}
