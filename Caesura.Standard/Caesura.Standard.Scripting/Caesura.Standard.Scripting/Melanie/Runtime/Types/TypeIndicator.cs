
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime.Types
{
    using System.Collections.Generic;
    using System.Linq;
    
    public enum TypeIndicator : Byte
    {
        None    = 0x00,
        Pointer = 0x03,
        Char    = 0x09,
        Int8    = 0x10,
        Int16   = 0x11,
        Int32   = 0x12,
        Int64   = 0x13,
        UInt64  = 0x14,
        Single  = 0x20,
        Double  = 0x21,
        Boolean = 0x22,
        Func    = 0x33,
        Object  = 0x34,
        String  = 0x35,
        Array   = 0x36,
    }
}
