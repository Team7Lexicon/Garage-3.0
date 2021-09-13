using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Garage3._0.Models
{
    public class VehicleType : IComparable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double ParkingSize { get; set; }

        public int CompareTo(object obj)
        {
            if (obj is VehicleType type)
            {
                return Name.CompareTo(type.Name);
            }
            throw new ArgumentException("This is not a VehicleType");
        }

        //Navigation Property
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}