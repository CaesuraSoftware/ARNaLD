
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using Caesura.Standard;
    
    public interface IStateAtom
    {
        String Name { get; set; }
        Maybe<String> Call();
    }
}
