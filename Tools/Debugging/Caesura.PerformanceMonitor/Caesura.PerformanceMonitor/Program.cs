
//
// Simple Performance Monitor Console Application
// Copyright (C) Caesura Software Solutions 2019
// Released under the Caesura Software Solutions Public License version 1.
// See LICENSE.TXT in the root directory for licensing information.
//
// DESCRIPTION:
// A simple performance monitor console application that tracks the
// performance of a single application, primarily used for debugging
// purposes. Extremely simple, written in a day. More of a simple script
// than the over-engineered stuff I usually make.
// 

using System;

namespace Caesura.PerformanceMonitor
{
    using System.Collections.Generic;
    using System.Linq;
    
    class Program
    {
        // TODO:
        // - Make this wait for the process to come online so we can
        //   actually integrate this into vscode's json build system
        //   by putting this in it's pre-start phase.
        static void Main(String[] args)
        {
            try
            {
                Start(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadLine();
                throw;
            }
        }
        
        static void Start(String[] args)
        {
            // --- ARGUMENT HANDLING --- //
            
            Int32 ProcessId      = 0;
            Int32 UpdateInterval = 1000;
            Boolean AutoShutdown = true;
            
            var nargs = new List<String>(args);
            if ((nargs.Count == 1)
            && (String.Equals(nargs.First(), "-h"    , StringComparison.OrdinalIgnoreCase)
            ||  String.Equals(nargs.First(), "--help", StringComparison.OrdinalIgnoreCase) 
            ||  String.Equals(nargs.First(), "/?"    , StringComparison.OrdinalIgnoreCase) 
            ||  String.Equals(nargs.First(), "/h"    , StringComparison.OrdinalIgnoreCase)
            ||  String.Equals(nargs.First(), "/help" , StringComparison.OrdinalIgnoreCase)
            ))
            {
                Console.WriteLine("Caesura Performance Monitor");
                Console.WriteLine();
                Console.WriteLine("Usage: [Caesura.PerformanceMonitor] [-p|--pid] Int32 [-u|--update] [Int32] [-s|--shutdown] [Boolean]");
                Console.WriteLine();
                Console.WriteLine(" [-p|--pid] Int32 - Process ID for the process to monitor the resources of.");
                Console.WriteLine(" [-u|--update] [Int32] - Update interval in milliseconds. Default is 1000.");
                Console.WriteLine(" [-s|--shutdown] [Boolean] - Automatically shut down after the process to be monitored ends. Default is true.");
            }
            else if (nargs.Count == 1)
            {
                var success = Int32.TryParse(nargs[0], out var pid);
                if (success)
                {
                    ProcessId = pid;
                }
                else
                {
                    Console.WriteLine("Bad argument. Process ID argument needs a process ID. Try '--help'.");
                    return;
                }
            }
            else
            {
                var argsHandled = 0;
                
                // --- Process ID argument filter --- //
                var pidArg = nargs.Find(x => 
                   String.Equals(x, "-p"   , StringComparison.OrdinalIgnoreCase)
                || String.Equals(x, "--pid", StringComparison.OrdinalIgnoreCase)
                || String.Equals(x, "/p"   , StringComparison.OrdinalIgnoreCase)
                || String.Equals(x, "/pid" , StringComparison.OrdinalIgnoreCase)
                );
                if (pidArg != null)
                {
                    var pos = nargs.IndexOf(pidArg);
                    if (nargs.Count <= pos + 1)
                    {
                        Console.WriteLine("Bad argument. Process ID argument needs a process ID. Try '--help'.");
                        return;
                    }
                    else
                    {
                        var success = Int32.TryParse(nargs[pos + 1], out var pid);
                        if (success)
                        {
                            ProcessId = pid;
                            argsHandled += 2;
                        }
                        else
                        {
                            Console.WriteLine("Bad argument. Process ID argument needs a process ID. Try '--help'.");
                            return;
                        }
                    }
                }
                else
                {
                    var myPid = System.Diagnostics.Process.GetCurrentProcess().Id;
                    ProcessId = myPid;
                }
                
                // --- Update Interval argument filter. --- //
                var updateArg = nargs.Find(x => 
                   String.Equals(x, "-u"      , StringComparison.OrdinalIgnoreCase)
                || String.Equals(x, "--update", StringComparison.OrdinalIgnoreCase)
                || String.Equals(x, "/u"      , StringComparison.OrdinalIgnoreCase)
                || String.Equals(x, "/update" , StringComparison.OrdinalIgnoreCase)
                );
                if (updateArg != null)
                {
                    var pos = nargs.IndexOf(updateArg);
                    if (nargs.Count <= pos + 1)
                    {
                        Console.WriteLine("Bad argument. Update interval needs an integer. Try '--help'.");
                        return;
                    }
                    else
                    {
                        var success = Int32.TryParse(nargs[pos + 1], out var interval);
                        if (success && interval >= 0)
                        {
                            UpdateInterval = interval;
                            argsHandled += 2;
                        }
                        else
                        {
                            Console.WriteLine("Bad argument. Update interval needs an integer. Try '--help'.");
                            return;
                        }
                    }
                }
                else
                {
                    // no-op
                }
                
                // --- Automatic Shutdown argument filter. --- //
                var shutdownArg = nargs.Find(x => 
                   String.Equals(x, "-s"        , StringComparison.OrdinalIgnoreCase)
                || String.Equals(x, "--shutdown", StringComparison.OrdinalIgnoreCase)
                || String.Equals(x, "/s"        , StringComparison.OrdinalIgnoreCase)
                || String.Equals(x, "/shutdown" , StringComparison.OrdinalIgnoreCase)
                );
                if (shutdownArg != null)
                {
                    var pos = nargs.IndexOf(shutdownArg);
                    if (nargs.Count <= pos + 1)
                    {
                        Console.WriteLine("Bad argument. Auto-shutdown argument needs to be a boolean [True|False]. Try '--help'.");
                        return;
                    }
                    else
                    {
                        var success = Boolean.TryParse(nargs[pos + 1], out var autoshutdown);
                        if (success)
                        {
                            AutoShutdown = autoshutdown;
                            argsHandled += 2;
                        }
                        else
                        {
                            Console.WriteLine("Bad argument. Auto-shutdown argument needs to be a boolean [True|False]. Try '--help'.");
                            return;
                        }
                    }
                }
                else
                {
                    // no-op
                }
                
                if (nargs.Count > argsHandled)
                {
                    Console.WriteLine("Unrecognized argument(s). Try '--help'.");
                    return;
                }
            }
            
            // --- MAIN PROGRAM --- //
            
            var monitor     = new Monitor.Resources(ProcessId);
            var view        = new Display.View(UpdateInterval, monitor);
            var keyboard    = new Display.KeyboardHandler();
            var handler     = new Commands.CommandHandler();
            
            if (AutoShutdown)
            {
                monitor.OnTargetExit += () =>
                {
                    view.Stop();
                    view.Wait();
                    view.ClearScreen();
                    MainLoop = false;
                };
            }
            var customview = new Display.Views.View2();
            view.AddView(new Display.ViewField()
            {
                Name = "Main",
                Run = customview.MainView,
            });
            view.AddView(new Display.ViewField()
            {
                Name = "Help",
                Run = customview.HelpView,
                OnChange = customview.OnChange,
            });
            handler.Add(new Commands.Shutdown() );
            handler.Add(new Commands.Echo()     );
            handler.Add(new Commands.Refresh()  );
            handler.Add(new Commands.Clear()    );
            handler.Add(new Commands.ViewCmd()  );
            handler.Add(new Commands.Egg()      );
            
            var cmdModeMsg = " [COMMAND MODE; PRESS 'I' FOR TEXT INPUT MODE. PRESS 'ESC' FOR COMMAND MODE AGAIN.]";
            var display = String.Empty;
            view.SetInput(cmdModeMsg, ConsoleColor.DarkCyan);
            
            view.Start();
            while (MainLoop)
            {
                var input  = Console.ReadKey(true);
                var result = keyboard.Process(input);
                
                view.TextAreaColor = ConsoleColor.Gray;
                
                /**/ if (result == RequestProgramState.Exit)
                {
                    view.Stop();
                    view.Wait();
                    view.ClearScreen();
                    MainLoop = false;
                    Environment.Exit(0);
                
                }
                else if (result == RequestProgramState.PageBack)
                {
                    customview.ThreadPage--;
                    customview.HelpIndex--;
                }
                else if (result == RequestProgramState.PageForward)
                {
                    customview.ThreadPage++;
                    customview.HelpIndex++;
                }
                else if (result == RequestProgramState.TextInput)
                {
                    display = keyboard.ProcessText();
                    view.SetInput($"{view.Prompt}{display}");
                }
                else if (result == RequestProgramState.EditMode)
                {
                    view.SetInput($"{view.Prompt}{display}");
                }
                else if (result == RequestProgramState.CommandMode)
                {
                    view.SetInput(cmdModeMsg, ConsoleColor.DarkCyan);
                }
                else if (result == RequestProgramState.CommandInput)
                {
                    keyboard.ClearBuffer();
                    view.SetInput(view.Prompt);
                    var nr = handler.Run(display, view);
                    if (nr == RequestProgramState.Exit)
                    {
                        view.Stop();
                        view.Wait();
                        view.ClearScreen();
                        MainLoop = false;
                        Environment.Exit(0);
                    }
                }
            }
        }
        
        static Boolean MainLoop = true;
    }
}
