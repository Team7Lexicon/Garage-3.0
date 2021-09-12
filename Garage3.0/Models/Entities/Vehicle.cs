using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garage3._0.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        //[CheckRegistrationNo(7)]
        public string RegNo { get; set; }
        [MaxLength(20, ErrorMessage = "20 characters max")]
        public string Color { get; set; }
        [MaxLength(25, ErrorMessage = "25 characters max")]
        public string Brand { get; set; }
        [MaxLength(50, ErrorMessage = "50 characters max")]
        public string Model { get; set; }
        [Range(1,10, ErrorMessage = "Number of wheels cannot be negative. 10 wheels max")]
        [Display(Name = "No of wheels")]
        public int Wheels { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ArrivalTime { get; set; }
        public bool IsParked { get; set; }
        //Foreign Keys
        public int MemberId { get; set; }
        public int VehicleTypeId { get; set; }

        //Navigation Properties
        public Member Member { get; set; }
        public VehicleType VehicleType { get; set; }
        public ICollection<Parked> Parkeds { get; set; }
    }
}