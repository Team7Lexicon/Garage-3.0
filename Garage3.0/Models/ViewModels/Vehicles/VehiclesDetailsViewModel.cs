using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Garage3._0.Models
{
    public class VehiclesDetailsViewModel
    {
        public int Id { get; set; }
        [Display(Name = "License plate")]
        public string RegNo { get; set; }
        [Display(Name = "Owner")]
        public string FullName { get; set; }
        [Display(Name = "Type")]
        public VehicleType VehicleType { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        [Display(Name = "No of wheels")]
        public int Wheels { get; set; }
        [Display(Name = "Arrival time")]
        public DateTime? ArrivalTime { get; set; }
        public int VehicleTypeId { get; set; }
    }
}