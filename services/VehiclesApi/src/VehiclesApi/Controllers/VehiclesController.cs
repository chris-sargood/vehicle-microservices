using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Internal;
using VehicleEventContracts;
using VehiclesApi.Domain;
using VehiclesApi.Domain.Interfaces;
using VehiclesApi.Extensions;
using VehiclesApi.Models;

namespace VehiclesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly ILogger<VehiclesController> _logger;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly ISystemClock _systemClock;
    private readonly IPublishEndpoint _publishEndpoint;

    public VehiclesController(ILogger<VehiclesController> logger, 
        IVehicleRepository vehicleRepository,
        ISystemClock systemClock, 
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _vehicleRepository = vehicleRepository;
        _systemClock = systemClock;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<IActionResult> Get(Guid id)
    {
        Vehicle? response = null;
        try
        {
            response = await _vehicleRepository.GetById(id);
            if (response == null)
            {
                return NotFound();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return Problem();
        }

        //If this was a large volatile object I might look at something like automapper.
        //For demo purposes, an extension method manually mapping the entity to a API response dto is fine
        return Ok(response.ToVehicleResponse());
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] VehicleRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        var id = Guid.NewGuid();
        try
        {
            var vehicle = new Vehicle()
            {
                Id = id,
                Description = model.Description,
                Make = model.Make.ToLower(),
                Model = model.Model.ToLower(),
                Price = model.Price,
                AddedOn = _systemClock.UtcNow.DateTime
            };
            
            // For this demo we'll write to the source of truth in a synchronous manner. 
            // For a production system we would need to decide if this was appropriate 
            // or if an async transaction via a message queue would be needed. 
            await _vehicleRepository.Add(vehicle);
            
            // let our subscribers know that the source of truth has been updated.
            // In the case of this demo the subscriber is the search index consumer
            await _publishEndpoint.Publish<VehicleAdded>(new {
                Id = id,
                vehicle.Description,
                vehicle.Make,
                vehicle.Model,
                vehicle.Price,
                vehicle.AddedOn
            });

        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return Problem();
        }
        
        return CreatedAtAction(nameof(Get), new { id });
    }
}