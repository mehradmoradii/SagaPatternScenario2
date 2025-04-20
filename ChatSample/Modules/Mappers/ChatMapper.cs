using ChatSample.Modules.Domains._0_Chats.Aggregate;
using ChatSample.Modules.Domains._0_Chats.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatSample.Modules.Mappers
{
    public class ChatMapper : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasKey(i => i.userId);
            builder.HasMany(m => m.Messages).WithOne(c=>c.Belongto).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
