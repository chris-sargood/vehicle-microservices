using MassTransit;
using Nest;
using VehicleEventContracts;
using VehiclesSearchConsumer.Models;

namespace VehiclesSearchConsumer.Consumers;

public class VehicleAddedConsumer : IConsumer<VehicleAdded>
{
    private readonly IElasticClient _client;

    public VehicleAddedConsumer(IElasticClient client)
    {
        _client = client;
    }
    
    public async Task Consume(ConsumeContext<VehicleAdded> context)
    {
        await _client.IndexDocumentAsync(new Vehicle()
        {
            Id = context.Message.Id,
            Description = context.Message.Description,
            Make = context.Message.Make,
            Model = context.Message.Model,
            Price = context.Message.Price
        });
    }
}