
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime.Types
{
    using System.Collections.Generic;
    using System.Linq;
    
    public class MelFunc : IMelType
    {
        // TODO: not even sure what I'm planning on doing
        // with this yet. I actually want MelObject to
        // contain a callback and act as a function and
        // pretty much work like how JavaScript works where
        // functions and objects are the same thing.
        
        public MelFunc()
        {
            
        }
        
        public Boolean Convert(Int32 size, Byte[] bytes)
        {
            throw new NotImplementedException();
        }
        
        public IMelType Copy()
        {
            throw new NotImplementedException();
        }
    }
    
    public class MelString : MelObject, IMelType<String>
    {
        private String _melStringRep = "__MelString";
        public String InternalRepresentation
        { 
            get
            {
                if (this.Fields.ContainsKey(this._melStringRep))
                {
                    var rawmel = this.Fields[this._melStringRep];
                    var melstr = rawmel as String;
                    return melstr;
                }
                return null;
            }
            set
            {
                if (this.Fields.ContainsKey(this._melStringRep))
                {
                    this.Fields.Remove(this._melStringRep);
                }
                this.Fields.Add(this._melStringRep, value);
            }
        }
        
        public MelString()
        {
            
        }
        
        public MelString(String str)
        {
            this.InternalRepresentation = str;
        }
        
        public override Boolean Convert(Int32 size, Byte[] bytes)
        {
            // TODO:
            throw new NotImplementedException();
        }
        
        public override IMelType Copy()
        {
            var mtb = base.Copy();
            var mt = mtb as MelString;
            mt.InternalRepresentation = this.InternalRepresentation;
            return mt;
        }
    }
    
    public class MelObject : IMelType
    {
        public Dictionary<String, Object> Fields { get; set; }
        
        public MelObject()
        {
            this.Fields = new Dictionary<String, Object>();
        }
        
        public MelObject(MelObject obj) : this()
        {
            foreach (var f in obj.Fields)
            {
                if (f.Value is IMelType mt)
                {
                    this.Fields.Add(f.Key, mt.Copy());
                }
                else
                {
                    this.Fields.Add(f.Key, f.Value);
                }
            }
        }
        
        public MelObject(Dictionary<String, Object> fields) : this()
        {
            foreach (var f in fields)
            {
                if (f.Value is IMelType mt)
                {
                    this.Fields.Add(f.Key, mt.Copy());
                }
                else
                {
                    this.Fields.Add(f.Key, f.Value);
                }
            }
        }
        
        public virtual Boolean Convert(Int32 size, Byte[] bytes)
        {
            throw new NotImplementedException();
        }
        
        public virtual IMelType Copy()
        {
            return new MelObject(this.Fields);
        }
    }
    
    public class MelPointer : IMelType<Int64>
    {
        public Int64 InternalRepresentation { get; set; }
        public MelObject Resource { get; set; }
        
        public MelPointer()
        {
            
        }
        
        public MelPointer(Int64 num)
        {
            this.InternalRepresentation = num;
        }
        
        public MelPointer(Int64 num, MelObject obj) : this(num)
        {
            this.Resource = obj;
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
