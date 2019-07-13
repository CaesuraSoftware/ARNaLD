
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Caesura.Standard;
    
    public class Locator : ILocator
    {
        public String Name { get; set; }
        public Guid Identifier { get; set; }
        public Boolean DisposeAgentsOnDispose { get; set; }
        public event Action<ILocator, IAgent> OnAdd;
        public event Action<ILocator, IAgent> OnRemove;
        public event Action<ILocator, IAgent> OnDisposeAgent;
        public event Action<ILocator> OnDispose;
        
        private List<IAgent> Agents { get; set; }
        private Thread ManualAgentCycler { get; set; }
        private Boolean ManualAgentCyclerRunning { get; set; }
        private CancellationTokenSource CancelToken { get; set; }
        
        public Locator()
        {
            this.Agents                             = new List<IAgent>();
            this.Identifier                         = Guid.NewGuid();
            this.DisposeAgentsOnDispose             = true;
            
            this.ManualAgentCycler                  = new Thread(this.Run);
            this.ManualAgentCycler.IsBackground     = true;
            this.CancelToken                        = new CancellationTokenSource();
        }
        
        public Locator(String name, Boolean disposeAgentsOnDispose) : this()
        {
            this.Name = name;
            this.DisposeAgentsOnDispose = disposeAgentsOnDispose;
        }
        
        public Locator(String name) : this(name, true)
        {
            
        }
        
        public Boolean Send(IMessage message)
        {
            var agent = this.Find(x => x.Name == message.Recipient);
            if (agent)
            {
                agent.Value.Send(message);
                return true;
            }
            return false;
        }
        
        public void SendToAll(IMessage message)
        {
            var agents = this.FindAll(x => true);
            foreach (var agent in agents)
            {
                agent.Send(message);
            }
        }
        
        public void Start()
        {
            if (this.ManualAgentCyclerRunning)
            {
                return;
            }
            
            try
            {
                if (this.CancelToken.IsCancellationRequested)
                {
                    this.CancelToken = new CancellationTokenSource();
                }
                this.ManualAgentCycler.Start();
            }
            catch (ThreadStateException)
            {
                // no-op
            }
        }
        
        public void Stop()
        {
            var autonomous = this.FindAll(x => x.Autonomy.HasFlag(AgentAutonomy.IndependentThread));
            foreach (var agent in autonomous)
            {
                agent.Stop();
            }
            
            this.CancelToken.Cancel(false);
        }
        
        public void Wait()
        {
            while (this.ManualAgentCyclerRunning)
            {
                Thread.Sleep(50);
            }
        }
        
        public void Run()
        {
            this.Run(this.CancelToken);
        }
        
        public void Run(CancellationTokenSource token)
        {
            this.ManualAgentCyclerRunning = true;
            
            var autonomous = this.FindAll(x => x.Autonomy.HasFlag(AgentAutonomy.IndependentThread));
            
            foreach (var agent in autonomous)
            {
                if (agent.AgentThreadState.HasFlag(ThreadState.Unstarted | ThreadState.Stopped))
                {
                    agent.Start();
                }
            }
            
            while (!token.IsCancellationRequested)
            {
                var agents = this.FindAll(x => x.Autonomy.HasFlag(AgentAutonomy.SimulateCycle));
                
                foreach (var agent in agents)
                {
                    agent.CycleOnceNoBlock();
                    
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }
            
            this.ManualAgentCyclerRunning = false;
        }
        
        public Maybe<IAgent> Find(Predicate<IAgent> predicate)
        {
            var agent = this.Agents.Find(predicate);
            if (agent is null)
            {
                return Maybe.None;
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
            if (this.Find(agent.Name))
            {
                return false;
            }
            
            this.Agents.Add(agent);
            
            if ((this.ManualAgentCyclerRunning) 
            && (!this.CancelToken.IsCancellationRequested)
            && (agent.Autonomy.HasFlag(AgentAutonomy.IndependentThread))
            && (agent.AgentThreadState.HasFlag(ThreadState.Unstarted | ThreadState.Stopped)))
            {
                agent.Start();
            }
            
            this.OnAdd?.Invoke(this, agent);
            
            return true;
        }
        
        public Boolean Remove(Predicate<IAgent> predicate)
        {
            var agent = this.Find(predicate);
            if (agent)
            {
                var success = this.Agents.Remove(agent.Value);
                this.OnRemove?.Invoke(this, agent.Value);
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
        
        public void Clear()
        {
            this.Agents.Clear();
        }
        
        public void Clear(Boolean disposeAgents)
        {
            if (disposeAgents)
            {
                foreach (var agent in this.Agents)
                {
                    this.OnDisposeAgent?.Invoke(this, agent);
                    agent.Dispose();
                }
            }
            this.Clear();
        }
        
        public void Dispose()
        {
            this.Stop();
            this.Wait();
            this.OnDispose?.Invoke(this);
            this.Clear(this.DisposeAgentsOnDispose);
        }
    }
}
