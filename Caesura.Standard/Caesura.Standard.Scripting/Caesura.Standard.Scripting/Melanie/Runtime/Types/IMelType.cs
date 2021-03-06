
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
        Boolean Convert(Int32 size, Byte[] bytes);
        IMelType Copy();
    }
}
