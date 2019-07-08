
using System;

namespace Caesura.Standard.Logging
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    
    public class LoggingHandler : ILoggingHandler
    {
        private readonly Object messageLock = new Object();
        private Queue<LogInformation> Messages { get; set; }
        private List<BaseLogEventHandler> Handlers { get; set; }
        private Thread HandlerThread { get; set; }
        private Int32 b_HandlerThreadSleepInterval;
        public Int32 HandlerThreadSleepInterval 
        { 
            get => this.b_HandlerThreadSleepInterval;
            set => this.b_HandlerThreadSleepInterval = value < 0 ? 0 : value;
        }
        public Boolean IsBackground { get; set; }
        public Boolean HandlerRunning { get; private set; }
        
        public LoggingHandler()
        {
            this.Messages = new Queue<LogInformation>();
            this.Handlers = new List<BaseLogEventHandler>();
            this.HandlerRunning = false;
            this.HandlerThreadSleepInterval = 1;
            this.IsBackground = true;
        }
        
        public void Start()
        {
            if (this.HandlerRunning)
            {
                return;
            }
            if (this.HandlerThread is null)
            {
                this.HandlerThread = new Thread(this.HandlerThreadCallback);
                this.HandlerThread.IsBackground = this.IsBackground;
            }
            
            try
            {
                this.HandlerRunning = true;
                this.HandlerThread.Start();
            }
            catch (ThreadStateException)
            {
                
            }
            catch (ThreadStartException)
            {
                
            }
        }
        
        public void Stop()
        {
            this.HandlerRunning = false;
        }
        
        public void Enqueue(LogInformation info)
        {
            lock (this.messageLock)
            {
                this.Messages.Enqueue(info);
            }
        }
        
        public void AddHandler(BaseLogEventHandler handler)
        {
            this.Handlers.Add(handler);
        }
        
        public Boolean RemoveHandler(BaseLogEventHandler handler)
        {
            return this.Handlers.Remove(handler);
        }
        
        protected virtual void HandlerThreadCallback()
        {
            while (this.HandlerRunning)
            {
                Queue<LogInformation> messages;
                lock (this.messageLock)
                {
                    messages = new Queue<LogInformation>(this.Messages);
                    this.Messages.Clear();
                }
                foreach (var message in messages)
                {
                    foreach (var handler in this.Handlers)
                    {
                        if (handler.EventKind.HasFlag(message.Kind))
                        {
                            handler.Log(message);
                        }
                    }
                }
                
                Thread.Sleep(this.HandlerThreadSleepInterval);
            }
        }
    }
}
