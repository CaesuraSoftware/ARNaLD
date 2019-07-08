
using System;

namespace Caesura.Standard
{
    using System.Runtime.Serialization;
    
    /// <summary>
    /// Thrown when execution path reaches a state that it was never expected to.
    /// </summary>
    public class UnreachableCodeException : Exception
    {
        public UnreachableCodeException() : base() { }
        public UnreachableCodeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public UnreachableCodeException(String message) : base(message) { }
        public UnreachableCodeException(String message, Exception inner) : base(message, inner) { }
    }
}
