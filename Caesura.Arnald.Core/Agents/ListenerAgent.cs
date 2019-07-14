
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    
    public abstract class ListenerAgent : BaseAgent
    {
        protected Thread ListenerThread { get; set; }
        protected Boolean ListenerRunning { get; set; }
        
        public ListenerAgent() : base()
        {
            
        }
        
        public ListenerAgent(IAgentConfiguration config) : base(config)
        {
            
        }
        
        public override void Setup(IAgentConfiguration config)
        {
            base.Setup(config);
            
            this.ListenerThread                 = new Thread(this.UpdateListen);
            this.ListenerThread.IsBackground    = true;
            this.ListenerRunning                = false;
        }
        
        public override void Start()
        {
            if ((!this.ListenerRunning) || (!this.CancelToken.IsCancellationRequested))
            {
                try
                {
                    this.ListenerThread.Start();
                }
                catch (ThreadStartException)
                {
                    // no-op
                }
            }
            base.Start();
        }
        
        protected virtual void UpdateListen()
        {
            this.ListenerRunning = true;
            this.BeginListen();
            while (!this.CancelToken.IsCancellationRequested)
            {
                this.Listen();
            }
            this.ListenerRunning = false;
            this.EndListen();
        }
        
        protected virtual void BeginListen()
        {
            
        }
        
        protected virtual void EndListen()
        {
            
        }
        
        protected abstract void Listen();
    }
}
