using System;
using Nest;
using VehicleSearchApi.DAL.Elastic.Models;

namespace VehicleSearchApi.DAL.Elastic.Aggregations;

public interface IAggregationBuilder
{
    Func<AggregationContainerDescriptor<Vehicle>, IAggregationContainer> BuildSearchFacetAggregation(int size = 10);
}