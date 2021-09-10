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

        public async Task<IEnumerable<SelectListItem>> GetGenresAsync()
        {
            return await db.Vehicle
                        .Select(m => m.VehicleType)
                        .Distinct()
                        .Select(g => new SelectListItem
                        {
                            Text = g.ToString(),
                            Value = g.ToString()
                        })
                        .ToListAsync();
        }
    }
}
