using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Garage3._0.Models
{
    public class VehicleType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double ParkingSize { get; set; }

        //Navigation Property
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}