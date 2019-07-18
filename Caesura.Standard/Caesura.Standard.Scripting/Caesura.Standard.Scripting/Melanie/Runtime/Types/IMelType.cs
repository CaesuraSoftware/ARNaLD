
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime.Types
{
    using System.Collections.Generic;
    using System.Linq;
    
    public interface IMelType<T> : IMelType
    {
        T InternalRepresentation { get; set; }
    }
    
    public interface IMelType
    {
        String Name { get; }
        Boolean IsType(Byte[] item);
        void Convert(Byte[] item);
    }
}
