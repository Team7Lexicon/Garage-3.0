using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Garage3._0.Models
{
    public class VehiclesViewEditModel
    {
        public int Id { get; set; }

        [Display(Name = "License plate")]
        public string RegNo { get; set; }
        [Display(Name = "Owner")]
        public string FullName { get; set; }
        [Display(Name = "Type")]
        public VehicleType VehicleType { get; set; }
        [MaxLength(20)]
        public string Color { get; set; }
        [MaxLength(25)]
        public string Brand { get; set; }
        [MaxLength(50)]
        public string Model { get; set; }
        [Range(2, 10)]
        [Display(Name = "No of wheels")]
        public int Wheels { get; set; }
    }
}
