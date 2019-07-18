
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime.Types
{
    using System.Collections.Generic;
    using System.Linq;
    
    public class MelInt8 : IMelType<Byte>
    {
        public Byte InternalRepresentation { get; set; }
        
        public MelInt8()
        {
            
        }
        
        public MelInt8(Byte num)
        {
            this.InternalRepresentation = num;
        }
        
        public Boolean Convert(Int32 size, Byte[] bytes)
        {
            if (size == 1 && bytes.Length == 1)
            {
                this.InternalRepresentation = bytes[0];
                return true;
            }
            return false;
        }
        
        public IMelType Copy()
        {
            return new MelInt8(this.InternalRepresentation);
        }
    }
    
    public class MelInt16 : IMelType<Int16>
    {
        public Int16 InternalRepresentation { get; set; }
        
        public MelInt16()
        {
            
        }
        
        public MelInt16(Int16 num)
        {
            this.InternalRepresentation = num;
        }
        
        public Boolean Convert(Int32 size, Byte[] bytes)
        {
            if (size == 2 && bytes.Length == 2)
            {
                Int16 i16 = (Int16)(
                    (bytes[1] << 8) + 
                     bytes[0]
                );
                this.InternalRepresentation = i16;
                return true;
            }
            return false;
        }
        
        public IMelType Copy()
        {
            return new MelInt16(this.InternalRepresentation);
        }
    }
    
    public class MelInt32 : IMelType<Int32>
    {
        public Int32 InternalRepresentation { get; set; }
        
        public MelInt32()
        {
            
        }
        
        public MelInt32(Int32 num)
        {
            this.InternalRepresentation = num;
        }
        
        public Boolean Convert(Int32 size, Byte[] bytes)
        {
            if (size == 4 && bytes.Length == 4)
            {
                Int32 i32 = (
                    (bytes[3] << 24) + 
                    (bytes[2] << 16) + 
                    (bytes[1] <<  8) + 
                     bytes[0]
                );
                this.InternalRepresentation = i32;
                return true;
            }
            return false;
        }
        
        public IMelType Copy()
        {
            return new MelInt32(this.InternalRepresentation);
        }
    }
}
