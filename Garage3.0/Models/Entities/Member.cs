using Garage3._0.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3._0.Models
{
    public class Member
    {
        public int Id { get; set; }

        [Required]
        [CheckPersonNo(12)]
        public string PersonNo { get; set; }
        [CheckFirstAndLastNames]
        public string FirstName { get; set; }
        [CheckFirstAndLastNames]
        public string LastName { get; set; }
        [CheckEmail]
        public string Email { get; set; }
        public DateTime RegistrationTime { get; set; }
        //[CheckPassWord]
        public string Password { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public MembershipLevels MembershipLevel { get; set; }

        //Navigation Property
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
