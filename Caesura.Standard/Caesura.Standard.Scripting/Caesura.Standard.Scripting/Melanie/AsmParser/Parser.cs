
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
        
        public void ContextParse(String lines)
        {
            var nlines = this.Parse(lines);
            foreach (var line in nlines)
            {
                this.ContextHandle.AddCaller(line.LineNumber, line);
            }
            this.ContextHandle.VerifyListing();
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
                if (ncs)
                {
                    var cs = ncs.Value;
                    if (cs.Code == OpCode.Def && cs.Arguments.Count > 0 && cs.Arguments[0] is MelString ms)
                    {
                        var fd = ms.InternalRepresentation;
                        cs.FunctionDef = fd;
                        if (css.Exists(x => x.FunctionDef == fd))
                        {
                            throw new InvalidOperationException($"Duplicate function definition \"{fd}\"");
                        }
                    }
                    css.Add(cs);
                }
            }
            return css;
        }
        
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
                nopcs.LineNumber = lineNumber;
                return Maybe<CallSite<IMelType>>.Some(nopcs);
            }
            
            var opCode = this.GetOpCode(rawLine);
            
            if (opCode == OpCode.Nop)
            {
                // no-op does not need arguments, so ignore this rest of the text
                // and just return a no-op.
                var nopcs = new CallSite<IMelType>(OpCode.Nop);
                nopcs.LineNumber = lineNumber;
                return Maybe<CallSite<IMelType>>.Some(nopcs);
            }
            
            var rawLineNoOpCode = this.GetLineAfterOpCode(rawLine).TrimStart();
            
            if (String.IsNullOrEmpty(rawLineNoOpCode)
            || String.IsNullOrWhiteSpace(rawLineNoOpCode)
            || rawLineNoOpCode.Trim().StartsWith(";"))
            {
                // line has no arguments, return the opcode.
                var opcs = new CallSite<IMelType>(opCode);
                opcs.LineNumber = lineNumber;
                return Maybe<CallSite<IMelType>>.Some(opcs);
            }
            
            var args = this.GetArguments(rawLineNoOpCode);
            var aopcs = new CallSite<IMelType>(opCode);
            aopcs.LineNumber = lineNumber;
            aopcs.Arguments = args.ToList();
            return Maybe<CallSite<IMelType>>.Some(aopcs);
        }
        
        private UInt64 GetLineNumber(String line)
        {
            var linenums = String.Empty;
            var linenum = 0UL;
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
            var success = UInt64.TryParse(linenums, out linenum);
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
            line = line.TrimStart();
            var rawop = String.Empty;
            foreach (var c in line)
            {
                if (Char.IsWhiteSpace(c) || c == ';')
                {
                    break;
                }
                rawop += c;
            }
            var success = Enum.TryParse<OpCode>(rawop, true, out var code);
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
            
            var hitArgs = false;
            var instr = false;
            var escap = false;
            var str = String.Empty;
            var index = 0;
            foreach (var c in line)
            {
                /**/ if (!hitArgs)
                {
                    // wait until we go past the instruction and hit the arguments
                    if (c == ' ')
                    {
                        hitArgs = true;
                    }
                    index++;
                    continue;
                }
                
                /**/ if (c == '"' && !instr)
                {
                    // start of string, begin parsing string
                    instr = true;
                }
                else if (c =='\\' && instr)
                {
                    if (escap)
                    {
                        // we're escaping a backslash, so add it
                        escap = false;
                        str += c;
                    }
                    else
                    {
                        // switch to escape mode
                        escap = true;
                    }
                }
                else if (c == '"' && instr)
                {
                    if (escap)
                    {
                        // we're escaping the current character, so just continue to add it.
                        escap = false;
                        str += c;
                    }
                    else
                    {
                        // end of string, parse it.
                        instr = false;
                        var str_arg = new MelString(str);
                        args.Add(str_arg);
                        str = String.Empty;
                    }
                }
                else if (escap && instr)
                {
                    // general escape character handling
                    /**/ if (c == 'n')
                    {
                        str += '\n'; // newline
                    }
                    else if (c == 'r')
                    {
                        str += '\r'; // carriage return
                    }
                    else if (c == 'a')
                    {
                        str += '\a'; // bell (alert)
                    }
                    else if (c == 'b')
                    {
                        str += '\b'; // backspace
                    }
                    else if (c == 't')
                    {
                        str += '\t'; // horizontal line
                    }
                    else if (c == 'v')
                    {
                        str += '\v'; // vertical tab
                    }
                    else if (c =='\'')
                    {
                        str += '\''; // single quote
                    }
                    else
                    {
                        throw new InvalidOperationException($"Cannot escape character '{c}'");
                    }
                    escap = false;
                }
                else if (c == '*' && !instr)
                {
                    // argument is to pop a value from the stack
                    var drarg = new MelString("*");
                    args.Add(drarg);
                    str = String.Empty;
                }
                else if (c == '[' && !instr)
                {
                    str += c;
                    instr = true;
                }
                else if (c == ']' && instr && str.StartsWith("["))
                {
                    // external function call
                    str += c;
                    var drarg = new MelString(str);
                    args.Add(drarg);
                    instr = false;
                    str = String.Empty;
                }
                else if ((Char.IsWhiteSpace(c) && !instr)
                     || (c == ';' && !instr) // comment
                     || (index == line.Length - 1))
                {
                    // end of non-string argument or end of arguments
                    // if last argument isn't a string.
                    
                    if ((str.Length > 0 && Int32.TryParse(str[0].ToString(), out _))
                    || ( str.Length > 1 && Int32.TryParse(str[1].ToString(), out _)))
                    {
                        var arg = this.ParseNumberArgument(str);
                        args.Add(arg);
                    }
                    str = String.Empty;
                    
                    break;
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
            var numstr = String.Empty;
            var hasDot = false;
            var indicator = 'I'; // default to a 32-bit integer
            foreach (var c in arg)
            {
                /**/ if (String.IsNullOrEmpty(numstr) && c == '-')
                {
                    // negative value
                    numstr += c;
                }
                else if (Int32.TryParse(c.ToString(), out var num))
                {
                    numstr += c;
                }
                else if (c == '_')
                {
                    // allow underscores in numbers
                    continue;
                }
                else if (c == '.')
                {
                    if (!hasDot)
                    {
                        numstr += c;
                        indicator = 'D'; // implicitly make this number a Double if no explicit indicator
                        hasDot = true;
                    }
                    else
                    {
                        throw new InvalidOperationException("Number has two dots");
                    }
                }
                else
                {
                    // end of number, check indicator
                    indicator = Char.ToUpper(c);
                    break;
                }
            }
            
            if (numstr.EndsWith("."))
            {
                throw new InvalidOperationException("Number cannot end in a dot");
            }
            if (numstr.Contains(".") && !(indicator == 'F' || indicator == 'D'))
            {
                throw new InvalidOperationException("Number must be a Single or Double if containing a dot");
            }
            
            var success = false;
            IMelType ret = default;
            switch (indicator)
            {
                case '~':
                    // TODO: binary
                    throw new NotImplementedException("~");
                case 'H':
                    // TODO: hex (convert to int32/64)
                    throw new NotImplementedException("H");
                case 'B':
                    if (Byte.TryParse(numstr, out var b))
                    {
                        ret = new MelInt8(b);
                        success = true;
                    }
                    break;
                case 'S':
                    if (Int16.TryParse(numstr, out var i16))
                    {
                        ret = new MelInt16(i16);
                        success = true;
                    }
                    break;
                case 'I':
                    if (Int32.TryParse(numstr, out var i32))
                    {
                        ret = new MelInt32(i32);
                        success = true;
                    }
                    break;
                case 'L':
                    if (Int64.TryParse(numstr, out var i64))
                    {
                        ret = new MelInt64(i64);
                        success = true;
                    }
                    break;
                case 'F':
                    if (Single.TryParse(numstr, out var s))
                    {
                        ret = new MelSingle(s);
                        success = true;
                    }
                    break;
                case 'D':
                    if (Double.TryParse(numstr, out var d))
                    {
                        ret = new MelDouble(d);
                        success = true;
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Unrecognized number signifier \"{indicator}\"");
            }
            
            if (success)
            {
                return ret;
            }
            
            throw new InvalidOperationException($"Unknown error parsing number \"{arg}\"");
        }
        
        private IMelType ParseSymbolArgument(String arg)
        {
            // TODO: not sure what a symbol could do in the
            // instruction set yet, possibly nothing.
            throw new NotImplementedException();
        }
    }
}
