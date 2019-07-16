
using System;

namespace Caesura.Standard
{
    using System.Collections.Generic;
    using System.Linq;
    
    public static class Mathe
    {
        public static Boolean FloatEqual(this Single a, Single b, Single range)
        {
            return Math.Abs(a - b) < range;
        }
        
        public static Boolean FloatEqual(this Single a, Single b)
        {
            return FloatEqual(a, b, 0.001f);
        }
        
        public static Boolean FloatEqual(this Double a, Double b, Double range)
        {
            return Math.Abs(a - b) < range;
        }
        
        public static Boolean FloatEqual(this Double a, Double b)
        {
            return FloatEqual(a, b, 0.001);
        }
        
        public static Int32 ConvertRange(Int32 value, Int32 originalStart, Int32 originalEnd, Int32 newStart, Int32 newEnd)
        {
            var scale = (Double)(newEnd - newStart) / (originalEnd - originalStart);
            return (Int32)(newStart + ((value - originalStart) * scale));
        }
        
        public static Int32 SafeDiv(this Int32 dividend, Int32 divisor, Int32 onZero)
        {
            if (divisor == 0)
            {
                return onZero;
            }
            return dividend / divisor;
        }
        
        public static Int32 SafeDiv(this Int32 dividend, Int32 divisor)
        {
            return SafeDiv(dividend, divisor, 0);
        }
        
        public static Int64 SafeDiv(this Int64 dividend, Int64 divisor, Int64 onZero)
        {
            if (divisor == 0)
            {
                return onZero;
            }
            return dividend / divisor;
        }
        
        public static Int64 SafeDiv(this Int64 dividend, Int64 divisor)
        {
            return SafeDiv(dividend, divisor, 0);
        }
        
        public static Int32 SafeMod(this Int32 dividend, Int32 divisor, Int32 onZero)
        {
            if (divisor == 0)
            {
                return onZero;
            }
            return dividend % divisor;
        }
        
        public static Int32 SafeMod(this Int32 dividend, Int32 divisor)
        {
            return SafeMod(dividend, divisor, 0);
        }
        
        public static Int64 SafeMod(this Int64 dividend, Int64 divisor, Int64 onZero)
        {
            if (divisor == 0)
            {
                return onZero;
            }
            return dividend % divisor;
        }
        
        public static Int64 SafeMod(this Int64 dividend, Int64 divisor)
        {
            return SafeMod(dividend, divisor, 0);
        }
    }
}
