
using System;

namespace Caesura.Standard
{
    using System.Collections.Generic;
    
    public interface ICopyable
    {
        void Copy(Object o);
        ICopyable Clone();
    }
    
    public interface ICopyable<T> : ICopyable
    {
        void Copy(T item);
    }
}
