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
            //Get the id of the parked vehicles
            var result = spots.Except(db.Parked.Select(p => p.ParkingSpotId));
            //Get the total number of the parked vvehicles
            int NumberOfParkedVehicles = db.Vehicle.Count(v => v.IsParked == true);
          
            //types of parked vehicles?
            var tmpParkedVeic = db.Parked.Select(v => v);

            //check if buss can park
            int spot = 10;
            int checkSpot = CheckParkingSpotForBuss(spot, db);
            
            ViewBag.data = result;

            var model = new ParkinOverviewModel
            {
                Result = result,
                totalParkingPlace = NumberOfParkedVehicles,
                PardkedVeicType = tmpParkedVeic,
            };

            return View( model);
        }
        static int CheckParkingSpotForBuss(int spot, Garage3_0Context db) 
        {
            if (db.Parked.FirstOrDefault(v => v.ParkingSpotId == (spot)) == null)//Empty with available left and right
            {
                //check left
                if (db.Parked.FirstOrDefault(v => v.ParkingSpotId == (spot - 1)) != null)//if left taken
                {
                    return 1;
                }
                //check right
                else if (db.Parked.FirstOrDefault(v => v.ParkingSpotId == (spot + 1)) != null)//if right taken
                {
                    return 2;
                }
                return 0;
            }            
            //Left available
            /*if (db.Parked.FirstOrDefault(v => v.ParkingSpotId == (spot - 1)) != null)
            {
                return 1;
            }
            else if (db.Parked.FirstOrDefault(v => v.ParkingSpotId == (spot + 1)) != null)//right available
            {
                return 2;
            }
            else*/ 
            else return 3;//right and left
        }
    }
}
