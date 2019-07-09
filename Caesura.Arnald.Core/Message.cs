
using System;

namespace Caesura.Arnald.Core
{
    using System.Collections.Generic;
    using System.Linq;
    
    public class PluginMessage : IMessage
    {
        public String Sender { get; set; }
        public String Recipient { get; set; }
        public String Message { get; set; }
        public Dictionary<String, Object> Items { get; set; }
        
        public PluginMessage()
        {
            this.Items = new Dictionary<String, Object>();
        }
        
        public PluginMessage(IMessage msg)
        {
            this.Copy(msg);
        }
        
        public virtual void Copy(IMessage msg)
        {
            this.Sender     = msg.Sender;
            this.Recipient  = msg.Recipient;
            this.Message    = msg.Message;
            this.Items      = new Dictionary<String, Object>(msg.Items);
        }
        
        public T Get<T>(String name)
        {
            if (!this.Items.ContainsKey(name))
            {
                throw new KeyNotFoundException();
            }
            var obj = this.Items[name];
            if (obj is T item)
            {
                return item;
            }
            else
            {
                throw new InvalidCastException();
            }
        }
        
        public Boolean TryGet<T>(String name, out T item)
        {
            if (this.Items.ContainsKey(name))
            {
                var obj = this.Items[name];
                if (obj is T nitem)
                {
                    item = nitem;
                    return true;
                }
            }
            item = default;
            return false;
        }
        
        public Boolean Set<T>(String name, T item)
        {
            return this.Set<T>(name, item, false);
        }
        
        public Boolean Set<T>(String name, T item, Boolean force)
        {
            if (this.Items.ContainsKey(name))
            {
                if (force)
                {
                    this.Items[name] = item;
                    return true;
                }
                return false;
            }
            else
            {
                this.Items.Add(name, item);
                return true;
            }
        }
        
        public override String ToString()
        {
            // TODO: turn into JSON
            return base.ToString();
        }
    }
}
