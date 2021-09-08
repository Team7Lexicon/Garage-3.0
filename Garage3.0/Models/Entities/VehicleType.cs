using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3._0.Models
{
    public class VehicleType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParkingSize { get; set; }
        
        //Foreign Key
        public int VehicleId { get; set; }

        //Navigation Property
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
