using ChatSample.CRUD.Command.Handlers;

using ChatSample.Infrastructures.Interfaces.MinimalApi;
using ChatSample.Infrastructures.Interfaces.RabbitMQ;
using ChatSample.Infrastructures.Interfaces.Repository;
using ChatSample.MinimalApi;
using ChatSample.Modules.Context;
using ChatSample.Modules.Domains._0_Chats.Aggregate;
using ChatSample.RabbitMQ.RpcClient;
using ChatSample.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ChatSample.Implements.Services
{
    public static class CustomConfigurationService
    {
        public static void AddCustomConfigurationService(this IServiceCollection services, IConfiguration con)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod().AllowAnyHeader();
                });
            });
            services.AddDbContext<ChatDbContext>(options =>
                options.UseSqlServer(con.GetConnectionString("DefaultConnectionString"),
                                     b => b.MigrationsAssembly("ChatSample")));

            services.AddMediatR(conf =>
            {
                conf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddMediatR(hd => hd.RegisterServicesFromAssembly(typeof(SendMessageCommandHandler).Assembly));
            services.AddMediatR(hd => hd.RegisterServicesFromAssembly(typeof(CreatePremiumChatCommandHandler).Assembly));   




            // lifetime

            services.AddScoped<IRpcClient, ChatRpcClient>();
            services.AddScoped<IMinimalApi, MessageMinimalApi>();
            services.AddScoped<BaseRepostory<Chat, Guid>>();
            services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepostory<,>));







        }
    }
}
