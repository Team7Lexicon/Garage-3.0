using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3._0.Models
{
    public class ParkingSpot
    {        
        public int Id { get; set; }

        public int ParkingSpotNumber { get; set; }

        //Navigation Property        
        public ICollection<Parked> Parkeds { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
