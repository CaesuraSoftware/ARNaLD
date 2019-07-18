
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
        
        public Boolean Convert(Byte[] item)
        {
            // TODO: need to figure out how we want to pass
            // data to this. I'm thinking the first byte will be
            // the indicator, the next four will be the size of
            // the data, and the sixth will be where the data
            // starts (so in this case we skip right to six)
            throw new NotImplementedException();
        }
    }
}
