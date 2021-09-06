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
        public string PersonNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public MembershipLevels MembershipLevel { get; set; }
    }
}
