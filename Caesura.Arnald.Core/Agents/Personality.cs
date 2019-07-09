
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    public class Personality : IPersonality
    {
        private List<IBehavior> Behaviors { get; set; }
        
        public Personality()
        {
            this.Behaviors = new List<IBehavior>();
        }
        
        public IEnumerable<Task<IMessage>> Execute(IEnumerable<IMessage> messages)
        {
            var tasks = new List<Task<IMessage>>(this.Behaviors.Count);
            foreach (var behavior in this.Behaviors)
            {
                var task = behavior.Execute(messages);
                tasks.Add(task);
            }
            return tasks;
        }
        
        public void Learn(IBehavior behavior)
        {
            var b1 = this.GetBehavior(x => x.Name == behavior.Name);
            if (b1 is IBehavior)
            {
                this.Behaviors.Remove(b1);
            }
            this.Behaviors.Add(b1);
            this.Behaviors.Sort();
        }
        
        public void Unlearn(IBehavior behavior)
        {
            this.Unlearn(x => x.Name == behavior.Name);
        }
        
        public void Unlearn(Predicate<IBehavior> predicate)
        {
            var b1 = this.GetBehavior(predicate);
            if (b1 is IBehavior)
            {
                b1.Dispose();
                this.Behaviors.Remove(b1);
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
        
        public IBehavior GetBehavior(Predicate<IBehavior> predicate)
        {
            return this.Behaviors.Find(predicate);
        }
        
        public void Dispose()
        {
            this.UnlearnAll();
        }
    }
}
