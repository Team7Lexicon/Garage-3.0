using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Garage3._0.Models
{
    public class VehiclesIndexViewModel
    {
        public IEnumerable<Vehicle> Vehicles { get; set; }
        public int Id { get; set; }

        [Display(Name = "License plate")]
        public string RegNo { get; set; }

        [Display(Name = "Owner")]
        public string FullName { get; set; }

        [Display(Name = "Membership")]
        public MembershipLevels MembershipLevel { get; set; }

        [Display(Name = "Type")]
        public VehicleType VehicleType { get; set; }
        public bool IsParked { get; set; }
        [DisplayFormat(DataFormatString = @"{0:dd\:hh\:mm}")]
        public TimeSpan? ParkedTime { get; set; }
    }
}