
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using Types;
    
    public class Context
    {
        public Interpreter Environment { get; set; }
        public Stack Stack { get; set; }
        public Stack Arguments { get; set; }
        public List<ExtCallSite> ExternalCallSites { get; set; }
        public Dictionary<UInt64, CallSite<IMelType>> Listing { get; set; }
        public UInt64 ProgramCounter { get; set; }
        public List<UInt64> CallStack { get; set; }
        
        public Context()
        {
            this.Stack              = new Stack();
            this.Arguments          = new Stack(3);
            this.ExternalCallSites  = new List<ExtCallSite>();
            this.Listing            = new Dictionary<UInt64, CallSite<IMelType>>();
            this.CallStack          = new List<UInt64>();
            this.ProgramCounter     = 0;
        }
        
        public Context(Interpreter handle) : this()
        {
            this.Environment = handle;
        }
        
        public void Run()
        {
            var last = this.Listing.Last();
            while (true)
            {
                if (this.Listing.ContainsKey(this.ProgramCounter))
                {
                    var cs = this.Listing[this.ProgramCounter];
                    var instruction = cs.Code;
                    
                    try
                    {
                        if (cs.Arguments.Count > 0)
                        {
                            this.Environment.ParseInstruction(cs.Code, cs.Arguments);
                        }
                        else
                        {
                            this.Environment.ParseInstruction(cs.Code);
                        }
                    }
                    catch (Exception e)
                    {
                        // TODO: append line number to proper Melanie exception
                        var ne = new Exception($"Melanie Line Number {this.ProgramCounter}: {e.Message}", e);
                        throw ne;
                    }
                }
                
                if (this.ProgramCounter >= last.Key)
                {
                    break;
                }
                this.ProgramCounter++;
            }
        }
        
        public void ExternalCall(String name)
        {
            var extcall = this.ExternalCallSites.Find(x => String.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
            if (extcall is null)
            {
                throw new ElementNotFoundException($"External function \"{name}\" not found");
            }
            extcall.Execute(this);
        }
        
        public void Call(UInt64 line)
        {
            this.CallStack.Add(this.ProgramCounter);
            this.ProgramCounter = line - 1; // minus 1, don't skip the line this jumps to
        }
        
        public void ReturnFromCall()
        {
            if (this.CallStack.Count == 0)
            {
                this.ProgramCounter = this.Listing.Last().Key; // if nothing to return to, exit
                return;
            }
            var line = this.CallStack.Last();
            this.CallStack.RemoveAt(this.CallStack.Count - 1);
            this.ProgramCounter = line;
        }
        
        public void PushArgument(IMelType item)
        {
            this.Arguments.Push(item);
        }
        
        public Maybe<IMelType> PopArgument()
        {
            return this.Arguments.Pop();
        }
        
        public void Push(IMelType item)
        {
            this.Stack.Push(item);
        }
        
        public Maybe<IMelType> Pop()
        {
            return this.Stack.Pop();
        }
        
        public void AddCaller(UInt64 index, CallSite<IMelType> cs)
        {
            this.Listing.Add(index, cs);
        }
        
        public void AddCaller(CallSite<IMelType> cs)
        {
            var line = 0UL;
            if (this.Listing.Count > 0)
            {
                var last = this.Listing.Last();
                line = last.Key;
            }
            this.Listing.Add(line + 1, cs);
        }
        
        public void AddCaller(OpCode code)
        {
            var cs = new CallSite<IMelType>(code);
            this.AddCaller(cs);
        }
        
        public void AddCaller(OpCode code, params IMelType[] args)
        {
            var cs = new CallSite<IMelType>(code, args);
            this.AddCaller(cs);
        }
        
        public void VerifyListing()
        {
            var num = 0UL;
            // var infunc = false;
            CallSite<IMelType> lastcall = default;
            foreach (var item in this.Listing)
            {
                if (num > item.Key)
                {
                    throw new InvalidOperationException("Listing contains out-of-order line numbers.");
                }
                num = item.Key;
                
                if (item.Value.FunctionDef == "Main")
                {
                    this.ProgramCounter = item.Key; // main function
                }
                
                lastcall = item.Value;
            }
        }
    }
}
