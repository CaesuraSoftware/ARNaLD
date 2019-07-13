
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
        public String Name { get; protected set; }
        public Guid Identifier { get; protected set; }
        public IPersonality Personality { get; protected set; }
        public ThreadState AgentThreadState { get; protected set; }
        protected Object _threadStateLock { get; set; } = new Object();
        protected Boolean AgentRunning { get; set; }
        protected CancellationTokenSource CancelToken { get; set; }
        protected IMailbox Messages { get; set; }
        protected IState AgentState { get; set; }
        protected Thread AgentThread { get; set; }
        
        public BaseAgent()
        {
            this.AgentThreadState           = ThreadState.Unstarted;
            this.AgentRunning               = false;
            this.AgentThread                = new Thread(this.Run);
            this.AgentThread.IsBackground   = true;
        }
        
        public BaseAgent(AgentConfiguration config) : this()
        {
            config.Owner = this;
            this.Setup(config);
        }
        
        public void Setup(IAgentConfiguration config)
        {
            this.Name               = config.Name;
            this.Identifier         = config.Identifier;
            this.Personality        = config.Personality;
            this.Messages           = config.Messages;
            this.AgentState         = config.AgentState;
            this.AgentState.Owner   = this;
        }
        
        /// <summary>
        /// Start an agent in it's own thread. If this is not desired, use an Agent
        /// collection and manually call their CycleOnce methods to Tasks.
        /// </summary>
        public virtual void Start()
        {
            if (this.AgentRunning)
            {
                throw new InvalidOperationException("Agent is already running.");
            }
            this.AgentThreadState = ThreadState.Running;
            this.CancelToken = new CancellationTokenSource();
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
            while (this.AgentRunning)
            {
                Thread.Sleep(50); // 50 based on experiences from other developers
            }
        }
        
        /// <summary>
        /// Run the Agent's main loop. This is a blocking call.
        /// </summary>
        public virtual void Run()
        {
            this.AgentRunning = true;
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
            this.AgentRunning = false;
        }
        
        /// <summary>
        /// Cycle the Agent. If the agent does not have any messages, this will block.
        /// </summary>
        public virtual void CycleOnce()
        {
            var msg = this.Messages.Receive(this.CancelToken.Token);
            this.AgentState.Next(msg);
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
