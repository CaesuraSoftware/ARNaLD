
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime.Instructions
{
    using System.Collections.Generic;
    using System.Linq;
    using Types;
    
    public static class NumberHelper
    {
        // TODO: number conversion functions, including implicit conversion down.
        
        public static IMelType Add(IMelType x, IMelType y)
        {
            /**/ if (x is MelInt8  xi8  && y is MelInt8  yi8 )
            {
                var nn = (Byte)(xi8.InternalRepresentation + yi8.InternalRepresentation);
                return new MelInt8(nn);
            }
            else if (x is MelInt16 xi16 && y is MelInt16 yi16)
            {
                var nn = (Int16)(xi16.InternalRepresentation + yi16.InternalRepresentation);
                return new MelInt16(nn);
            }
            else if (x is MelInt32 xi32 && y is MelInt32 yi32)
            {
                var nn = (Int32)(xi32.InternalRepresentation + yi32.InternalRepresentation);
                return new MelInt32(nn);
            }
            else if (x is MelInt64 xi64 && y is MelInt64 yi64)
            {
                var nn = (Int64)(xi64.InternalRepresentation + yi64.InternalRepresentation);
                return new MelInt64(nn);
            }
            else if (x is MelSingle xs  && y is MelSingle ys )
            {
                var nn = (Single)(xs.InternalRepresentation + ys.InternalRepresentation);
                return new MelSingle(nn);
            }
            else if (x is MelDouble xd  && y is MelDouble yd )
            {
                var nn = (Double)(xd.InternalRepresentation + yd.InternalRepresentation);
                return new MelDouble(nn);
            }
            else
            {
                throw new InvalidOperationException("Argument types do not match");
            }
        }
        
        public static IMelType Sub(IMelType x, IMelType y)
        {
            /**/ if (x is MelInt8  xi8  && y is MelInt8  yi8 )
            {
                var nn = (Byte)(xi8.InternalRepresentation - yi8.InternalRepresentation);
                return new MelInt8(nn);
            }
            else if (x is MelInt16 xi16 && y is MelInt16 yi16)
            {
                var nn = (Int16)(xi16.InternalRepresentation - yi16.InternalRepresentation);
                return new MelInt16(nn);
            }
            else if (x is MelInt32 xi32 && y is MelInt32 yi32)
            {
                var nn = (Int32)(xi32.InternalRepresentation - yi32.InternalRepresentation);
                return new MelInt32(nn);
            }
            else if (x is MelInt64 xi64 && y is MelInt64 yi64)
            {
                var nn = (Int64)(xi64.InternalRepresentation - yi64.InternalRepresentation);
                return new MelInt64(nn);
            }
            else if (x is MelSingle xs  && y is MelSingle ys )
            {
                var nn = (Single)(xs.InternalRepresentation - ys.InternalRepresentation);
                return new MelSingle(nn);
            }
            else if (x is MelDouble xd  && y is MelDouble yd )
            {
                var nn = (Double)(xd.InternalRepresentation - yd.InternalRepresentation);
                return new MelDouble(nn);
            }
            else
            {
                throw new InvalidOperationException("Argument types do not match");
            }
        }
        
        public static IMelType Div(IMelType x, IMelType y)
        {
            /**/ if (x is MelInt8  xi8  && y is MelInt8  yi8 )
            {
                var nn = (Byte)(xi8.InternalRepresentation / yi8.InternalRepresentation);
                return new MelInt8(nn);
            }
            else if (x is MelInt16 xi16 && y is MelInt16 yi16)
            {
                var nn = (Int16)(xi16.InternalRepresentation / yi16.InternalRepresentation);
                return new MelInt16(nn);
            }
            else if (x is MelInt32 xi32 && y is MelInt32 yi32)
            {
                var nn = (Int32)(xi32.InternalRepresentation / yi32.InternalRepresentation);
                return new MelInt32(nn);
            }
            else if (x is MelInt64 xi64 && y is MelInt64 yi64)
            {
                var nn = (Int64)(xi64.InternalRepresentation / yi64.InternalRepresentation);
                return new MelInt64(nn);
            }
            else if (x is MelSingle xs  && y is MelSingle ys )
            {
                var nn = (Single)(xs.InternalRepresentation / ys.InternalRepresentation);
                return new MelSingle(nn);
            }
            else if (x is MelDouble xd  && y is MelDouble yd )
            {
                var nn = (Double)(xd.InternalRepresentation / yd.InternalRepresentation);
                return new MelDouble(nn);
            }
            else
            {
                throw new InvalidOperationException("Argument types do not match");
            }
        }
        
        public static IMelType Rem(IMelType x, IMelType y)
        {
            /**/ if (x is MelInt8  xi8  && y is MelInt8  yi8 )
            {
                var nn = (Byte)(xi8.InternalRepresentation % yi8.InternalRepresentation);
                return new MelInt8(nn);
            }
            else if (x is MelInt16 xi16 && y is MelInt16 yi16)
            {
                var nn = (Int16)(xi16.InternalRepresentation % yi16.InternalRepresentation);
                return new MelInt16(nn);
            }
            else if (x is MelInt32 xi32 && y is MelInt32 yi32)
            {
                var nn = (Int32)(xi32.InternalRepresentation % yi32.InternalRepresentation);
                return new MelInt32(nn);
            }
            else if (x is MelInt64 xi64 && y is MelInt64 yi64)
            {
                var nn = (Int64)(xi64.InternalRepresentation % yi64.InternalRepresentation);
                return new MelInt64(nn);
            }
            else if (x is MelSingle xs  && y is MelSingle ys )
            {
                var nn = (Single)(xs.InternalRepresentation % ys.InternalRepresentation);
                return new MelSingle(nn);
            }
            else if (x is MelDouble xd  && y is MelDouble yd )
            {
                var nn = (Double)(xd.InternalRepresentation % yd.InternalRepresentation);
                return new MelDouble(nn);
            }
            else
            {
                throw new InvalidOperationException("Argument types do not match");
            }
        }
        
        public static IMelType Mul(IMelType x, IMelType y)
        {
            /**/ if (x is MelInt8  xi8  && y is MelInt8  yi8 )
            {
                var nn = (Byte)(xi8.InternalRepresentation * yi8.InternalRepresentation);
                return new MelInt8(nn);
            }
            else if (x is MelInt16 xi16 && y is MelInt16 yi16)
            {
                var nn = (Int16)(xi16.InternalRepresentation * yi16.InternalRepresentation);
                return new MelInt16(nn);
            }
            else if (x is MelInt32 xi32 && y is MelInt32 yi32)
            {
                var nn = (Int32)(xi32.InternalRepresentation * yi32.InternalRepresentation);
                return new MelInt32(nn);
            }
            else if (x is MelInt64 xi64 && y is MelInt64 yi64)
            {
                var nn = (Int64)(xi64.InternalRepresentation * yi64.InternalRepresentation);
                return new MelInt64(nn);
            }
            else if (x is MelSingle xs  && y is MelSingle ys )
            {
                var nn = (Single)(xs.InternalRepresentation * ys.InternalRepresentation);
                return new MelSingle(nn);
            }
            else if (x is MelDouble xd  && y is MelDouble yd )
            {
                var nn = (Double)(xd.InternalRepresentation * yd.InternalRepresentation);
                return new MelDouble(nn);
            }
            else
            {
                throw new InvalidOperationException("Argument types do not match");
            }
        }
        
        public static IMelType AddChecked(IMelType x, IMelType y)
        {
            checked
            {
                /**/ if (x is MelInt8  xi8  && y is MelInt8  yi8 )
                {
                    var nn = (Byte)(xi8.InternalRepresentation + yi8.InternalRepresentation);
                    return new MelInt8(nn);
                }
                else if (x is MelInt16 xi16 && y is MelInt16 yi16)
                {
                    var nn = (Int16)(xi16.InternalRepresentation + yi16.InternalRepresentation);
                    return new MelInt16(nn);
                }
                else if (x is MelInt32 xi32 && y is MelInt32 yi32)
                {
                    var nn = (Int32)(xi32.InternalRepresentation + yi32.InternalRepresentation);
                    return new MelInt32(nn);
                }
                else if (x is MelInt64 xi64 && y is MelInt64 yi64)
                {
                    var nn = (Int64)(xi64.InternalRepresentation + yi64.InternalRepresentation);
                    return new MelInt64(nn);
                }
                else if (x is MelSingle xs  && y is MelSingle ys )
                {
                    var nn = (Single)(xs.InternalRepresentation + ys.InternalRepresentation);
                    return new MelSingle(nn);
                }
                else if (x is MelDouble xd  && y is MelDouble yd )
                {
                    var nn = (Double)(xd.InternalRepresentation + yd.InternalRepresentation);
                    return new MelDouble(nn);
                }
                else
                {
                    throw new InvalidOperationException("Argument types do not match");
                }
            }
        }
        
        public static IMelType MulChecked(IMelType x, IMelType y)
        {
            checked
            {
                /**/ if (x is MelInt8  xi8  && y is MelInt8  yi8 )
                {
                    var nn = (Byte)(xi8.InternalRepresentation * yi8.InternalRepresentation);
                    return new MelInt8(nn);
                }
                else if (x is MelInt16 xi16 && y is MelInt16 yi16)
                {
                    var nn = (Int16)(xi16.InternalRepresentation * yi16.InternalRepresentation);
                    return new MelInt16(nn);
                }
                else if (x is MelInt32 xi32 && y is MelInt32 yi32)
                {
                    var nn = (Int32)(xi32.InternalRepresentation * yi32.InternalRepresentation);
                    return new MelInt32(nn);
                }
                else if (x is MelInt64 xi64 && y is MelInt64 yi64)
                {
                    var nn = (Int64)(xi64.InternalRepresentation * yi64.InternalRepresentation);
                    return new MelInt64(nn);
                }
                else if (x is MelSingle xs  && y is MelSingle ys )
                {
                    var nn = (Single)(xs.InternalRepresentation * ys.InternalRepresentation);
                    return new MelSingle(nn);
                }
                else if (x is MelDouble xd  && y is MelDouble yd )
                {
                    var nn = (Double)(xd.InternalRepresentation * yd.InternalRepresentation);
                    return new MelDouble(nn);
                }
                else
                {
                    throw new InvalidOperationException("Argument types do not match");
                }
            }
        }
    }
}
