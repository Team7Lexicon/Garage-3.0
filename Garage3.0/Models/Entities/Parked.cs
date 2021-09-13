using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3._0.Models
{
    public class Parked
    {
        public int Id { get; set; }        

        //Foreign Keys
        public int ParkingSpotId { get; set; }
        public int VehicleId { get; set; }
        public int VehicleType { get; set; }//William

        //Navigation properties        
        public ParkingSpot ParkingSpot { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
