
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    using Caesura.Standard;
    
    public class Locator : IDisposable
    {
        public String Name { get; set; }
        public Guid Identifier { get; set; }
        public Boolean DisposeAgentsOnDispose { get; set; }
        private List<IAgent> Agents { get; set; }
        public event Action<Locator, IAgent> OnAdd;
        public event Action<Locator, IAgent> OnRemove;
        public event Action<Locator, IAgent> OnDisposeAgent;
        public event Action<Locator> OnDispose;
        
        public Locator()
        {
            this.Agents = new List<IAgent>();
            this.Identifier = Guid.NewGuid();
            this.DisposeAgentsOnDispose = true;
        }
        
        public Locator(String name, Boolean disposeAgentsOnDispose) : this()
        {
            this.Name = name;
            this.DisposeAgentsOnDispose = disposeAgentsOnDispose;
        }
        
        public Locator(String name) : this(name, true)
        {
            
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
        
        public void Add(IAgent agent)
        {
            var success = this.TryAdd(agent);
            if (!success)
            {
                throw new ElementExistsException();
            }
        }
        
        public Boolean TryAdd(IAgent agent)
        {
            if (this.Find(agent.Name).HasValue)
            {
                return false;
            }
            this.Agents.Add(agent);
            this.OnAdd.Invoke(this, agent);
            return true;
        }
        
        public Boolean Remove(Predicate<IAgent> predicate)
        {
            var agent = this.Find(predicate);
            if (agent.HasValue)
            {
                var success = this.Agents.Remove(agent.Value);
                this.OnRemove.Invoke(this, agent.Value);
                return success;
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
        
        public void Dispose()
        {
            this.OnDispose.Invoke(this);
            if (this.DisposeAgentsOnDispose)
            {
                foreach (var agent in this.Agents)
                {
                    this.OnDisposeAgent(this, agent);
                    agent.Dispose();
                }
            }
        }
    }
}
