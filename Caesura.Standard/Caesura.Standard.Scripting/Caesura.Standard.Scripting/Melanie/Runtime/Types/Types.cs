
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime.Types
{
    using System.Collections.Generic;
    using System.Linq;
    
    
    
    public class MelBoolean : IMelType<Boolean>
    {
        public Boolean InternalRepresentation { get; set; }
        
        public MelBoolean()
        {
            
        }
        
        public MelBoolean(Boolean b)
        {
            this.InternalRepresentation = b;
        }
        
        public Boolean Convert(Int32 size, Byte[] bytes)
        {
            if (size == 1 && bytes.Length == 1)
            {
                this.InternalRepresentation = bytes[0] > 0;
                return true;
            }
            return false;
        }
        
        public IMelType Copy()
        {
            return new MelBoolean(this.InternalRepresentation);
        }
    }
    
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
    
    public class MelInt64 : IMelType<Int64>
    {
        public Int64 InternalRepresentation { get; set; }
        
        
        public MelInt64()
        {
            
        }
        
        public MelInt64(Int64 num)
        {
            this.InternalRepresentation = num;
        }
        
        public Boolean Convert(Int32 size, Byte[] bytes)
        {
            if (size == 8 && bytes.Length == 8)
            {
                Int64 i64 = (
                    (bytes[7] << 56) +
                    (bytes[6] << 48) +
                    (bytes[5] << 40) +
                    (bytes[4] << 32) +
                    (bytes[3] << 24) + 
                    (bytes[2] << 16) + 
                    (bytes[1] <<  8) + 
                     bytes[0]
                );
                this.InternalRepresentation = i64;
                return true;
            }
            return false;
        }
        
        public IMelType Copy()
        {
            return new MelInt64(this.InternalRepresentation);
        }
    }
    
    public class MelSingle : IMelType<Single>
    {
        public Single InternalRepresentation { get; set; }
        
        public MelSingle()
        {
            
        }
        
        public MelSingle(Single num)
        {
            this.InternalRepresentation = num;
        }
        
        public Boolean Convert(Int32 size, Byte[] bytes)
        {
            if (size == 4 && bytes.Length == 4)
            {
                Single s = (
                    (bytes[3] << 24) + 
                    (bytes[2] << 16) + 
                    (bytes[1] <<  8) + 
                     bytes[0]
                );
                this.InternalRepresentation = s;
                return true;
            }
            return false;
        }
        
        public IMelType Copy()
        {
            return new MelSingle(this.InternalRepresentation);
        }
    }
    
    public class MelDouble : IMelType<Double>
    {
        public Double InternalRepresentation { get; set; }
        
        public MelDouble()
        {
            
        }
        
        public MelDouble(Double num)
        {
            this.InternalRepresentation = num;
        }
        
        public Boolean Convert(Int32 size, Byte[] bytes)
        {
            if (size == 8 && bytes.Length == 8)
            {
                Double d = (
                    (bytes[7] << 56) +
                    (bytes[6] << 48) +
                    (bytes[5] << 40) +
                    (bytes[4] << 32) +
                    (bytes[3] << 24) + 
                    (bytes[2] << 16) + 
                    (bytes[1] <<  8) + 
                     bytes[0]
                );
                this.InternalRepresentation = d;
                return true;
            }
            return false;
        }
        
        public IMelType Copy()
        {
            return new MelDouble(this.InternalRepresentation);
        }
    }
}
