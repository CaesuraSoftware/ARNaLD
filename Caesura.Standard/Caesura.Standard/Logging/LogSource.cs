
using System;

namespace Caesura.Standard.Logging
{
    using System.Collections.Generic;
    using System.Linq;
    
    public class LogSource
    {
        public LogSource Constructor => this + ".ctor";
        
        public String Method { get; set; }
        public String Class { get; set; }
        public String Namespace { get; set; }
        public Guid Guid { get; set; }
        
        public LogSource()
        {
            
        }
        
        public LogSource(LogSource ls)
        {
            this.Copy(ls);
        }
        
        public LogSource(String namespacename, String classname, String methodname, Guid guid)
        {
            this.Namespace  = namespacename;
            this.Class      = classname;
            this.Method     = methodname;
            this.Guid       = guid;
        }
        
        public LogSource(String namespacename, String classname, Guid guid) : this(namespacename, classname, null, guid)
        {
            
        }
        
        public LogSource(String namespacename, String classname, String methodname)  : this(namespacename, classname, methodname, Guid.Empty)
        {
            
        }
        
        public LogSource(String namespacename, String classname) : this(namespacename, classname, null)
        {
            
        }
        
        public LogSource(String namespacename) : this(namespacename, null)
        {
            
        }
        
        public void Copy(LogSource ls)
        {
            this.Namespace  = ls.Namespace;
            this.Class      = ls.Class;
            this.Method     = ls.Method;
            this.Guid       = ls.Guid;
        }
        
        public LogSource AndClass(String classname)
        {
            return this + classname;
        }
        
        public static LogSource operator + (LogSource ls1, LogSource ls2)
        {
            var nls = new LogSource();
            nls.Method      = ls1.Method    ?? ls2.Method;
            nls.Class       = ls1.Class     ?? ls2.Class;
            nls.Namespace   = ls1.Namespace ?? ls2.Namespace;
            nls.Guid        = ls1.Guid != Guid.Empty ? ls1.Guid : ls2.Guid;
            return nls;
        }
        
        public static LogSource operator + (LogSource ls, String s)
        {
            var nls = new LogSource(ls);
            nls.Method = s;
            return nls;
        }
        
        public override String ToString()
        {
            return this.ToString(".", "P");
        }
        
        public String ToString(String namespaceSeperator, String guidFormatter)
        {
            var str = String.Empty;
            if (this.Guid != Guid.Empty)
            {
                str += this.Guid.ToString(guidFormatter).ToUpper();
            }
            if (!String.IsNullOrEmpty(this.Namespace))
            {
                if (!String.IsNullOrEmpty(str))
                {
                    str += " ";
                }
                str += this.Namespace;
            }
            if (!String.IsNullOrEmpty(this.Class))
            {
                if (str.EndsWith(this.Namespace))
                {
                    str += namespaceSeperator;
                }
                str += this.Class;
            }
            if (!String.IsNullOrEmpty(this.Method))
            {
                if (str.EndsWith(this.Class))
                {
                    str += namespaceSeperator;
                }
                str += this.Method;
            }
            if (!String.IsNullOrEmpty(str))
            {
                return str;
            }
            return null;
        }
    }
}
