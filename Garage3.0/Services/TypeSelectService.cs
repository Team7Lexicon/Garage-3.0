using Garage3._0.Data;
using Garage3._0.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3._0.Services
{
    public class TypeSelectService : ITypeSelectService
    {
        private readonly Garage3_0Context db;

        public TypeSelectService(Garage3_0Context db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<SelectListItem>> GetTypeAsync()
        {
            return await db.Vehicle
                        .Select(m => m.VehicleType)
                        .Distinct()
                        .Select(g => new SelectListItem
                        {
                            Text = g.Name.ToString(),
                            Value = g.Id.ToString()
                        })
                        .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetVehicleTypeAsync()
        {
            return await db.VehicleType.OrderBy(v => v.Name)
                    .Select(r => new SelectListItem
                    {
                    Text = r.Name.ToString(),
                    Value = r.Id.ToString()
                    }).ToListAsync();
        }
    }
}