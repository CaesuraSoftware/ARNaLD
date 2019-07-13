
using System;

namespace Caesura.Arnald.Core.Agents
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    
    public class AgentConfiguration : IAgentConfiguration
    {
        public static AgentConfiguration Default => CreateDefaults();
        public IAgent Owner { get; set; }
        public ILocator Location { get; set; }
        public String Name { get; set; }
        public Guid Identifier { get; set; }
        public IPersonality Personality { get; set; }
        public IMessageHandler Resolver { get; set; }
        public IMailbox Messages { get; set; }
        public IState AgentState { get; set; }
        public AgentAutonomy Autonomy { get; set; }
        public CancellationTokenSource CancelToken { get; set; }
        
        public AgentConfiguration()
        {
            
        }
        
        public AgentConfiguration(IAgent owner) : this()
        {
            this.Owner = owner;
        }
        
        public AgentConfiguration(IAgentConfiguration config) : this()
        {
            this.Copy(config);
        }
        
        public void Copy(IAgentConfiguration config)
        {
            this.Owner          = config.Owner;
            this.Location       = config.Location;
            this.Name           = config.Name;
            this.Identifier     = config.Identifier;
            this.Personality    = config.Personality;
            this.Resolver       = config.Resolver;
            this.AgentState     = config.AgentState;
            this.Messages       = config.Messages;
            this.Autonomy       = config.Autonomy;
            this.CancelToken    = config.CancelToken;
        }
        
        public static AgentConfiguration CreateDefaults(String name, Guid guid)
        {
            var aconf = new AgentConfiguration()
            {
                Name            = name,
                Identifier      = guid,
                Personality     = new Personality(),
                Resolver        = new MessageHandler(),
                AgentState      = new State(),
                Messages        = new Mailbox(),
                Autonomy        = AgentAutonomy.IndependentThread,
                CancelToken     = new CancellationTokenSource(),
            };
            return aconf;
        }
        
        public static AgentConfiguration CreateDefaults(String name)
        {
            return CreateDefaults(name, Guid.NewGuid());
        }
        
        public static AgentConfiguration CreateDefaults()
        {
            var guid = Guid.NewGuid();
            var name = guid.ToString("N").ToUpper();
            return CreateDefaults(name, guid);
        }
    }
}
