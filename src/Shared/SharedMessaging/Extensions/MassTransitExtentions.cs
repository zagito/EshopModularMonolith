using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace SharedMessaging.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitForAssemblies(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();
            config.SetInMemorySagaRepositoryProvider();
            config.AddConsumers(assemblies);
            config.AddSagaStateMachines(assemblies);
            config.AddSagas(assemblies);
            config.AddActivities(assemblies);

            //config.UsingInMemory((context, configurator) =>
            //{
            //    configurator.ConfigureEndpoints(context);
            //});

            config.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(configuration.GetConnectionString("eshop-mq"));
                configurator.ConfigureEndpoints(context);
            });

        });

        return services;
    }
}
