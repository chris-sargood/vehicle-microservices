using Nest;

namespace VehiclesSearchConsumer.Models;

[ElasticsearchType(RelationName = "vehicle")]
public class Vehicle
{
    [Keyword]
    public Guid Id { get; set; }
    [Text]
    public string Description { get; set; }
    [Keyword]
    public string Make { get; set; }
    [Keyword]
    public string Model { get; set; }
    public Decimal Price { get; set; }
}