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
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber, VehiclesParkedViewModel viewModel)
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

            var viewModel1 = vehicles.Select(v => new VehiclesParkedViewModel
            {
                RegNo = v.RegNo,
                FullName = v.Member.FullName,
                MembershipLevel = v.Member.MembershipLevel,
                VehicleType = v.VehicleType,
                ParkedTime = DateTime.Now.Subtract(v.ArrivalTime)
            });

            switch (sortOrder)
            {
                case "regNo_desc":
                    viewModel1 = viewModel1.OrderByDescending(s => s.RegNo);
                    break;
                case "FullName":
                    viewModel1 = viewModel1.OrderBy(s => s.FullName);
                    break;
                case "fullName_desc":
                    viewModel1 = viewModel1.OrderByDescending(s => s.FullName);
                    break;
                case "MembershipLevel":
                    viewModel1 = viewModel1.OrderBy(s => s.MembershipLevel);
                    break;
                case "membershipLevel_desc":
                    viewModel1 = viewModel1.OrderByDescending(s => s.MembershipLevel);
                    break;
                case "VehicleType":
                    viewModel1 = viewModel1.OrderBy(s => s.VehicleType);
                    break;
                case "vehicleType_desc":
                    viewModel1 = viewModel1.OrderByDescending(s => s.VehicleType);
                    break;
                case "ParkedTime":
                    viewModel1 = viewModel1.OrderBy(s => s.ParkedTime);
                    break;
                case "parkedTime_desc":
                    viewModel1 = viewModel1.OrderByDescending(s => s.ParkedTime);
                    break;
                default:
                    viewModel1 = viewModel1.OrderBy(s => s.RegNo);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<VehiclesParkedViewModel>.CreateAsync(viewModel1.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Filter(VehiclesParkedViewModel viewModel)
        {
            var vehicles = string.IsNullOrWhiteSpace(viewModel.RegNo) ?
                            _context.Vehicle :
                            _context.Vehicle.Where(m => m.RegNo.StartsWith(viewModel.RegNo));

            vehicles = viewModel.VehicleType == null ?
                            vehicles :
                            vehicles.Where(m => m.VehicleType == viewModel.VehicleType);

            var model = new VehiclesParkedViewModel
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
    }
}
