using Garage3._0.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3._0.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
//        [CheckRegistrationNo(7)]
        public string RegNo { get; set; }
        [MaxLength(20)]
        public string Color { get; set; }
        [MaxLength(25)]
        public string Brand { get; set; }
        [MaxLength(50)]
        public string Model { get; set; }
        [Range(2,10)]
        [Display(Name = "No of wheels")]
        public int Wheels { get; set; }        
        public DateTime ArrivalTime { get; set; }
        public bool IsParked { get; set; }

        //Foreign Keys
        public int MemberId { get; set; }

        //Navigation Properties
        public Member Member { get; set; }
        public VehicleType VehicleType { get; set; }
        public ICollection<Parked> Parkeds { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Vehicle vehicle)
                return RegNo.Equals(vehicle.RegNo);
            return false;
        }
    }
}
