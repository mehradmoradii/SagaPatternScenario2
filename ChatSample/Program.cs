
using ChatSample.CRUD.Saga.PremiumChat.Commands;
using ChatSample.Implements.MinimalApiConf;
using ChatSample.Implements.Services;
using ChatSample.Infrastructures.Interfaces.RabbitMQ;
using ChatSample.RabbitMQ.RpcClient;
using Microsoft.Data.SqlClient;
using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Text.Json.Serialization;
using System.Text.Json;

Console.Title = "ChatSample";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCustomConfigurationService(builder.Configuration);
builder.Services.AddAllAuthMinimalApis();


// NServiceBus Configuration

builder.Host.UseNServiceBus(Cf =>
{
    var endPointConf = new EndpointConfiguration("PremiumChat");

    var serialization = endPointConf.UseSerialization<SystemJsonSerializer>();
    serialization.Options(
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        });

    endPointConf.AssemblyScanner()
    .ScanAssembliesInNestedDirectories = true;
    endPointConf.AssemblyScanner()
        .ScanAppDomainAssemblies = true;


    var transport = endPointConf.UseTransport<RabbitMQTransport>();
    transport.ConnectionString("host=localhost;username=guest;password=guest"); // default value
    transport.UseConventionalRoutingTopology(QueueType.Quorum);

    var conventions = endPointConf.Conventions();
    conventions.DefiningCommandsAs(type => type.Namespace == "ChatSample.CRUD.Saga.PremiumChat.Commands");
    conventions.DefiningEventsAs(type => type.Namespace == "ChatSample.CRUD.Saga.PremiumChat.Events");

  



    var routing = transport.Routing();
    routing.RouteToEndpoint(typeof(CheckAuthPremiumChatSagaCommand), "AuthenticationPremiumChat"); // just for commands
    routing.RouteToEndpoint(typeof(PaymentPremiumChatSagaCommand), "PaymentPremiumChat");
    
    routing.RouteToEndpoint(typeof(StartPremiumChatProccessCommand), "PremiumChat");
    routing.RouteToEndpoint(typeof(CreatePremiumChatSagaCommand), "PremiumChat");

    endPointConf.EnableInstallers();
    endPointConf.UsePersistence<LearningPersistence>(); // Or SQL persistence in production
    endPointConf.SendFailedMessagesTo("error");
    endPointConf.AuditProcessedMessagesTo("audit");

    

    return endPointConf;
    

});




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
