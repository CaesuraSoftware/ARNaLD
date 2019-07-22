
using System;

namespace Caesura.Standard.Scripting.Melanie.AsmParser
{
    using System.Collections.Generic;
    using System.Linq;
    using Runtime;
    using Runtime.Instructions;
    using Runtime.Types;
    
    public class Parser
    {
        public Context ContextHandle { get; set; }
        
        public Parser()
        {
            
        }
        
        public Parser(Context context) : this()
        {
            this.ContextHandle = context;
        }
        
        // TODO: need to redo this, need to account for strings and
        // strings containing operators (like colon for the line number syntax)
        
        public Maybe<CallSite<IMelType>> ParseLine(String line)
        {
            if (String.IsNullOrEmpty(line) || String.IsNullOrWhiteSpace(line))
            {
                // nothing on this line
                return Maybe.None;
            }
            if (line.StartsWith(";"))
            {
                // this is a comment, so skip it.
                return Maybe.None;
            }
            if (!line.Contains(':'))
            {
                throw new ArgumentException("Line does not contain a colon");
            }
            
            var li = line.Split(':', ' ').ToList();
            var litemp = new List<String>();
            foreach (var item in li)
            {
                var ni = item.Trim();
                if (String.IsNullOrEmpty(ni) || String.IsNullOrWhiteSpace(ni))
                {
                    continue;
                }
                litemp.Add(ni);
            }
            li = litemp;
            
            var ln = 0L;
            var lc = OpCode.Nop;
            
            var lnsuc = Int64.TryParse(li[0], out ln);
            if (!lnsuc)
            {
                throw new ArgumentException("Line does not start with a number");
            }
            
            if (li.Count >= 2)
            {
                var lcsuc = Enum.TryParse<OpCode>(li[1], true, out var opc);
                if (lcsuc)
                {
                    lc = opc;
                }
                else
                {
                    throw new UnrecognizedOpcodeException($"Operation \"{li[1]}\" is not a valid instruction.");
                }
            }
            
            var sopargs = new List<String>();
            if (li.Count >= 3)
            {
                for (var i = 2; i < li.Count; i++)
                {
                    var arg = li[i];
                    /**/ if (arg.StartsWith(";"))
                    {
                        break;
                    }
                    else if (arg.Contains(";"))
                    {
                        var nsb = String.Empty;
                        foreach (var s in arg)
                        {
                            /**/ if (s == ';')
                            {
                                break;
                            }
                            nsb = nsb + s;
                        }
                        if (!String.IsNullOrEmpty(nsb))
                        {
                            sopargs.Add(nsb);
                        }
                        continue;
                    }
                    else
                    {
                        sopargs.Add(arg);
                    }
                }
            }
            
            var opargs = this.ParseArgs(sopargs);
            var cs = new CallSite<IMelType>(lc);
            cs.Arguments = opargs;
            
            return Maybe<CallSite<IMelType>>.Some(cs);
        }
        
        private List<IMelType> ParseArgs(IEnumerable<String> args)
        {
            var nargs = new List<IMelType>(args.Count());
            var instr = false;
            var str = String.Empty;
            foreach (var arg in args)
            {
                if (arg.StartsWith("\""))
                {
                    instr = !instr;
                }
                if (instr)
                {
                    
                }
            }
            return nargs;
        }
        
        private IMelType ParseArg(String arg)
        {
            /**/ if (Int32.TryParse(arg, out var i32arg))
            {
                return new MelInt32(i32arg);
            }
            else
            {
                throw new ArgumentException($"Argument \"{arg}\" is of an unknown or unexpected type.");
            }
        }
    }
}
