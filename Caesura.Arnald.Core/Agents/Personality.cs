
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Caesura.Standard;
    
    public class Personality : IPersonality
    {
        private List<IBehavior> Behaviors { get; set; }
        
        public Personality()
        {
            this.Behaviors = new List<IBehavior>();
        }
        
        public void Run(String name, IMessage message)
        {
            var be = this.GetBehavior(x => x.Name == name);
            if (be)
            {
                Task.Run(() =>
                {
                    be.Value.Execute(message);
                });
            }
        }
        
        public void Learn(IBehavior behavior)
        {
            var be = this.GetBehavior(x => x.Name == behavior.Name);
            if (be)
            {
                this.Behaviors.Remove(be.Value);
            }
            this.Behaviors.Add(behavior);
            this.Behaviors.Sort();
        }
        
        public void Unlearn(IBehavior behavior)
        {
            this.Unlearn(x => x.Name == behavior.Name);
        }
        
        public void Unlearn(Predicate<IBehavior> predicate)
        {
            var be = this.GetBehavior(predicate);
            if (be)
            {
                be.Value.Dispose();
                this.Behaviors.Remove(be.Value);
            }
        }
        
        public void UnlearnAll()
        {
            foreach (var behavior in this.Behaviors)
            {
                behavior.Dispose();
            }
            this.Behaviors.Clear();
        }
        
        public Boolean HasBehavior(IBehavior behavior)
        {
            return this.Behaviors.Exists(x => x.Name == behavior.Name);
        }
        
        public Boolean HasBehavior(String name)
        {
            return this.Behaviors.Exists(x => x.Name == name);
        }
        
        public Maybe<IBehavior> GetBehavior(Predicate<IBehavior> predicate)
        {
            var behavior = this.Behaviors.Find(predicate);
            if (behavior is null)
            {
                return Maybe.None;
            }
            return Maybe<IBehavior>.Some(behavior);
        }
        
        public void Dispose()
        {
            this.UnlearnAll();
        }
    }
}
