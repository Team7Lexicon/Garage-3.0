using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3._0.Models
{
    public class ParkinOverviewModel
    {
        public IEnumerable<int> Result { get; set; }
        public IQueryable<Parked> PardkedVeicType { get; set; }
        public int totalParkingPlace { get; set; }
    }
}
