using MassTransit.Consumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using VehiclesSearchConsumer.Models;

namespace VehiclesSearchConsumer.Extensions;

public static class ElasticSearchExtensions
{
    public static void AddElasticsearch(
        this IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration["elasticsearch:url"];
        var settings = new ConnectionSettings(new Uri(url))
                .DefaultMappingFor<Vehicle>(deviceMapping => deviceMapping
                    .IndexName(configuration["elasticsearch:IndexName"])
                    .IdProperty(dev => dev.Id)
                );
        var client = new ElasticClient(settings);
        
        var response = client.Indices.Create(configuration["elasticsearch:IndexName"], creator => creator
            .Map<Vehicle>(device => device
                .AutoMap()
            )
        );

        if (response.Acknowledged)
        {
            // This is a very 'prototype' thing to do. Obviously not production ready.
            // Would probably be best handled in the docker compose file with a script, but this is the 
            // quick n dirty method
            client.Bulk(u => u.Index(configuration["elasticsearch:IndexName"])
                .IndexMany(DummyVehicles()));
        }

        services.AddSingleton<IElasticClient>(client);
    }

    private static Vehicle[] DummyVehicles()
    {
        return new[]
        {
            new Vehicle()
            {
                Id = Guid.NewGuid(),
                Make = "audi",
                Model = "a1 sportback",
                Description = "The perfect urban accomplice; compact and taut, without compromising on style or detail. A dynamic design and refined interior offer new levels of spaciousness and comfort, making the Audi A1 Sportback perfect for longer adventures too.",
                Price = 30000.99m
            },
            new Vehicle()
            {
                Id = Guid.NewGuid(),
                Make = "audi",
                Model = "a4",
                Description = "Polished and progressive, our powerful A4 Saloon offers the ultimate combination of athletic style and practicality for those needing a sports car with versatility. With a sharp, classic look and premium interior full of innovative technologies to aid your driving experience, the A4 Saloon offers the whole package.",
                Price = 50000.99m
            },
            new Vehicle()
            {
                Id = Guid.NewGuid(),
                Make = "peugeot",
                Model = "208",
                Description = "PEUGEOT 208 shows off its youthful side with its distinctive sporty shape with the interior revealing the original PEUGEOT i-Cockpit® 3D. Discover PEUGEOT's Power of Choice  commitment, this city car gives you the freedom to choose a petrol, diesel or electric engine.",
                Price = 25999.99m
            },
            new Vehicle()
            {
                Id = Guid.NewGuid(),
                Make = "volvo",
                Model = "ex90",
                Description = "We’re human, we make mistakes. So if life happens to sometimes distract you, we are here to help keep you safe. With the new Volvo EX90 we enter a new era for safety.",
                Price = 45000.99m
            }
        };

    }
}