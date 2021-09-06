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
        public int Size { get; set; }  // number of parking slots divided by 3 i e motocycle size
        public int VehicleId { get; set; }
        public Vehicle vehicle { get; set; }
    }
}
