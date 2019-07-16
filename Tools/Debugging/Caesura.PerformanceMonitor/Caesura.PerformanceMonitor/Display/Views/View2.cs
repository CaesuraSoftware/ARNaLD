
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
        private Int32 b_maxThreadPage;
        public Int32 ThreadPage 
        { 
            get => this.b_ThreadPage; 
            set => this.b_ThreadPage = value < 1 ? 1 : (value > this.b_maxThreadPage ? this.b_maxThreadPage : value); 
        }
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
help (command)
From Wikipedia, the free encyclopedia

In computing, help is a command in various command line shells such as COMMAND.COM, cmd.exe, Bash, 4DOS/4NT, Windows PowerShell, Singularity shell, Python, MATLAB[1] and GNU Octave.[2] It provides online information about available commands and the shell environment.[3]

Implementations
The command is available in operating systems such as DOS, IBM OS/2, Microsoft Windows, ReactOS, THEOS/OASIS,[4] Zilog Z80-RIO,[5] OS-9[6] OpenVOS,[7] HP MPE/iX,[8] Motorola VERSAdos,[9] KolibriOS[10] and also in the DEC RT-11,[11] TOPS-10[12] and TOPS-20[13] operating systems. Furthermore it is available in the open source MS-DOS emulator DOSBox and in the EFI shell.[14]

On Unix, the command is part of the Source Code Control System and prints help information for the SCCS commands.

DEC OS/8
The DEC OS/8 CCL help command prints information on specified OS/8 programs.[15]

DOS
MS-DOS
The help command is available in MS-DOS 5.x and later versions of the software. The help command with a 'command' parameter would give help on a specific command. If no arguments are provided, the command lists the contents of DOSHELP.HLP.

In MS-DOS 6.x this command exists as FASTHELP.

The MS-DOS 6.xx help command uses QBasic to view a quickhelp HELP.HLP file, which contains more extensive information on the commands, with some hyperlinking etc. The MS-DOS 6.22 help system is included on Windows 9x CD-ROM versions as well.

PC DOS
In PC DOS 5 and 6 help is the same form as MS-DOS 5 help command.

PC DOS 7.xx help uses view.exe to open OS/2 style INF files (cmdref.inf, dosrexx.inf and doserror.inf), opening these to the appropriate pages.

DR-DOS
In DR-DOS, help is a batch file that launches DR-DOS' internal help program, dosbook.

FreeDOS
The FreeDOS version was developed by Joe Cosentino.[16]

4DOS/4NT
The 4DOS/4NT help command uses a text user interface to display the online help.

Microsoft Windows cmd.exe
Used without parameters, help lists and briefly describes every system command. Windows NT-based versions use MS-DOS 5 style help. Versions before Windows Vista also have a Windows help file (NTCMDS.HLP or NTCMDS.INF) in a similar style to MS-DOS 6.

PowerShell
In PowerShell, help is a short form (implemented as a PowerShell function) for access to the Get-Help Cmdlet.

Windows PowerShell includes an extensive, console-based help system, reminiscent of man pages in Unix. The help topics include help for cmdlets, providers, and concepts in PowerShell.

GNU Bash
In Bash, the builtin command help'[17] lists all Bash builtin commands if used without arguments. Otherwise, it prints a brief summary of a command. Its syntax is:

help [-dms] [pattern]
Syntax
The command-syntax is:

help [command]
Arguments:

command This command-line argument specifies the name of the command about which information is to be displayed.
Examples
DOSBox
Z:\>help
If you want a list of all supported commands type help /all .
A short list of the most often used commands:
<DIR     > Directory View.
<CD      > Display/changes the current directory.
<CLS     > Clear screen.
<COPY    > Copy files.
...
Python
>>> help
Type help() for interactive help, or help(object) for help about object.
>>> help()

Welcome to Python 2.5!  This is the online help utility.

If this is your first time using Python, you should definitely check out
the tutorial on the Internet at https://www.python.org/doc/tut/.

Enter the name of any module, keyword, or topic to get help on writing
Python programs and using Python modules.  To quit this help utility and
return to the interpreter, just type 'quit'.
...
GNU Octave
octave-3.0.0.exe:1> help

Help is available for the topics listed below.
Additional help for built-in functions and operators is
available in the on-line version of the manual.  Use the command
`doc <topic>' to search the manual index.
...
See also
	Wikibooks has a book on the topic of: Guide to Windows Commands
Online help
List of DOS commands
References
 https://www.mathworks.com/help/matlab/ref/help.html
 https://octave.sourceforge.io/octave/function/help.html
 Microsoft TechNet Help article
 THEOS/OASIS User′s Handbook
 Z80-RIO OPERATING SYSTEM USER'S MANUAL
 Paul S. Dayan (1992). The OS-9 Guru - 1 : The Facts. Galactic Industrial Limited. ISBN 0-9519228-0-7.
 http://stratadoc.stratus.com/vos/19.1.0/r098-19/wwhelp/wwhimpl/common/html/r098-19.pdf
 MPE/iX Command Reference Manual
 M68000 Family VERSAdos System Facilities Reference Manual
 http://wiki.kolibrios.org/wiki/Shell
http://paleoferrosaurus.com/beta/documents/rt11help.html#HELP
 TOPS-10 Operating System Commands Manual (pdf). Digital Equipment Corporation. August 1980. Retrieved 2019-02-17.
 'TOPS-20 Command manual' (PDF).
 'EFI Shells and Scripting'. Intel. Retrieved 2013-09-25.
 'Concise Command Language' (CCL).'OS/8 Handbook' (PDF). April 1974. Retrieved 28 November 2017.
 http://www.ibiblio.org/pub/micro/pc-stuff/freedos/files/distributions/1.2/repos/pkg-html/help.html
 'Bash Reference Manual'. www.gnu.org. Retrieved 2016-05-09.
Further reading
Wolverton, Van (1990). MS-DOS Commands: Microsoft Quick Reference, 4th Revised edition. Microsoft Press. ISBN 978-1556152894.
Kathy Ivens; Brian Proffit (1993). OS/2 Inside & Out. Osborne McGraw-Hill. ISBN 9780078818714.
Frisch, Æleen (2001). Windows 2000 Commands Pocket Reference. O'Reilly. ISBN 978-0-596-00148-3.
";
        }
        
        public void OnChange(View self)
        {
            this.ThreadPage = 0;
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
            this.b_maxThreadPage = (result.ThreadCount / this.MaxThreadsPerPage) + 1;
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
                    var pageguide = $"--- Page {this.ThreadPage} of {this.b_maxThreadPage} (▲/◄ = Back ▼/► = Forward) ";
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
                if (this.HelpPage.Count > 0 && this.HelpPage.ElementAt(0) == String.Empty)
                {
                    this.HelpPage.RemoveAt(0);
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
            
            var height = (Console.WindowHeight - 5);
            if (this.HelpPage.Count > height)
            {
                var cy = Console.CursorLeft;
                var cx = Console.CursorTop;
                var progress = (((this.HelpIndex - 1) * (height - 1)) / (this.HelpPage.Count - 1)) + 1;
                Console.SetCursorPosition(0, progress);
                Console.Write("█");
                Console.SetCursorPosition(cy, cx);
            }
            
            var lastline = $"--- Viewing HELP --- Navigation: ▲/◄ = Page Up ▼/► = Page Down ";
            Console.Write(lastline);
            Console.WriteLine(new String('-', Console.WindowWidth - lastline.Length - 1));
            Console.ResetColor();
            
            this.CmdArea(self, result);
        }
    }
}
