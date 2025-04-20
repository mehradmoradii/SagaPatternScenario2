using ChatSample.Infrastructures.BaseModules;
using ChatSample.Modules.Domains._0_Chats.Entity;

namespace ChatSample.Modules.Domains._0_Chats.Aggregate
{
    public class Chat : EntityBase
    {
        public Guid Owner1 { get; set; }
        public Guid Owner2 { get; set; }

        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
