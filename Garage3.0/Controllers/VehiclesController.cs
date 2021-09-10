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
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber, VehiclesViewModel viewModel)
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

            var vehicles = from s in _context.Vehicle.Include(v => v.Member) select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                vehicles = vehicles.Where(s => s.RegNo.Contains(searchString) || s.VehicleType.Equals(currentFilter));
            }
            vehicles = viewModel.VehicleType == null ?
                            vehicles :
                            vehicles.Where(m => m.VehicleType == viewModel.VehicleType);

            IEnumerable<SelectListItem> vehicleTypeSelectItems = await GetVehicleTypeSelectListItems();

            var viewModels = new List<VehiclesViewModel>();
            IEnumerable<Member> members = _context.Member;

            foreach (Vehicle vehicle in vehicles)
            {
                if (vehicle.IsParked)
                {
                    VehiclesViewModel viewModel2 = CreateVehiclesParkedViewModel(vehicle, members);
                    viewModels.Add(viewModel2);
                }
            }

            //var viewModels3 = viewModels.AsQueryable();

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
            return View(new Tuple<IEnumerable<VehiclesViewModel>, IEnumerable<SelectListItem>>
                                 (viewModels, vehicleTypeSelectItems));
        }

        public async Task<IActionResult> Filter(VehiclesViewModel viewModel)
        {
            var vehicles = string.IsNullOrWhiteSpace(viewModel.RegNo) ?
                            _context.Vehicle :
                            _context.Vehicle.Where(m => m.RegNo.StartsWith(viewModel.RegNo));

            vehicles = viewModel.VehicleType == null ?
                            vehicles :
                            vehicles.Where(m => m.VehicleType == viewModel.VehicleType);

            var model = new VehiclesViewModel
            {
                Vehicles = await vehicles.ToListAsync()
            };

            return View(nameof(Index), model);

        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            ViewData["MemberId"] = new SelectList(_context.Set<Member>(), "Id", "PersonNo");
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RegNo,Color,Brand,Model,Wheels,MemberId")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Set<Member>(), "Id", "PersonNo", vehicle.MemberId);
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.Set<Member>(), "Id", "PersonNo", vehicle.MemberId);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RegNo,Color,Brand,Model,Wheels,MemberId")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Set<Member>(), "Id", "PersonNo", vehicle.MemberId);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            _context.Vehicle.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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

        private static VehiclesViewModel CreateVehiclesParkedViewModel(Vehicle vehicle, IEnumerable<Member> members)
        {
            var model = new VehiclesViewModel();
            var member = members.FirstOrDefault(m => m.Id == vehicle.MemberId);
            model.Id = vehicle.Id;
            model.RegNo = vehicle.RegNo;
            model.ParkedTime = DateTime.Now - vehicle.ArrivalTime;
            model.VehicleType = vehicle.VehicleType;
            model.FullName = member.FullName;
            model.MembershipLevel = member.MembershipLevel;
            return model;
        }
    }
}
