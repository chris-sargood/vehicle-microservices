using VehiclesApi.Domain;
using VehiclesApi.Models;

namespace VehiclesApi.Extensions;

public static class VehicleExtensions
{
    public static VehicleResponse ToVehicleResponse(this Vehicle entity)
    {
        return new VehicleResponse()
        {
            Id = entity.Id,
            Description = entity.Description,
            Make = entity.Make,
            Model = entity.Model,
            Price = entity.Price,
            AddedOn = entity.AddedOn
        };
    }
}