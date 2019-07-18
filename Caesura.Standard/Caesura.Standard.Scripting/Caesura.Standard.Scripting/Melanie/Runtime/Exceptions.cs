
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    
    [Serializable]
    public class UnrecognizedOpcodeException : Exception
    {
        public OpCode OpCode { get; set; }
        
        public UnrecognizedOpcodeException()
        {
            
        }
        
        public UnrecognizedOpcodeException(OpCode code) : base($"Unknown opcode {code}")
        {
            this.OpCode = code;
        }
        
        public UnrecognizedOpcodeException(string message) : base(message)
        {
            
        }
        
        public UnrecognizedOpcodeException(string message, Exception inner) : base(message, inner)
        {
            
        }
        
        protected UnrecognizedOpcodeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            
        }
    }
}
