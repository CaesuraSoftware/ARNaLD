
using System;

namespace Caesura.Standard
{
    using System.Collections.Generic;
    using System.Linq;
    
    public static class NumberExtensions
    {
        /// <summary>
        /// Create an array of integers starting with begin and incrementing
        /// by one until reaching end. Both arguments are inclusive.
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static IEnumerable<Int32> To(this Int32 begin, Int32 end)
        {
            if (end <= begin)
            {
                throw new ArgumentException("end cannot be equal to or greater than begin");
            }
            if (end < 0 || begin < 0)
            {
                throw new ArgumentOutOfRangeException("begin and end must be positive integers");
            }
            
            var max = (end - begin) + 1;
            var nums = new Int32[max];
            var incrementer = begin;
            for (var i = 0; i < max; i++)
            {
                nums[i] = incrementer;
                incrementer++;
            }
            return nums;
        }
    }
}
