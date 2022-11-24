using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using VehiclesApi.DAL.EfCore.Contexts;
using VehiclesApi.DAL.EfCore.Repositories;
using VehiclesApi.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<VehicleDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly(typeof(VehicleDbContext).Assembly.FullName)));
builder.Services.AddTransient<IVehicleRepository, VehicleRepository>();
builder.Services.AddTransient<ISystemClock, SystemClock>();

builder.Services.AddMassTransit(x =>
{

    x.UsingRabbitMq((context,cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", h => {
            h.Username(builder.Configuration["RabbitMq:User"]);
            h.Password(builder.Configuration["RabbitMq:Password"]);
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddOptions<MassTransitHostOptions>()
    .Configure(options =>
    {
        options.WaitUntilStarted = true;
        options.StartTimeout = TimeSpan.FromSeconds(10);
        options.StopTimeout = TimeSpan.FromSeconds(30);
    });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Doing this for simplicity on this demo project. I wouldn't recommend for production.
// Should be part of CI/CD pipeline
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VehicleDbContext>();
    db.Database.Migrate();
}

app.Run();