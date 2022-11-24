using System;
using System.Threading.Tasks;

namespace VehiclesApi.Domain.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Vehicle?> GetById(Guid id);
        Task Add(Vehicle? entity);
    }
}