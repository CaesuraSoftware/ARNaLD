
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    
    // TODO:
    // - implement exceptions with line numbers and other
    //   debugging information.
    // - change out all the code to use these new exceptions
    //   including the tests
    
    [Serializable]
    public class RuntimeException : Exception
    {
        public RuntimeException()
        {
            
        }
        
        public RuntimeException(string message) : base(message)
        {
            
        }
        
        public RuntimeException(string message, Exception inner) : base(message, inner)
        {
            
        }
        
        protected RuntimeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            
        }
    }
    
    [Serializable]
    public class UnrecognizedOpcodeException : RuntimeException
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
