
using System;

namespace Caesura.PerformanceMonitor.Display
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Monitor;
    
    public class View
    {
        public Int32 RefreshRate { get; set; }
        public Boolean Running { get; private set; }
        public String TextArea { get; private set; }
        private Thread RenderThread { get; set; }
        private IMonitor Monitor { get; set; }
        private MonitorResult CurrentStatus { get; set; }
        private DateTime RefreshStartTime { get; set; }
        private List<ViewField> Views { get; set; }
        private String CurrentView { get; set; }
        
        public View()
        {
            this.RenderThread               = new Thread(this.Run);
            this.RenderThread.IsBackground  = true;
            this.RefreshStartTime           = DateTime.UtcNow;
            this.Views                      = new List<ViewField>(); // I'd do anything for views
        }
        
        public View(Int32 refreshRate, IMonitor monitorHandle) : this()
        {
            this.RefreshRate                = refreshRate;
            this.Monitor                    = monitorHandle;
        }
        
        public void Start()
        {
            if (this.Running)
            {
                return;
            }
            this.Running = true;
            this.RenderThread.Start();
        }
        
        public void Stop()
        {
            this.Running = false;
        }
        
        public void Run()
        {
            Console.CursorVisible  = false;
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            while (this.Running)
            {
                Thread.Sleep(50);
                var elapsed = (DateTime.UtcNow - this.RefreshStartTime).TotalMilliseconds;
                if (this.CurrentStatus is null || elapsed > this.RefreshRate)
                {
                    this.CurrentStatus = this.Monitor.GetStatus();
                    this.RefreshStartTime = DateTime.UtcNow;
                }
                this.Render(this.CurrentStatus);
            }
        }
        
        public void SetInput(String input)
        {
            this.TextArea = input;
        }
        
        public void ClearInputBuffer()
        {
            this.TextArea = String.Empty;
        }
        
        public void ClearScreen()
        {
            Console.Clear();
        }
        
        public void AddView(ViewField view)
        {
            this.Views.Add(view);
            if (this.CurrentView is null)
            {
                this.CurrentView = view.Name;
            }
        }
        
        public void SetView(String name)
        {
            this.CurrentView = name;
        }
        
        public void Render(MonitorResult result)
        {
            var view = this.Views.Find(x => x.Name == this.CurrentView);
            if (view != null)
            {
                view.Run(this, result);
            }
        }
    }
}
