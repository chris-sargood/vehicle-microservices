using System;
using Nest;
using VehicleSearchApi.DAL.Elastic.Models;

namespace VehicleSearchApi.DAL.Elastic.Aggregations;

public class AggregationBuilder : IAggregationBuilder
{
    public Func<AggregationContainerDescriptor<Vehicle>, IAggregationContainer> BuildSearchFacetAggregation(int size = 10)
    {
        return a => a
            .Terms(Constants.Constants.MakeAggregationName, st => st
                .Field(f => f.Make)
                .Size(10));
    }
}