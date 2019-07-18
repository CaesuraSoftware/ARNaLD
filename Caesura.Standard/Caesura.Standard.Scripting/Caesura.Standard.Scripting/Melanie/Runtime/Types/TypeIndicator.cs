
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime.Types
{
    using System.Collections.Generic;
    using System.Linq;
    
    [Flags]
    public enum TypeIndicator : Byte
    {
        None    = 0x00,
        Int8    = 0x10,
        Int16   = 0x11,
        Int32   = 0x12,
        Int64   = 0x13,
        Single  = 0x14,
        Double  = 0x15,
        Boolean = 0x20,
        Func    = 0x30,
        Object  = 0x31,
    }
}
