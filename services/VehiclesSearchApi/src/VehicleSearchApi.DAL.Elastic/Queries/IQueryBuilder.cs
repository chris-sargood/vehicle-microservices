using Nest;

namespace VehicleSearchApi.DAL.Elastic.Queries;

public interface IQueryBuilder
{
    QueryContainer BuildSearchQuery(string? keywords, string? make);
}