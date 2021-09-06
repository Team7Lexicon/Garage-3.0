using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3._0.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }

        // Fixing n-m realtionship with parking slots and vehicles
        public int ParkingSpotId { get; set; }
        public int VehicleId { get; set; }
        public ICollection<ParkingSpot> parkingSpot { get; set; }
        public ICollection<Vehicle> vehicle { get; set; }
    }
}
