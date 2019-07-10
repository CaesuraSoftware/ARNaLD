
using System;

namespace Caesura.Arnald.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    public class Message : Message<String>
    {
        
    }
    
    public class Message<T> : IMessage<T>
    {
        public String Sender { get; set; }
        public String Recipient { get; set; }
        public String Information { get; set; }
        public T Data { get; set; }
        private Dictionary<String, Object> _items;
        public Dictionary<String, Object> Items { get => this.GetItems(); set => this._items = value; }
        
        public Message()
        {
            
        }
        
        public Message(IMessage msg)
        {
            this.Copy(msg);
        }
        
        public virtual void Copy(IMessage msg)
        {
            this.Sender         = msg.Sender;
            this.Recipient      = msg.Recipient;
            this.Information    = msg.Information;
            this.Items          = new Dictionary<String, Object>(msg.Items);
            if (msg is IMessage<T> tmsg)
            {
                this.Data = tmsg.Data;
            }
        }
        
        public R Get<R>(String name)
        {
            if (!this.Items.ContainsKey(name))
            {
                throw new KeyNotFoundException();
            }
            var obj = this.Items[name];
            if (obj is R item)
            {
                return item;
            }
            else
            {
                throw new InvalidCastException();
            }
        }
        
        public Boolean TryGet<R>(String name, out R item)
        {
            if (this.Items.ContainsKey(name))
            {
                var obj = this.Items[name];
                if (obj is R nitem)
                {
                    item = nitem;
                    return true;
                }
            }
            item = default;
            return false;
        }
        
        public Boolean Set<R>(String name, R item)
        {
            return this.Set<R>(name, item, false);
        }
        
        public Boolean Set<R>(String name, R item, Boolean force)
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
        
        private Dictionary<String, Object> GetItems()
        {
            if (this._items is null)
            {
                this._items = new Dictionary<String, Object>();
            }
            return this._items;
        }
        
        public override String ToString()
        {
            // TODO: turn into JSON
            return base.ToString();
        }
    }
}
