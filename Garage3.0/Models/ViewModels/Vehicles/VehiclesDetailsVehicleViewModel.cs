using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Garage3._0.Models
{
    public class VehiclesDetailsVehicleViewModel
    {
        public int Id { get; set; }
        [Display(Name = "License plate")]
        public string RegNo { get; set; }
        [Display(Name = "Owner")]
        public string FullName { get; set; }
        [Display(Name = "Type")]
        public VehicleType VehicleType { get; set; }
        [MaxLength(20, ErrorMessage = "20 characters max")]
        public string Color { get; set; }
        [MaxLength(25, ErrorMessage = "25 characters max")]
        public string Brand { get; set; }
        [MaxLength(50, ErrorMessage = "50 characters max")]
        public string Model { get; set; }
        [Range(1, 10, ErrorMessage = "Number of wheels cannot be negative. 10 wheels max")]
        [Display(Name = "No of wheels")]
        public int Wheels { get; set; }
        [Display(Name = "Arrival time")]
        public DateTime ArrivalTime { get; set; }
    }
}
