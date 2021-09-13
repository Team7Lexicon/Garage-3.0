using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage3._0.Data;
using Garage3._0.Models;
using Garage3._0.Models.ViewModels;

namespace Garage3._0.Controllers
{
    public class MembersController : Controller
    {
        private readonly Garage3_0Context _context;

        public MembersController(Garage3_0Context context)
        {
            _context = context;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            //TODO: Implement OrderBy without error
            var model = await _context.Member.Include(i => i.Vehicles)
                        .Select(m => new MemberViewModel
                        {
                            Id = m.Id,
                            PersonNo = m.PersonNo,
                            FullName = $"{m.FirstName} {m.LastName}",
                            NoOfVehicles = m.Vehicles.Count
                        })
                        .ToListAsync();

            var model2 = model.AsQueryable().OrderBy(m => m.FullName).ToList();
            
            return View(model2);
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
 //               .Include(t => t.VehicleType)
                .Where(m => m.Id==id)
 //               .Select(t => t.VehicleType.Id = m.Vehicle.VehicleTypeId)
                .Select(m => new MemberViewDetailsModel
                {
                    Id = m.Id,
                    PersonNo = m.PersonNo,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Email = m.Email,
                    MembershipLevel = m.MembershipLevel,
                    Vehicles = m.Vehicles
                }
                ).FirstOrDefaultAsync();
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        /*public IActionResult CreateMember()
        {
            return View("MembersCreateView");
        }*/

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PersonNo,FirstName,LastName,Email,RegistrationTime,Password,MembershipLevel")] Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                member.RegistrationTime = DateTime.Now;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PersonNo,FirstName,LastName,Email,RegistrationTime,Password,MembershipLevel")] Member member)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
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
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Member.FindAsync(id);
            _context.Member.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Member.Any(e => e.Id == id);
        }
    }
}
