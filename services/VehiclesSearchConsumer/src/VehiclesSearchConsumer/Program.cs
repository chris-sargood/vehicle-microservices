// See https://aka.ms/new-console-template for more information

using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using VehiclesSearchConsumer.Consumers;
using VehiclesSearchConsumer.Extensions;

await Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false);
        config.AddEnvironmentVariables();
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<VehicleAddedConsumer>();

            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(hostContext.Configuration["RabbitMq:Host"], "/", h => {
                    h.Username(hostContext.Configuration["RabbitMq:User"]);
                    h.Password(hostContext.Configuration["RabbitMq:Password"]);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
        services.AddElasticsearch(hostContext.Configuration);
    })
    .Build()
    .RunAsync();