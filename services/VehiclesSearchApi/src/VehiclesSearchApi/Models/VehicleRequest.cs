using System.ComponentModel.DataAnnotations;

namespace VehiclesSearchApi.Models;

public class VehicleSearchRequest
{
    [MaxLength(255)]
    public string? Keywords { get; set; }
    [MaxLength(100)]
    public string? Make { get; set; }
}