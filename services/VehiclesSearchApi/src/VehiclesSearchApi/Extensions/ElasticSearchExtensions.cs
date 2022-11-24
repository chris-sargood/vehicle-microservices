using Nest;
using VehicleSearchApi.DAL.Elastic.Models;

namespace VehiclesSearchApi.Extensions;

public static class ElasticSearchExtensions
{
    public static void AddElasticsearch(
        this IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration["elasticsearch:url"];
        var settings = new ConnectionSettings(new Uri(url))
            .DefaultMappingFor<Vehicle>(deviceMapping => deviceMapping
                .IndexName("vehicles")
                .IdProperty(dev => dev.Id)
            );
        var client = new ElasticClient(settings);
        services.AddSingleton<IElasticClient>(client);
    }
}