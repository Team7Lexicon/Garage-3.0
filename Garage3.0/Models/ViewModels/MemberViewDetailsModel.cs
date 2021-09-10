using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3._0.Models.ViewModels
{
    public class MemberViewDetailsModel
    {
        public int Id { get; set; }
        public string PersonNo { get; set; }       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public MembershipLevels MembershipLevel { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
