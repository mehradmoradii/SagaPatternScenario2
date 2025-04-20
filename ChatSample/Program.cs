using ChatSample.Implements.MinimalApiConf;
using ChatSample.Implements.Services;
using ChatSample.Infrastructures.Interfaces.RabbitMQ;
using ChatSample.RabbitMQ.RpcClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCustomConfigurationService(builder.Configuration);
builder.Services.AddAllAuthMinimalApis();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.RegisterAuthMinimalApis();

app.UseHttpsRedirection();


app.Lifetime.ApplicationStarted.Register(async () =>
{
    using var scope = app.Services.CreateScope(); // ? Create a scope
    var scopedProvider = scope.ServiceProvider;

    var rpcClient = scopedProvider.GetRequiredService<IRpcClient>() as ChatRpcClient;

    if (rpcClient != null)
    {
        try
        {
            await rpcClient.InitAsync("Result-Check-Authentication");
            Console.WriteLine("? ChatRpcClient initialized successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"? RPC Init failed: {ex.Message}");
        }
    }
});


app.Run();
