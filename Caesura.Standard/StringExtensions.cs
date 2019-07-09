
using System;

namespace Caesura.Standard
{
    using System.Collections.Generic;
    using System.Linq;
    
    public static class StringExtensions
    {
        
        /// <summary>
        /// Wrap a string in quotes if it does not already start or end with a quote.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String Quote(this String str)
        {
            /**/ if (str.StartsWith("\"") && str.EndsWith("\""))
            {
                return str;
            }
            else if (str.StartsWith("\""))
            {
                return str + "\"";
            }
            else if (str.EndsWith("\""))
            {
                return "\"" + str;
            }
            else
            {
                return $"\"{str}\"";
            }
        }
        
        /// <summary>
        /// Generate a deterministic hashcode for a string. In .NET Core, strings 
        /// return a different hashcode per program instance. DO NOT use this for
        /// generating key values, as .NET specifically randomizes the hashcode
        /// for strings to prevent a type of denial of service attack.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Int32 DeterministicHashCode(this String str)
        {
            unchecked
            {
                Int32 hash1 = (5381 << 16) + 5381;
                Int32 hash2 = hash1;
                for (Int32 i = 0; i < str.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1)
                    {
                        break;
                    }
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }
                return hash1 + (hash2 * 1566083941);
            }
        }
        
        /// <summary>
        /// returns String.IsNullOrEmpty(str) || String.IsNullOrWhiteSpace(str).
        /// Do note this does not detect Unicode characters without the WSpace=Y property,
        /// even if they are "invisible".
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsNullEmptyWhitespace(this String str)
        {
            return String.IsNullOrEmpty(str) || String.IsNullOrWhiteSpace(str);
        }
    }
}
