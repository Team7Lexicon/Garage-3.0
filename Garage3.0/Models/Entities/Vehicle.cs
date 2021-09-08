using Garage3._0.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3._0.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        [CheckRegistrationNo(7)]
        public string RegNo { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Wheels { get; set; }        
        public DateTime ArrivalTime { get; set; }

        //Foreign key
        public int MemberId { get; set; }

        //Nav Property
        public Member Member { get; set; }
        public VehicleType VehicleType { get; set; }

        public ICollection <Parked> Parkeds { get; set; }
    }
}
