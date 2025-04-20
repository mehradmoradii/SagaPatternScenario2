using ChatSample.Modules.Domains._0_Chats.Aggregate;
using ChatSample.Modules.Domains._0_Chats.Entity;
using ChatSample.Modules.Mappers;
using Microsoft.EntityFrameworkCore;

namespace ChatSample.Modules.Context
{
    public class ChatDbContext : DbContext
    {


        public ChatDbContext(DbContextOptions<ChatDbContext> opt) : base(opt) 
        {
            
        }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ChatMapper());
            modelBuilder.ApplyConfiguration(new MessageMapper());


            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

    }
}
