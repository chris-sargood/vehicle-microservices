using Microsoft.EntityFrameworkCore;
using VehiclesApi.Domain;

namespace VehiclesApi.DAL.EfCore.Contexts;

public class VehicleDbContext : DbContext
{
    //todo this is extremly basic.....implement a code first approach 
    public VehicleDbContext(DbContextOptions<VehicleDbContext> options) : base(options)
    {
    }
    
    public DbSet<Vehicle?> Vehicles { get; set; }
}