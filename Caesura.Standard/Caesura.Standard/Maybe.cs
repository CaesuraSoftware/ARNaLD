
using System;

namespace Caesura.Standard
{
    using System.Collections.Generic;
    using System.Linq;
    
    public struct Unit
    {
        
    }
    
    public static class Maybe
    {
        private static Unit _unit = new Unit();
        public static Unit Unit => _unit;
        public static Unit None => _unit;
    }
    
    public struct Maybe<T>
    {
        public T Value => this.GetValue();
        public Boolean HasValue => this._hasValue;
        public Boolean NoValue => !this.HasValue;
        public static Maybe<T> None => Maybe<T>.Nothing();
        
        private T _value;
        private Boolean _hasValue;
        
        public Maybe(T item)
        {
            this._value = item;
            if ((!typeof(T).IsValueType) && item == default)
            {
                this._hasValue = false;
            }
            else
            {
                this._hasValue = true;
            }
        }
        
        public static Maybe<T> Some(T item)
        {
            return new Maybe<T>(item);
        }
        
        public static Maybe<T> Nothing()
        {
            return new Maybe<T>();
        }
        
        public static implicit operator Maybe<T>(T item)
        {
            return new Maybe<T>(item);
        }
        
        public static implicit operator Maybe<T>(Unit unit)
        {
            return Maybe<T>.Nothing();
        }
        
        public static Boolean operator == (Maybe<T> m1, Maybe<T> m2)
        {
            return m1.Equals(m2);
        }
        
        public static Boolean operator != (Maybe<T> m1, Maybe<T> m2)
        {
            return !(m1 == m2);
        }
        
        private T GetValue()
        {
            if (!this._hasValue)
            {
                throw new InvalidOperationException("Value is not set.");
            }
            return this._value;
        }
        
        public override Boolean Equals(Object obj)
        {
            if (obj is Maybe<T> maybe)
            {
                if (this.HasValue && maybe.HasValue)
                {
                    return (this.Value?.Equals(maybe.Value)) ?? false;
                }
            }
            return false;
        }
        
        public override Int32 GetHashCode()
        {
            if (this.HasValue)
            {
                return this.Value?.GetHashCode() ?? -1;
            }
            return -1;
        }
        
        public override String ToString()
        {
            if (this._hasValue)
            {
                return this._value?.ToString();
            }
            return "<None>";
        }
    }
}
