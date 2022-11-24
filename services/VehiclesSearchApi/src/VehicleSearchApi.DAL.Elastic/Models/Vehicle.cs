using System;
using Nest;

namespace VehicleSearchApi.DAL.Elastic.Models;

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
    [FloatRange]
    public Decimal Price { get; set; }
}