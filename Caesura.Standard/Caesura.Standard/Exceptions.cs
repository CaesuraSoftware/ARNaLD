
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
    
    /// <summary>
    /// Thrown when attempting to add an element to a collection type that already contains that element.
    /// </summary>
    public class ElementExistsException : Exception
    {
        public ElementExistsException() : base() { }
        public ElementExistsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public ElementExistsException(String message) : base(message) { }
        public ElementExistsException(String message, Exception inner) : base(message, inner) { }
    }
    
    /// <summary>
    /// Thrown when attempting to get an element from a collection that does not contain it.
    /// </summary>
    public class ElementNotFoundException : Exception
    {
        public ElementNotFoundException() : base() { }
        public ElementNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public ElementNotFoundException(String message) : base(message) { }
        public ElementNotFoundException(String message, Exception inner) : base(message, inner) { }
    }
    
    /// <summary>
    /// Thrown when there was an attempt to add a value to a collection that is full.
    /// </summary>
    public class CollectionFullException : Exception
    {
        public CollectionFullException() : base() { }
        public CollectionFullException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public CollectionFullException(String message) : base(message) { }
        public CollectionFullException(String message, Exception inner) : base(message, inner) { }
    }
}
