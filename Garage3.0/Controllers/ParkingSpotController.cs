using Garage3._0.Data;
using Garage3._0.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3._0.Controllers
{
    public class ParkingSpotController : Controller
    {
        private readonly Garage3_0Context db;

        public ParkingSpotController(Garage3_0Context db)
        {
            this.db = db;
        }
        public IActionResult Overview()
        {
            var spots = Enumerable.Range(1, 100).ToList();
            var result = spots.Except(db.Parked.Select(p => p.ParkingSpotId));

            int NumberOfParkedVehicles = db.Vehicle.Count(v => v.IsParked == true);

            ViewBag.data = result;

            var model = new ParkinOverviewModel
            {
                Result = result,
                totalParkingPlace = NumberOfParkedVehicles,
            };

            return View( model);
        }
    }
}
