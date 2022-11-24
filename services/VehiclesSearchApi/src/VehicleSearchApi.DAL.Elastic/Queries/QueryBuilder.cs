using Nest;
using VehicleSearchApi.DAL.Elastic.Models;

namespace VehicleSearchApi.DAL.Elastic.Queries;

public class QueryBuilder : IQueryBuilder
{
    public QueryContainer BuildSearchQuery(string? keywords, string? make)
    {
        var boolQuery = new QueryContainer();

        boolQuery &=
            DescriptionQuery(keywords);

        if (make != null)
        {
            boolQuery &= MakeTermFilter(make);
        }

        return boolQuery;
    }
    
    private static TermQuery MakeTermFilter(string make)
    {
        return new TermQuery { Field = Infer.Field<Vehicle>(f => f.Make), Value = make };
    }

    private static MatchQuery DescriptionQuery(string? keywords)
    {
        return new MatchQuery() { Field = Infer.Field<Vehicle>(f => f.Description), Query = keywords };
    }
}