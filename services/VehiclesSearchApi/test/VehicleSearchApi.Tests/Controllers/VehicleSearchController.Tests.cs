using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;
using NSubstitute;
using VehicleSearchApi.DAL.Elastic.Aggregations;
using VehicleSearchApi.DAL.Elastic.Models;
using VehicleSearchApi.DAL.Elastic.Queries;
using VehiclesSearchApi.Controllers;
using VehiclesSearchApi.Models;
using Xunit;

namespace VehicleSearchApi.Tests.Controllers;

public class VehicleSearchControllerTests
{
    [Fact]
    public async Task Get_ModelNotValid_400Returned()
    {
        var sut = GetSut();
        sut.ModelState.AddModelError("Keywords", "Required");

        var result = await sut.Get(new VehicleSearchRequest()) as StatusCodeResult;
        result?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }
    
   [Fact]
    public async Task Get_ExceptionThrown_500Returned()
    {
        var client = Substitute.For<IElasticClient>();
        client.SearchAsync<Vehicle>(Arg.Any<SearchDescriptor<Vehicle>?>())
            .Returns<Task>(x => { throw new Exception("Boom"); });
        var sut = GetSut(elasticClientOverride:client);

        var result = await sut.Get(new VehicleSearchRequest()) as StatusCodeResult;
        result?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
    
    private static VehiclesSearchController GetSut(IQueryBuilder? queryBuilderOverride = null, 
        IElasticClient? elasticClientOverride = null)
    {
        var logger = Substitute.For<ILogger<VehiclesSearchController>>();
        var client = elasticClientOverride ?? Substitute.For<IElasticClient>();
        var aggregationBuilder = Substitute.For<IAggregationBuilder>();
        var queryBuilder = queryBuilderOverride ?? Substitute.For<IQueryBuilder>();

        return new VehiclesSearchController(logger, client, aggregationBuilder, queryBuilder);
    }
}