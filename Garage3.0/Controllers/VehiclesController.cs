using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage3._0.Data;
using Garage3._0.Models;
using System.Collections.Generic;

namespace Garage3._0.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly Garage3_0Context _context;

        public VehiclesController(Garage3_0Context context)
        {
            _context = context;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber, VehiclesIndexViewModel viewModel)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }

            ViewData["RegNoSortParm"] = String.IsNullOrEmpty(sortOrder) ? "regNo_desc" : "";
            ViewData["FullNameSortParm"] = sortOrder == "FullName" ? "fullName_desc" : "FullName";
            ViewData["MembershipLevelSortParm"] = sortOrder == "MembershipLevel" ? "membershipLevel_desc" : "MembershipLevel";
            ViewData["VehicleTypeSortParm"] = sortOrder == "VehicleType" ? "vehicleType_desc" : "VehicleType";
            ViewData["ParkedTimeSortParm"] = sortOrder == "ParkedTime" ? "parkedTime_desc" : "ParkedTime";

            var vehicles = from s in _context.Vehicle.Include(v => v.Member).Include(v => v.VehicleType) select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                vehicles = vehicles.Where(s => s.RegNo.Contains(searchString) || s.VehicleType.Equals(currentFilter));
            }

            vehicles = viewModel.VehicleType == null ?
                            vehicles :
                            vehicles.Where(m => m.VehicleType == viewModel.VehicleType);

            IEnumerable<SelectListItem> vehicleTypeSelectItems = await GetVehicleTypeSelectListItems();

            var viewModels = new List<VehiclesIndexViewModel>();
            
            IEnumerable<Member> members = _context.Member;

            foreach (var vehicle in vehicles)
            {
                if (vehicle.IsParked == true)
                {
                    VehiclesIndexViewModel viewModel2 = CreateVehiclesIndexViewModel(vehicle, members);
                    viewModels.Add(viewModel2);
                }
            }

            switch (sortOrder)
            {
                case "regNo_desc":
                    viewModels = viewModels.OrderByDescending(s => s.RegNo).ToList();
                    break;
                case "FullName":
                    viewModels = viewModels.OrderBy(s => s.FullName).ToList();
                    break;
                case "fullName_desc":
                    viewModels = viewModels.OrderByDescending(s => s.FullName).ToList();
                    break;
                case "MembershipLevel":
                    viewModels = viewModels.OrderBy(s => s.MembershipLevel).ToList();
                    break;
                case "membershipLevel_desc":
                    viewModels = viewModels.OrderByDescending(s => s.MembershipLevel).ToList();
                    break;
                case "VehicleType":
                    viewModels = viewModels.OrderBy(s => s.VehicleType).ToList();
                    break;
                case "vehicleType_desc":
                    viewModels = viewModels.OrderByDescending(s => s.VehicleType).ToList();
                    break;
                case "ParkedTime":
                    viewModels = viewModels.OrderBy(s => s.ParkedTime).ToList();
                    break;
                case "parkedTime_desc":
                    viewModels = viewModels.OrderByDescending(s => s.ParkedTime).ToList();
                    break;
                default:
                    viewModels = viewModels.OrderBy(s => s.RegNo).ToList();
                    break;
            }

            //int pageSize = 10;
            return View(new Tuple<IEnumerable<VehiclesIndexViewModel>, IEnumerable<SelectListItem>>
                                 (viewModels, vehicleTypeSelectItems));
        }

        public async Task<IActionResult> Filter(VehiclesIndexViewModel viewModel)
        {
            var vehicles = string.IsNullOrWhiteSpace(viewModel.RegNo) ?
                            _context.Vehicle :
                            _context.Vehicle.Where(m => m.RegNo.StartsWith(viewModel.RegNo));

            vehicles = viewModel.VehicleType == null ?
                            vehicles :
                            vehicles.Where(m => m.VehicleType == viewModel.VehicleType);

            var model = new VehiclesIndexViewModel
            {
                Vehicles = await vehicles.ToListAsync()
            };

            return View(nameof(Index), model);

        }

        // GET: Vehicles/DetailsVehicle/5
        public async Task<IActionResult> DetailsVehicle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(m => m.Member)
                .Include(t => t.VehicleType)
                .FirstOrDefaultAsync(v => v.Id == id);

            var vehiclesDetailsVehicleViewModel = new VehiclesDetailsViewModel
            {
                Id = vehicle.Id,
                RegNo = vehicle.RegNo,
                FullName = vehicle.Member.FullName,
                VehicleType = vehicle.VehicleType,
                Model = vehicle.Model,
                Color = vehicle.Color,
                Wheels = vehicle.Wheels,
                Brand = vehicle.Brand,
                ArrivalTime = vehicle.ArrivalTime
            };

            if (vehiclesDetailsVehicleViewModel == null)
            {
                return NotFound();
            }
            return View(vehiclesDetailsVehicleViewModel);
        }

        // GET: Vehicles/CheckInNewVehicle
        public IActionResult CheckInNewVehicle()
        {
            var vehiclesCheckInNewVehicleViewModel = new VehiclesCheckInNewViewModel
            {
                GetVehiclesType = GetTypeOfVehicle(),
                GetMembers = GetMemberList(),
                IsParked = false
            };

            return View(vehiclesCheckInNewVehicleViewModel);
        }

        // POST: Vehicles/CheckInNewVehicle/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckInNewVehicle([Bind("Id,RegNo,Color,Brand,Model,Wheels,MemberId,VehicleTypeId")] VehiclesCheckInNewViewModel vehiclesCheckInNewVehicleViewModel)
        {
            if (ModelState.IsValid)
            {
                var vehicle = new Vehicle
                {
                    RegNo = vehiclesCheckInNewVehicleViewModel.RegNo,
                    VehicleTypeId = vehiclesCheckInNewVehicleViewModel.VehicleTypeId,
                    Color = vehiclesCheckInNewVehicleViewModel.Color,
                    Brand = vehiclesCheckInNewVehicleViewModel.Brand,
                    Model = vehiclesCheckInNewVehicleViewModel.Model,
                    Wheels = vehiclesCheckInNewVehicleViewModel.Wheels,
                    MemberId = vehiclesCheckInNewVehicleViewModel.MemberId,
                    ArrivalTime = DateTime.Now,
                    IsParked = true
                };

                try
                {
                    _context.Add(vehicle);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(DetailsVehicle), new { id = vehiclesCheckInNewVehicleViewModel.Id });
                }
                catch (Exception)
                {
                    ViewBag.ExistMessage = $"A vehicle with license plate {vehiclesCheckInNewVehicleViewModel.RegNo} is already registered";
                }
                return RedirectToAction(nameof(DetailsVehicle), new { id = vehiclesCheckInNewVehicleViewModel.Id });
        }
            return View(vehiclesCheckInNewVehicleViewModel);
        }

        // GET: Vehicles/EditVehicle/5
        public async Task<IActionResult> EditVehicle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle.FindAsync(id);

            var vehiclesEditVehicleViewModel = new VehiclesEditViewModel
            {
                Id = vehicle.Id,
                RegNo = vehicle.RegNo,
                VehicleTypeId = vehicle.VehicleTypeId,
                Color = vehicle.Color,
                Wheels = vehicle.Wheels,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                GetVehiclesType = GetTypeOfVehicle(),
            };

            if (vehiclesEditVehicleViewModel == null)
            {
                return NotFound();
            }
            return View(vehiclesEditVehicleViewModel);
        }

        private IEnumerable<SelectListItem> GetTypeOfVehicle()
        {
            var vehicleTypes = _context.VehicleType;
            var GetTypeOfVehicle = new List<SelectListItem>();
            foreach (var type in vehicleTypes)
            {
                var newVehicleType = (new SelectListItem
                {
                    Text = type.Name,
                    Value = type.Id.ToString(),
                });
                GetTypeOfVehicle.Add(newVehicleType);
            }
            return (GetTypeOfVehicle);
        }

        private IEnumerable<SelectListItem> GetMemberList()
        {
            var memberList = _context.Set<Member>()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Id + ". " + x.FullName
                }).ToList();
            return (memberList);
        }

        // POST: Vehicles/EditVehicle/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVehicle(int id, [Bind("Id,RegNo,VehicleTypeId,Color,Brand,Model,Wheels")] VehiclesEditViewModel vehiclesEditVehicleViewModel)
        {
            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    vehicle.RegNo = vehiclesEditVehicleViewModel.RegNo;
                    vehicle.VehicleTypeId = vehiclesEditVehicleViewModel.VehicleTypeId;
                    vehicle.Color = vehiclesEditVehicleViewModel.Color;
                    vehicle.Brand = vehiclesEditVehicleViewModel.Brand;
                    vehicle.Model = vehiclesEditVehicleViewModel.Model;
                    vehicle.Wheels = vehiclesEditVehicleViewModel.Wheels;
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(DetailsVehicle), new { id = vehicle.Id });
            }
            return View(vehiclesEditVehicleViewModel);
        }

        // GET: Vehicles/DeleteVehicle/5
        public async Task<IActionResult> DeleteVehicle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.Id == id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/DeleteVehicle/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            _context.Vehicle.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Vehicles/CheckOutVehicle/5
        public async Task<IActionResult> CheckOutVehicle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.Member)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/CheckOutVehicle/5
        [HttpPost, ActionName("CheckOutVehicle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOutVehicleConfirmed(int id)
        {
            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.Id == id);

            vehicle.IsParked = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = vehicle.Id });
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicle.Any(e => e.Id == id);
        }

        private async Task<List<SelectListItem>> GetVehicleTypeSelectListItems()
        {
            var vehicleTypeSelectItems = new List<SelectListItem>();
            var vehicleTypes = await _context.VehicleType.ToListAsync();
            foreach (VehicleType vehicleType in vehicleTypes)
            {
                var item = new SelectListItem()
                {
                    Value = vehicleType.Id.ToString(),
                    Text = vehicleType.Name
                };
                vehicleTypeSelectItems.Add(item);
            }
            return vehicleTypeSelectItems;
        }

        private static VehiclesIndexViewModel CreateVehiclesIndexViewModel(Vehicle vehicles, IEnumerable<Member> members)
        {
            var model = new VehiclesIndexViewModel();
            var member = members.FirstOrDefault(m => m.Id == vehicles.MemberId);
            model.Id = vehicles.Id;
            model.RegNo = vehicles.RegNo;
            model.ParkedTime = DateTime.Now - vehicles.ArrivalTime;
            model.VehicleType = vehicles.VehicleType;
            model.FullName = member.FullName;
            model.MembershipLevel = member.MembershipLevel;
            return model;
        }
    }
}