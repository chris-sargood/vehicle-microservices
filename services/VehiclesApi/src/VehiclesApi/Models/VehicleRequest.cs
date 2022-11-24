using System.ComponentModel.DataAnnotations;

namespace VehiclesApi.Models;

public class VehicleRequest
{   
    [Required]
    [MaxLength(255)]
    public string Description { get; set; }
    [Required]
    [MaxLength(100)]
    public string Make { get; set; }
    [Required]
    [MaxLength(100)]
    public string Model { get; set; }
    [Required]
    public Decimal Price { get; set; }
}