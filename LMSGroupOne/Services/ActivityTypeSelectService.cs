using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Data.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LMSGroupOne.Services
{
    public class ActivityTypeSelectService : IActivityTypeSelectService
    {
        private readonly ApplicationDbContext db;

        public ActivityTypeSelectService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<SelectListItem>> GetActivityTypesAsync()
        {
            return await db.ActivityTypes
                    .Select(at => new SelectListItem
                    {
                        Text = at.Name.ToString(),
                        Value = at.Id.ToString()
                    })
                    .ToListAsync();
        }
    }
}
