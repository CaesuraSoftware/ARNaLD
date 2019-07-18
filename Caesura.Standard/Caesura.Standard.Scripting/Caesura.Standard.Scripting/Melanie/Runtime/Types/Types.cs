
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
        
        public Boolean IsType(String item)
        {
            return Byte.TryParse(item, out _);
        }
        
        public void Convert(String item)
        {
            this.InternalRepresentation = Byte.Parse(item);
        }
    }
}
