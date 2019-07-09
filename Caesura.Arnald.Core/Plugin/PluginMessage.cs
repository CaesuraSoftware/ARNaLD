
using System;

namespace Caesura.Arnald.Core.Plugin
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Caesura.Standard;
    
    public class PluginMessage : IPluginMessage
    {
        public PluginKind Kind { get; set; }
        public String Name { get; set; }
        public String Message { get; set; }
        public Dictionary<String, Object> Items { get; set; }
        
        public PluginMessage()
        {
            this.Items = new Dictionary<String, Object>();
        }
        
        public PluginMessage(IPluginMessage msg)
        {
            this.Copy(msg);
        }
        
        public void Copy(IPluginMessage msg)
        {
            this.Kind       = msg.Kind;
            this.Name       = msg.Name;
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
            return this.ToString(4);
        }
        
        public String ToString(Int32 spaceWidth)
        {
            return this.ToString(spaceWidth, true);
        }
        
        public String ToString(Int32 spaceWidth, Boolean format)
        {
            var space = "";
            if (format)
            {
                for (var i = 0; i < spaceWidth; i++)
                {
                    space += " ";
                }
            }
            var sb = new StringBuilder();
            
            sb.Append("{");
            formati();
            
            sb.Append(nameof(this.Kind).Quote());
            sb.Append(":");
            formats();
            sb.Append(((Int32)this.Kind).ToString().Quote());
            sb.Append(", ");
            formati();
            
            sb.Append(nameof(this.Name).Quote());
            sb.Append(":");
            formats();
            sb.Append(this.Name.Quote());
            sb.Append(", ");
            formati();
            
            sb.Append(nameof(this.Message).Quote());
            sb.Append(":");
            formats();
            sb.Append(this.Message.Quote());
            sb.Append(", ");
            formati();
            
            sb.Append(nameof(this.Items).Quote());
            sb.Append(":");
            formats();
            sb.Append("{");
            formati();
            
            var index = 0;
            foreach (var item in this.Items)
            {
                var key     = item.Key;
                var value   = item.Value;
                
                if (format)
                {
                    sb.Append(space);
                }
                sb.Append(key.Quote());
                sb.Append(":");
                formats();
                sb.Append(value.ToString().Quote()); //naive but whatever, this isn't supposed to be real json anyway
                if (index != this.Items.Count - 1)
                {
                    sb.Append(",");
                    formati();
                    if (format)
                    {
                        sb.Append(space); // double indent
                    }
                }
                index++;
            }
            sb.Append("}");
            return sb.ToString();
            
            // format indent
            void formati()
            {
                if (format) 
                { 
                    sb.AppendLine(); 
                    sb.Append(space);
                }
            }
            
            //format space
            void formats()
            {
                if (format)
                {
                    sb.Append(" ");
                }
            }
        }
    }
}
