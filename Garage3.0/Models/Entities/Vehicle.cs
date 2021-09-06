using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3._0.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string RegNo { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Wheels { get; set; }
        public int MemberId { get; set; }

        public Member member { get; set; }
    }
}
