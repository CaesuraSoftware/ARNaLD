
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    using Caesura.Standard;
    
    public class Locator
    {
        public String Name { get; set; }
        public Guid Identifier { get; set; }
        private List<IAgent> Agents { get; set; }
        
        public Locator()
        {
            this.Agents = new List<IAgent>();
            this.Identifier = Guid.NewGuid();
        }
        
        public Locator(String name) : this()
        {
            this.Name = name;
        }
        
        public Maybe<IAgent> Find(Predicate<IAgent> predicate)
        {
            var agent = this.Agents.Find(predicate);
            if (agent is null)
            {
                return Maybe<IAgent>.None;
            }
            return Maybe<IAgent>.Some(agent);
        }
        
        public Maybe<IAgent> Find(String name)
        {
            return this.Find(x => x.Name == name);
        }
        
        public IEnumerable<IAgent> FindAll(Predicate<IAgent> predicate)
        {
            return this.Agents.FindAll(predicate);
        }
        
        public void Load(IAgent agent)
        {
            var success = this.TryLoad(agent);
            if (!success)
            {
                throw new ElementExistsException();
            }
        }
        
        public Boolean TryLoad(IAgent agent)
        {
            if (this.Find(agent.Name).HasValue)
            {
                return false;
            }
            this.Agents.Add(agent);
            return true;
        }
        
        public Boolean Remove(Predicate<IAgent> predicate)
        {
            var agent = this.Find(predicate);
            if (agent.HasValue)
            {
                return this.Agents.Remove(agent.Value);
            }
            return false;
        }
        
        public Boolean Remove(String name)
        {
            return this.Remove(x => x.Name == name);
        }
        
        public Boolean Remove(IAgent agent)
        {
            return this.Remove(agent.Name);
        }
    }
}
