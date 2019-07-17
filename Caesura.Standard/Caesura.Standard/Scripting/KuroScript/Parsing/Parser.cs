
using System;

namespace Caesura.Standard.Scripting.KuroScript.Parsing
{
    using System.Collections.Generic;
    using System.Linq;
    
    // TODO: KuroScript, a simple lisp-like language that
    // has strong-typing (variable:type), type-interence,
    // and optional contextual keywords (aka "func", "begin"
    // in place of an open parenthesis, etc.)
    // Object-oriented, and scope/contexts will be objects,
    // so entire scopes can be passed around easily. They'll
    // inherently be objects, so capturing a closure will just
    // be passing the current scope object to the function, not
    // having to capture it first. so all functions will implicitly
    // start with "scope, this" arguments.
    // there will be a few global objects inherent to call scripts,
    // such as a read-only environment object, which will contain
    // the global scope (all classes), language information (version,
    // implementation, implementer company, interpreted/compiled/JIT, 
    // etc.) implementation/compiler will be called KuroScript.NETCore.
    //
    // File extension will be .ks
    
    public class Parser
    {
        
    }
}
