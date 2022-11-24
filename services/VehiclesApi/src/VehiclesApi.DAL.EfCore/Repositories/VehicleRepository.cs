using VehiclesApi.DAL.EfCore.Contexts;
using VehiclesApi.Domain;
using VehiclesApi.Domain.Interfaces;

namespace VehiclesApi.DAL.EfCore.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly VehicleDbContext _context;

    public VehicleRepository(VehicleDbContext context)
    {
        _context = context;
    }

    public async Task<Vehicle?> GetById(Guid id)
    {
        return await _context.Vehicles.FindAsync(id);
    }

    public async Task Add(Vehicle? entity)
    {
        await _context.Vehicles.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
}