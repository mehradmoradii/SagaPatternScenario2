using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace ChatSample.Modules.Context
{
    public class DbContextFactory
    {
        public class MainAuthDbContextFactory : IDesignTimeDbContextFactory<ChatDbContext>
        {
            public ChatDbContext CreateDbContext(string[] args)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory()) // Ensure this is correct for EF CLI
                    .AddJsonFile("appsettings.json")
                    .Build();

                var optionsBuilder = new DbContextOptionsBuilder<ChatDbContext>();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"));

                return new ChatDbContext(optionsBuilder.Options);
            }
        }
    }
}
