
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    
    /// <summary>
    /// Internals of an Agent: The agent's state machine is activated every
    /// time the agent gets a message, in which that message is passed to
    /// the current state. That state then should call a Behavior from the
    /// agent's Personality in order to act out tasks.
    /// </summary>
    public abstract class BaseAgent : IAgent
    {
        public virtual String Name { get; protected set; }
        public virtual Guid Identifier { get; protected set; }
        public virtual IPersonality Personality { get; protected set; }
        public virtual IMessageHandler Resolver { get; set; }
        public virtual IState AgentState { get; set; }
        public virtual ThreadState AgentThreadState { get; protected set; }
        public virtual AgentAutonomy Autonomy { get; set; }
        public virtual IMailbox Messages { get; set; }
        protected readonly Object _threadStateLock = new Object();
        protected virtual CancellationTokenSource CancelToken { get; set; }
        protected virtual Boolean AgentThreadRunning { get; set; }
        protected virtual Thread AgentThread { get; set; }
        
        public BaseAgent()
        {
            this.Autonomy                   = AgentAutonomy.None;
            this.AgentThreadState           = ThreadState.Unstarted;
            this.AgentThreadRunning         = false;
            this.AgentThread                = new Thread(this.Run);
            this.AgentThread.IsBackground   = true;
        }
        
        public BaseAgent(AgentConfiguration config) : this()
        {
            this.Setup(config);
        }
        
        public virtual void Setup(IAgentConfiguration config)
        {
            config.Owner            = this;
            
            this.Name               = config.Name;
            this.Identifier         = config.Identifier;
            this.Personality        = config.Personality;
            this.Resolver           = config.Resolver;
            this.Messages           = config.Messages;
            this.AgentState         = config.AgentState;
            this.Autonomy           = config.Autonomy;
            this.CancelToken        = config.CancelToken;
            
            this.Resolver.Owner     = this;
            this.AgentState.Owner   = this;
        }
        
        /// <summary>
        /// Start an agent in it's own thread. If this is not desired, use an Agent
        /// collection and manually call their CycleOnce methods to Tasks.
        /// </summary>
        public virtual void Start()
        {
            if (this.AgentThreadRunning)
            {
                throw new InvalidOperationException("Agent is already running.");
            }
            
            this.AgentThreadState = ThreadState.Running;
            try
            {
                this.AgentThread.Start();
            }
            catch (ThreadStateException)
            {
                // no-op
            }
        }
        
        public virtual void Stop()
        {
            lock (this._threadStateLock)
            {
                if (this.AgentThreadState == ThreadState.StopRequested)
                {
                    return;
                }
                this.AgentThreadState = ThreadState.StopRequested;
            }
            this.AgentState.TrySetState(StateAtomState.Cancelled);
            this.CancelToken.Cancel(false);
        }
        
        public virtual void Wait()
        {
            while (this.AgentThreadRunning)
            {
                Thread.Sleep(50); // 50 based on experiences from other developers
            }
        }
        
        /// <summary>
        /// Run the Agent's main loop. This is a blocking call.
        /// </summary>
        public virtual void Run()
        {
            this.AgentThreadRunning = true;
            while (!this.CancelToken.Token.IsCancellationRequested)
            {
                this.CycleOnce();
            }
            lock (this._threadStateLock)
            {
                if (this.AgentThreadState != ThreadState.Unstarted)
                {
                    this.AgentThreadState = ThreadState.Stopped;
                }
            }
            this.AgentThreadRunning = false;
        }
        
        /// <summary>
        /// Cycle the Agent. If the agent does not have any messages, this will block.
        /// </summary>
        public virtual void CycleOnce()
        {
            this.CycleOnce(this.CancelToken.Token);
        }
        
        public virtual void CycleOnce(CancellationToken token)
        {
            var msg = this.Messages.Receive(token);
            this.Resolver.Process(msg);
        }
        
        public virtual void Learn(IBehavior behavior)
        {
            this.Personality.Learn(behavior);
        }
        
        public virtual Boolean Send(IMessage message)
        {
            var success = this.Messages.TrySend(message);
            return success;
        }
        
        public virtual void Send(IMessage message, CancellationToken token)
        {
            this.Messages.WaitSend(message, token);
        }
        
        public virtual void Dispose()
        {
            this.Dispose(true);
        }
        
        public virtual void Dispose(Boolean wait)
        {
            this.AgentState?.Dispose(); // set State to Disposing to handle cleanup before disposing anything else
            this.Stop();
            if (wait)
            {
                lock (this._threadStateLock)
                {
                    this.AgentThreadState = ThreadState.WaitSleepJoin;
                }
                this.Wait();
            }
            this.Personality?.Dispose();
            this.Messages?.Dispose();
        }
    }
}
