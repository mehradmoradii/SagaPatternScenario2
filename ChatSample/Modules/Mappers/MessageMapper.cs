using ChatSample.Modules.Domains._0_Chats.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatSample.Modules.Mappers
{
    public class MessageMapper : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(i => i.userId);

            builder.HasOne(c => c.Belongto).WithMany(m => m.Messages).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
