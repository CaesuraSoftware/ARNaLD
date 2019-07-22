
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
        
        public IEnumerable<CallSite<IMelType>> Parse(String lines)
        {
            var nlines = lines.Split('\n');
            var css = this.ParseLines(nlines);
            return css;
        }
        
        public IEnumerable<CallSite<IMelType>> ParseLines(IEnumerable<String> lines)
        {
            var css = new List<CallSite<IMelType>>(lines.Count());
            foreach (var line in lines)
            {
                var ncs = this.ParseLine(line);
                if (ncs.HasValue)
                {
                    css.Add(ncs.Value);
                }
            }
            return css;
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
            if (line.TrimStart().StartsWith(";"))
            {
                // this is a comment, so skip it.
                return Maybe.None;
            }
            if (!line.Contains(':'))
            {
                throw new ArgumentException("Line does not contain a line number indicator (colon)");
            }
            
            var lineNumber = this.GetLineNumber(line);
            var rawLine = this.GetLineAfterNumber(line);
            
            if (String.IsNullOrEmpty(rawLine) || String.IsNullOrWhiteSpace(rawLine))
            {
                // line contained a line number but no opcode, treat it as a no-op.
                var nopcs = new CallSite<IMelType>(OpCode.Nop);
                return Maybe<CallSite<IMelType>>.Some(nopcs);
            }
            
            var opCode = this.GetOpCode(rawLine);
            
            if (opCode == OpCode.Nop)
            {
                // no-op does not need arguments, so ignore this rest of the text
                // and just return a no-op.
                var nopcs = new CallSite<IMelType>(OpCode.Nop);
                return Maybe<CallSite<IMelType>>.Some(nopcs);
            }
            
            var rawLineNoOpCode = this.GetLineAfterOpCode(rawLine).TrimStart();
            
            if (String.IsNullOrEmpty(rawLineNoOpCode)
            || String.IsNullOrWhiteSpace(rawLineNoOpCode)
            || rawLineNoOpCode.Trim().StartsWith(";"))
            {
                // line has no arguments, return the opcode.
                var opcs = new CallSite<IMelType>(opCode);
                return Maybe<CallSite<IMelType>>.Some(opcs);
            }
            
            var args = this.GetArguments(rawLineNoOpCode);
            var aopcs = new CallSite<IMelType>(opCode);
            aopcs.Arguments = args.ToList();
            return Maybe<CallSite<IMelType>>.Some(aopcs);
        }
        
        private Int64 GetLineNumber(String line)
        {
            var linenums = String.Empty;
            var linenum = 0L;
            foreach (var c in line)
            {
                /**/ if (Int32.TryParse(c.ToString(), out var num))
                {
                    linenums += c;
                }
                else if (c == ':')
                {
                    break;
                }
                else if (Char.IsWhiteSpace(c)
                     ||  c == '_')
                {
                    continue;
                }
                else
                {
                    throw new ArgumentException("Line did not start with a valid line number.");
                }
            }
            var success = Int64.TryParse(linenums, out linenum);
            if (!success)
            {
                throw new ArgumentException("Line did not start with a valid line number.");
            }
            return linenum;
        }
        
        private String GetLineAfterNumber(String line)
        {
            var index = line.IndexOf(':');
            var nline = line.Substring(index + 1);
            return nline;
        }
        
        private OpCode GetOpCode(String line)
        {
            var rawop = String.Empty;
            foreach (var c in line)
            {
                if (Char.IsWhiteSpace(c) || c == ';')
                {
                    break;
                }
                rawop += c;
            }
            var success = Enum.TryParse<OpCode>(rawop, out var code);
            if (!success)
            {
                throw new UnrecognizedOpcodeException($"Unrecognized operation \"{rawop}\".");
            }
            return code;
        }
        
        private String GetLineAfterOpCode(String line)
        {
            var rawop = String.Empty;
            foreach (var c in line)
            {
                if (Char.IsWhiteSpace(c) || c == ';')
                {
                    break;
                }
                rawop += c;
            }
            var nline = line.Substring(rawop.Length + 1);
            return nline;
        }
        
        private IEnumerable<IMelType> GetArguments(String line)
        {
            var args = new List<IMelType>(3); // can't have many more args than that
            
            var instr = false;
            var str = String.Empty;
            var index = 0;
            foreach (var c in line)
            {
                /**/ if (c == '"' && !instr)
                {
                    // start of string, begin parsing string
                    instr = true;
                }
                else if (c == '"' && instr)
                {
                    // end of string, parse it.
                    instr = false;
                    var str_arg = new MelString(str);
                    args.Add(str_arg);
                    str = String.Empty;
                }
                else if ((Char.IsWhiteSpace(c) && !instr)
                     || (index == line.Length - 1))
                {
                    // end of non-string argument or end of arguments
                    // if last argument isn't a string.
                    var num_arg = this.ParseNumberArgument(str);
                    args.Add(num_arg);
                    str = String.Empty;
                }
                else
                {
                    // part of the argument, continue adding
                    str += c;
                }
                index++;
            }
            
            return args;
        }
        
        private IMelType ParseNumberArgument(String arg)
        {
            throw new NotImplementedException();
        }
    }
}
