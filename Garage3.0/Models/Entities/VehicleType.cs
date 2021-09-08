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
        public double ParkingSize { get; set; } = 0;

        //Navigation Property
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
