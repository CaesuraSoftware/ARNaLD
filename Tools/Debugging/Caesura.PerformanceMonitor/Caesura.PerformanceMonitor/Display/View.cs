
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
        public String Prompt { get; set; }
        public String TextArea { get; private set; }
        public ConsoleColor TextAreaColor { get; set; }
        private Thread RenderThread { get; set; }
        private IMonitor Monitor { get; set; }
        private MonitorResult CurrentStatus { get; set; }
        private DateTime RefreshStartTime { get; set; }
        private List<ViewField> Views { get; set; }
        private String CurrentView { get; set; }
        
        public View()
        {
            this.TextAreaColor              = ConsoleColor.Gray;
            this.Prompt                     = "> ";
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
        
        public void Wait()
        {
            while (this.Running)
            {
                Thread.Sleep(50);
            }
        }
        
        public void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            this.ClearScreen();
            while (this.Running)
            {
                Thread.Sleep(100);
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
        
        public void SetInput(String input, ConsoleColor color)
        {
            this.TextArea = input;
            this.TextAreaColor = color;
        }
        
        public void ClearInputBuffer()
        {
            this.TextArea = String.Empty;
        }
        
        public void ClearScreen()
        {
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
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
        
        public ViewField GetView(String name)
        {
            return this.Views.Find(x => String.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
        }
        
        public void SetView(String name)
        {
            this.ClearScreen();
            this.CurrentView = name;
            foreach (var view in this.Views)
            {
                view.OnChange?.Invoke(this);
            }
        }
        
        public void Render(MonitorResult result)
        {
            var view = this.Views.Find(x => x.Name == this.CurrentView);
            if (view != null && result != null)
            {
                view.Run(this, result);
            }
        }
    }
}
