
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime.Types
{
    using System.Collections.Generic;
    using System.Linq;
    
    public struct MelInt8 : IMelType<Byte>
    {
        public String Name => "Int8";
        public Byte InternalRepresentation { get; set; }
        
        public MelInt8(Byte num)
        {
            this.InternalRepresentation = num;
        }
        
        public Boolean Convert(Int32 size, Byte[] item)
        {
            if (size == 1 && item.Length == 1)
            {
                this.InternalRepresentation = item[0];
                return true;
            }
            return false;
        }
    }
}
