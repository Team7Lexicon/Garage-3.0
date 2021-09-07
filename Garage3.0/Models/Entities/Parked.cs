using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3._0.Models
{
    public class Parked
    {
        public int Id { get; set; }        

        // Foreign key
        public int ParkingSpotId { get; set; }
        public int VehicleId { get; set; }

        //Nav property        
        public ICollection<ParkingSpot> ParkingSpots { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
