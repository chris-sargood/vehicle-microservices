using System;

namespace VehiclesApi.Domain
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public Decimal Price { get; set; }
        public DateTime AddedOn { get; set; }
    }
}