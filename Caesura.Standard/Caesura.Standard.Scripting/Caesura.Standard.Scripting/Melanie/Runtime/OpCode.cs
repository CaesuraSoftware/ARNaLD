
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime
{
    /// <summary>  </summary>
    public enum OpCode : Byte
    {
        // --- META --- //
        /// <summary> No Operation </summary>
        Nop         = 0x00,
        Meta        = 0x01,
        
        // --- ARITHMETIC --- //
        /// <summary> Add </summary>
        Add         = 0x03,
        /// <summary> Add with overflow protection </summary>
        AddOvp      = 0x05,
        /// <summary> Subtract </summary>
        Sub         = 0x07,
        /// <summary> Multiply </summary>
        Mul         = 0x09,
        /// <summary> Multiply with overflow protection </summary>
        MulOvp      = 0x1A,
        /// <summary> Divide </summary>
        Div         = 0x1D,
        /// <summary> Remainder of division </summary>
        Rem         = 0x1E,
        
        // --- VALUE PASSING --- //
        /// <summary> Box a primitive value </summary>
        Box         = 0x20,
        /// <summary> Unbox a primitive value </summary>
        Unbox       = 0x21,
        /// <summary> Allocate a new object </summary>
        New         = 0x22,
        /// <summary> Place a value inside an object </summary>
        Store       = 0x23,
        /// <summary> Get a value from an object </summary>
        Fetch       = 0x24,
        /// <summary> Delete an object </summary>
        Delete      = 0x25,
        
        // --- CONTROL FLOW --- //
        /// <summary> Go to a line </summary>
        Jmp         = 0x30,
        /// <summary> Branch if greater than </summary>
        Jgt         = 0x31,
        /// <summary> Branch if less than </summary>
        Jlt         = 0x32,
        /// <summary> Branch if equal </summary>
        Jeq         = 0x33,
        /// <summary> Branch if zero </summary>
        Jez         = 0x34,
        /// <summary> Branch if true </summary>
        Jet         = 0x35,
        /// <summary> Branch if false </summary>
        Jef         = 0x36,
        
        // --- BITWISE OPERATIONS --- //
        /// <summary> Bitwise NOT </summary>
        Not         = 0x40,
        /// <summary> Bitwise AND </summary>
        And         = 0x41,
        /// <summary> Bitwise NAND </summary>
        Nand        = 0x42,
        /// <summary> Bitwise OR </summary>
        Or          = 0x43,
        /// <summary> Bitwise NOR </summary>
        Nor         = 0x44,
        /// <summary> Bitwise shift left </summary>
        ShfL        = 0x45,
        /// <summary> Bitwise shift right </summary>
        ShfR        = 0x46,
        
        // --- STACK MANIPULATION --- //
        /// <summary> Set the current stack using a pointer to another stack </summary>
        SetStack    = 0x50,
        /// <summary> Push a value on to the local stack </summary>
        Push        = 0x51,
        /// <summary> Pop a value from the local stack </summary>
        Pop         = 0x52,
        /// <summary> Peek at the top element of the stack </summary>
        Peek        = 0x53,
        /// <summary> Swap the top two elements of the stack </summary>
        Swap        = 0x54,
        /// <summary> Duplicate the value on the top of the stack </summary>
        Dup         = 0x55,
        /// <summary> Clear the stack </summary>
        Clear       = 0x56,
        /// <summary> Lock on the next stack-manipulating instruction </summary>
        Lock        = 0x57,
        
        // --- FUNCTION CALLING --- //
        /// <summary> Call a function or invoke an external function </summary>
        Call        = 0x61,
        /// <summary> Define a function </summary>
        Def         = 0x62,
        /// <summary> Return from a function </summary>
        Ret         = 0x63,
        /// <summary> Invoke the debugger </summary>
        Break       = 0x64,
        /// <summary> Check for exceptions until reaching an EndTry instruction </summary>
        Try         = 0x65,
        /// <summary> End checking for exceptions </summary>
        EndTry      = 0x66,
        /// <summary> Handle an exception from an immediately previous EndTry instruction </summary>
        Catch       = 0x67,
        /// <summary> End exception handler </summary>
        EndCatch    = 0x68,
        /// <summary> Run this code regardless of results from exception handling </summary>
        Finally     = 0x69,
        /// <summary> End the finally block </summary>
        EndFinally  = 0x6A,
        /// <summary> Throw an exception </summary>
        Throw       = 0x6B,
        /// <summary> Throw an existing exception, preserving the callstack </summary>
        Rethrow     = 0x6C,
    }
}
