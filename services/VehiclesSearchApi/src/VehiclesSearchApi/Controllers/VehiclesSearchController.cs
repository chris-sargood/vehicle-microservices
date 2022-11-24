using Microsoft.AspNetCore.Mvc;
using Nest;
using VehicleSearchApi.DAL.Elastic.Aggregations;
using VehicleSearchApi.DAL.Elastic.Models;
using VehicleSearchApi.DAL.Elastic.Queries;
using VehiclesSearchApi.Models;

namespace VehiclesSearchApi.Controllers;

[ApiController]
[Route("[controller]")]
public class VehiclesSearchController : ControllerBase
{
    private readonly ILogger<VehiclesSearchController> _logger;
    private readonly IElasticClient _elasticClient;
    private readonly IAggregationBuilder _aggregationBuilder;
    private readonly IQueryBuilder _queryBuilder;

    public VehiclesSearchController(ILogger<VehiclesSearchController> logger,
        IElasticClient elasticClient,
        IAggregationBuilder aggregationBuilder,
        IQueryBuilder queryBuilder)
    {
        _logger = logger;
        _elasticClient = elasticClient;
        _aggregationBuilder = aggregationBuilder;
        _queryBuilder = queryBuilder;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] VehicleSearchRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        ISearchResponse<Vehicle> searchResponse;
        try
        {
            searchResponse = await _elasticClient.SearchAsync<Vehicle>(s => s
                .Query(q => _queryBuilder
                    .BuildSearchQuery(model.Keywords, model.Make))
                .Aggregations(_aggregationBuilder
                    .BuildSearchFacetAggregation()));
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return Problem();
        }

        return Ok(new
        {
            Results =
                searchResponse?.Documents?.Select(x => new
                {
                    x.Id,
                    x.Description,
                    x.Make,
                    x.Model,
                    x.Price,
                }),
            Facets = searchResponse?.Aggregations?.Terms("make")?.Buckets?.Select(b => new
            {
                Make = b.Key,
                Count = b.DocCount
            })
        });
    }
}