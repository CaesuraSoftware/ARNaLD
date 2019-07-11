
using System;

namespace Caesura.Standard
{
    using System.Collections.Generic;
    using System.Linq;
    
    public static class Mathe
    {
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
