using ChatSample.Infrastructures.Interfaces.MinimalApi;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChatSample.Implements.MinimalApiConf
{
    public static class MinimalApiConfiguration
    {
            public static IServiceCollection AddAllAuthMinimalApis(this IServiceCollection services)
            {
                var assembly = typeof(Program).Assembly;
                var serviceDescriptors = assembly
                    .DefinedTypes
                .Where(type => !type.IsAbstract &&
                                    !type.IsInterface &&
                                    type.IsAssignableTo(typeof(IMinimalApi)))
                    .Select(type => ServiceDescriptor.Transient(typeof(IMinimalApi), type));
                services.TryAddEnumerable(serviceDescriptors);
                return services;
            }
            public static IApplicationBuilder RegisterAuthMinimalApis(this WebApplication app)
            {
                using var scope = app.Services.CreateScope();
                var apis = scope.ServiceProvider.GetRequiredService<IEnumerable<IMinimalApi>>();

                foreach (var api in apis)
                {
                    api.RegisterEndpints(app);
                }
                return app;

            }

        }
    }



