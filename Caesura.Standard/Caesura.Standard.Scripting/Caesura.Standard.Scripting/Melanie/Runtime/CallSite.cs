
using System;

namespace Caesura.Standard.Scripting.Melanie.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using Types;
    
    public class ExtCallSite : CallSite<IMelType>
    {
        public String Name { get; set; }
        public Action<Context> Caller { get; set; }
        
        public ExtCallSite() : base(OpCode.Call)
        {
            
        }
        
        public void Execute(Context context)
        {
            this.Caller.Invoke(context);
        }
    }
    
    public class CallSite<T> where T : IMelType
    {
        public Int64 LineNumber { get; set; }
        public OpCode Code { get; set; }
        private List<T> _arguments;
        public List<T> Arguments 
        { 
            get
            {
                if (this._arguments is null)
                {
                    this._arguments = new List<T>();
                }
                return this._arguments;
            }
            set => this._arguments = value;
        }
        
        public CallSite()
        {
            
        }
        
        public CallSite(OpCode code)
        {
            this.Code = code;
        }
        
        public CallSite(OpCode code, params T[] args)
        {
            this.Arguments.AddRange(args);
        }
    }
}
