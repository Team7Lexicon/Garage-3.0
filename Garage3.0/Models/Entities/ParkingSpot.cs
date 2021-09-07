using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3._0.Models
{
    public class ParkingSpot
    {        
        public int Id { get; set; }

        //Nav property        
        public Parked Parked { get; set; }      
    }
}
