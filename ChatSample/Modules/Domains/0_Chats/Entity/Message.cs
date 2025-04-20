using ChatSample.Infrastructures.BaseModules;
using ChatSample.Modules.Domains._0_Chats.Aggregate;

namespace ChatSample.Modules.Domains._0_Chats.Entity
{
    public class Message : EntityBase
    {
        public string Text { get; set; }

        public bool IsReply { get; set; } = false;

        public Guid? RepliedMessageId { get; set; }


        public virtual Chat Belongto { get; set; }
    }
}
