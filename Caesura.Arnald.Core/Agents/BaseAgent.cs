
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Threading;
    
    public abstract class BaseAgent : IAgent
    {
        public virtual ILocator HostLocator { get; set; }
        public virtual String Name { get; protected set; }
        public virtual Guid Identifier { get; protected set; }
        public virtual IPersonality Personality { get; protected set; }
        public virtual IMessageHandler Resolver { get; set; }
        public virtual IState AgentState { get; set; }
        public virtual ThreadState AgentThreadState { get; protected set; }
        public virtual AgentAutonomy Autonomy { get; set; }
        public virtual Boolean Running => this.AgentThreadRunning;
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
        
        public BaseAgent(IAgentConfiguration config) : this()
        {
            this.Setup(config);
        }
        
        public virtual void Setup(IAgentConfiguration config)
        {
            // FIXME: this isn't making new instances/copies of
            // the subclasses like the Mailbox or State. change it
            // to create new instances so the config passed to this
            // can be re-used for other agents.
            var newconfig               = new AgentConfiguration(config);
            
            newconfig.Owner             = this;
            
            this.HostLocator            = newconfig.Location;
            this.Name                   = newconfig.Name;
            this.Identifier             = newconfig.Identifier;
            this.Personality            = newconfig.Personality;
            this.Resolver               = newconfig.Resolver;
            this.Messages               = newconfig.Messages;
            this.AgentState             = newconfig.AgentState;
            this.Autonomy               = newconfig.Autonomy;
            this.CancelToken            = newconfig.CancelToken;
            
            this.Resolver.HostAgent     = this;
            this.AgentState.HostAgent   = this;
        }
        
        /// <summary>
        /// Start an agent in it's own thread. If this is not desired, use an Agent
        /// collection and manually call their CycleOnce methods to Tasks.
        /// </summary>
        public virtual void Start()
        {
            if (this.AgentThreadRunning || (this.AgentThreadState == ThreadState.Running))
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
            if (!this.AgentThreadRunning)
            {
                return;
            }
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
                this.Update();
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
        public virtual void Update()
        {
            this.Update(this.CancelToken.Token);
        }
        
        public virtual void Update(CancellationToken token)
        {
            try
            {
                var msg = this.Messages.Receive(token);
                this.HandleMessage(msg);
            }
            catch (OperationCanceledException)
            {
                // no-op
            }
        }
        
        /// <summary>
        /// Cycle the agent. If it has no messages (nothing to do), immediately return.
        /// </summary>
        public virtual void UpdateAndContinue()
        {
            var msg = this.Messages.TryReceive();
            if (msg)
            {
                this.HandleMessage(msg.Value);
            }
        }
        
        /// <summary>
        /// Called by all update methods. This should not be called directly.
        /// </summary>
        /// <param name="message"></param>
        public virtual void HandleMessage(IMessage message)
        {
            this.Resolver.Process(message);
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
            this.HostLocator?.Remove(this);
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
